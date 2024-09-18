using System.Dynamic;
using System.Text;
using agit.Api.Master;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace agit.Api.Middlewares;

public class Mid_base_logger
{
    private readonly RequestDelegate _next;
    private readonly ILogger<Mid_base_logger> _logger;

    public Mid_base_logger(RequestDelegate next, ILogger<Mid_base_logger> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        var enableLogAPi = Basic_configuration.Get_variable_global("ENABLE_LOG_API");
        var flag = Helpers.Helper.Hel_convert_string_to_bool(enableLogAPi);


        if (flag)
        {
            await create_request_log(context);
            var originalResponseBody = context.Response.Body;

            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;
                await _next.Invoke(context);

                var contentType = get_content_type(context.Response.ContentType);

                if (contentType == "application/octet-stream")
                {
                    responseBody.Seek(0, SeekOrigin.Begin);
                    await responseBody.CopyToAsync(originalResponseBody);
                    responseBody.Dispose();
                    return;
                }

                await create_response_log(context, responseBody, originalResponseBody);
                // Dispose MemoryStream
                responseBody.Dispose();
            }
        }
        else
        {
            await _next.Invoke(context);
        }
    }

    private bool is_sensitive_field(string key)
    {
        switch (key.ToLower())
        {
            case "password":
                return true;
            default:
                return false;
        }
    }

    private JObject replace_sensitive_fields(JObject obj)
    {
        var modifiedObject = new JObject();
        if (obj == null) return obj;

        foreach (var property in obj.Properties())
            if (property.Value.Type == JTokenType.Object)
            {
                var modifiedChild = replace_sensitive_fields((JObject)property.Value);
                modifiedObject.Add(property.Name, modifiedChild);
            }
            else if (property.Value.Type == JTokenType.Array)
            {
                var modifiedArray = new JArray();

                foreach (var arrayItem in (JArray)property.Value)
                    if (arrayItem.Type == JTokenType.Object)
                    {
                        var modifiedArrayItem = replace_sensitive_fields((JObject)arrayItem);
                        modifiedArray.Add(modifiedArrayItem);
                    }
                    else
                    {
                        modifiedArray.Add(arrayItem);
                    }

                modifiedObject.Add(property.Name, modifiedArray);
            }
            else if (is_sensitive_field(property.Name))
            {
                modifiedObject.Add(property.Name, new JValue("###"));
            }
            else
            {
                modifiedObject.Add(property.Name, property.Value);
            }

        return modifiedObject;
    }

    private async Task<string> set_sensitive_fields_async(string contentJSONBody)
    {
        var objectContent = JsonConvert.DeserializeObject<JObject>(contentJSONBody);
        var modifiedContent = replace_sensitive_fields(objectContent);
        return JsonConvert.SerializeObject(modifiedContent);
    }

    private async Task<string> set_content_json_body(HttpContext context)
    {
        var requestReader = new StreamReader(context.Request.Body, Encoding.UTF8);
        var content = await requestReader.ReadToEndAsync();
        var objectContent = JsonConvert.DeserializeObject<JObject>(content);
        var modifiedContent = replace_sensitive_fields(objectContent);

        return JsonConvert.SerializeObject(modifiedContent);
    }

    private async Task<string> set_content_form_data_body(HttpContext context)
    {
        var result = "";
        if (context.Request.HasFormContentType)
        {
            var form = await context.Request.ReadFormAsync();

            var formDataObject = new
            {
                Files = form.Files.Select(file => new
                {
                    File_name = file.FileName,
                    ContentType = file.ContentType,
                    Size = file.Length
                }),
                FormFields = form.ToDictionary(kv => kv.Key,
                    kv => is_sensitive_field(kv.Key) ? "###" : kv.Value.ToString())
            };

            result = JsonConvert.SerializeObject(formDataObject);
        }

        return result;
    }

    private ExpandoObject set_content_detail_api(HttpContext context, string apiType)
    {
        dynamic result = new ExpandoObject();
        context.Items["request_id"] = context.Items["request_id"] is null
            ? Guid.NewGuid().ToString()
            : context.Items["request_id"]?.ToString();
        result.request_id = context.Items["request_id"]?.ToString();
        result.method = context.Request.Method.ToUpper();
        result.url = context.Request.Path + context.Request.QueryString;
        result.ip = context.Connection.RemoteIpAddress?.ToString();
        result.apiType = $"{apiType} Payload";
        result.level_name = "DEBUG";
        result.channel = "development";
        result.date = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
        result.timezone = TimeZoneInfo.Local.Id;

        return result;
    }

    private IDictionary<string, object> set_content_header(IHeaderDictionary header)
    {
        IDictionary<string, object> headerData = new Dictionary<string, object> { };
        foreach (var (headerKey, headerValue) in header)
            headerData.Add(headerKey, headerValue[0]);

        return headerData;
    }

    private string get_content_type(string contentType)
    {
        if (!string.IsNullOrEmpty(contentType) && contentType.StartsWith("multipart/form-data"))
            return "multipart/form-data";

        if (!string.IsNullOrEmpty(contentType) && contentType.StartsWith("application/octet-stream"))
            return "application/octet-stream";

        return "application/json";
    }

    private async Task create_request_log(HttpContext context)
    {
        var contentType = get_content_type(context.Request.ContentType);
        var apiType = "Request";
        var contentHeader = set_content_header(context.Request.Headers);
        var contentDetail = set_content_detail_api(context, "Request");

        string contentDetail_String;
        string contentHeader_String;
        string contentBody_String;

        contentDetail_String = JsonConvert.SerializeObject(contentDetail);
        contentHeader_String = JsonConvert.SerializeObject(contentHeader);
        switch (contentType)
        {
            case "multipart/form-data":
                contentBody_String = await set_content_form_data_body(context);
                create_view_logger(apiType, contentDetail_String, contentHeader_String, contentBody_String);
                break;
            case "application/json":
                context.Request.EnableBuffering();
                contentBody_String = await set_content_json_body(context);
                create_view_logger(apiType, contentDetail_String, contentHeader_String, contentBody_String);
                context.Request.Body.Position = 0;
                break;
        }
    }

    private async Task create_response_log(HttpContext context, MemoryStream responseBody, Stream originalResponseBody)
    {
        var apiType = "Response";
        responseBody.Position = 0;
        var contentJSONBody = await new StreamReader(responseBody).ReadToEndAsync();
        responseBody.Position = 0;
        await responseBody.CopyToAsync(originalResponseBody);
        context.Response.Body = originalResponseBody;

        var contentDetail = set_content_detail_api(context, apiType);
        var contentHeader = set_content_header(context.Request.Headers);

        var contentDetail_String = JsonConvert.SerializeObject(contentDetail);
        var contentHeader_String = JsonConvert.SerializeObject(contentHeader);

        // var contentBody_String = JsonConvert.SerializeObject(JsonConvert.DeserializeObject(contentJSONBody));
        var contentBody_String = await set_sensitive_fields_async(contentJSONBody);
        create_view_logger(apiType, contentDetail_String, contentHeader_String, contentBody_String);
    }

    private void create_view_logger(string apiType, string contentDetail_String, string contentHeader_String,
        string contentBody_String)
    {
        var loggerContent = new StringBuilder();
        loggerContent.AppendLine($"\n=== {apiType} Information Start ===");
        loggerContent.AppendLine($"\n{apiType} Detail -=> {contentDetail_String}");
        loggerContent.AppendLine($"\n{apiType} Header -=> {contentHeader_String}");
        loggerContent.AppendLine(
            $"\n{apiType} Body -=> {contentBody_String}");
        loggerContent.AppendLine($"\n=== {apiType} Information End ===");

        _logger.LogInformation(loggerContent.ToString());
    }
}
