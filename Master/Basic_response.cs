using agit.Api.Models;

namespace agit.Api.Master;

public class Basic_response
{
    protected string _source;
    protected readonly Basic_logger _basic_logger;
    protected readonly Basic_configuration _basic_configuration;

    public Basic_response(Basic_logger basic_logger, Basic_configuration basic_configuration)
    {
        _source = GetType().Name;
        _basic_logger = basic_logger;
        _basic_configuration = basic_configuration;
    }


    public Mod_base_data_response Reverse_success_data_response(int statusCode, string message,
        object data = null)
    {
        var response = new Mod_base_data_response();
        response.Http_code = statusCode;
        response.Data = new Mod_base_global_response();
        response.Data.Status_code = Basic_code.Status_code_general;
        response.Data.Title = "Success";
        response.Data.Message = message;
        response.Data.Data = data;

        return response;
    }

    public Mod_base_data_response Reverse_error_data_response(string code, string message,
        object data = null)
    {
        var response = new Mod_base_data_response();
        response.Http_code = Basic_code.Http_error_code_general;
        response.Data = new Mod_base_global_response();
        response.Data.Status_code = code;
        response.Data.Title = "Error";
        response.Data.Message = message;
        response.Data.Data = data;

        return response;
    }
}
