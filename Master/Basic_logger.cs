using System.Text;
using Newtonsoft.Json;
using agit.Api.Helpers;

namespace agit.Api.Master;

public class Basic_logger
{
    private readonly ILogger<Basic_logger> _logger;

    public Basic_logger(ILogger<Basic_logger> logger)
    {
        _logger = logger;
    }

    public void Log(string level, string type, string source, string message, object data = null)
    {
        var payload = JsonConvert.SerializeObject(data);
        switch (level)
        {
            case "Information":
                _logger.LogInformation($"\n{type} -=> {source}\n{message} : {payload}\n");
                break;
            case "Debug":
                _logger.LogDebug($"\n{type} -=> {source}\n{message} : {payload}\n");
                break;
            case "Trace":
                _logger.LogTrace($"\n{type} -=> {source}\n{message} : {payload}\n");
                break;
            case "Warning":
                _logger.LogWarning($"\n{type} -=> {source}\n{message} : {payload}\n");
                break;
            case "Error":
                _logger.LogError($"\n{type} -=> {source}\n{message} : {payload}\n");
                break;
            case "Critical":
                _logger.LogCritical($"\n{type} -=> {source}\n{message} : {payload}\n");
                break;
        }
    }

    public void Debug(string source, string message, object data = null)
    {
        _logger.LogInformation($"\n{source} -=> {message}\n{data}");
    }

    public void Custom_Specific_Log(string source, string typeLog, string payload)
    {
        var loggerContent = new StringBuilder();
        loggerContent.AppendLine($"\n=== {source} Logger Information Start ===");
        loggerContent.AppendLine($"\n {source} {typeLog} -=> {payload}");
        loggerContent.AppendLine($"\n=== {source} Logger Information END ===");
        _logger.LogInformation(loggerContent.ToString());
    }

    public void Custom_external_api_logger(string source, string type, string url, string header,
        string body = null)
    {
        var loggerContent = new StringBuilder();
        loggerContent.AppendLine(
            $"\n***XXX*** {type} CALL API Start ***XXX***");
        loggerContent.AppendLine($"\n -=> {url}");
        loggerContent.AppendLine($"\n Header -=> {header}");
        loggerContent.AppendLine($"\n {type} -=> {body}");
        loggerContent.AppendLine(
            $"\n***XXX*** {type} CALL API END ***XXX***");
        _logger.LogInformation(loggerContent.ToString());
    }


    public void Create_sql_log(string source, string typeLog, string query)
    {
        if (Helper.Hel_convert_string_to_bool(Basic_configuration.Get_variable_global("ENABLE_LOG_SQL")))
            Custom_Specific_Log(source, typeLog, query);
    }
}
