using agit.Api.Helpers;
using agit.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace agit.Api.Master;

public class Basic_controller : Controller
{
    protected string _source;
    protected Basic_response _basic_response;
    protected readonly Basic_logger _basic_logger;
    protected readonly Basic_configuration _basic_configuration;

    public Basic_controller(Basic_response basic_response, Basic_logger basic_logger,
        Basic_configuration basic_configuration)
    {
        _source = GetType().Name;
        _basic_response = basic_response;
        _basic_logger = basic_logger;
        _basic_configuration = basic_configuration;
    }


    [ApiExplorerSettings(IgnoreApi = true)]
    public ObjectResult Make_sucess_response(Mod_base_data_response modBaseDataResponse)
    {
        return StatusCode(modBaseDataResponse.Http_code, modBaseDataResponse.Data);
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    public ObjectResult Make_error_response(Exception e)
    {
        var responseData = new Mod_base_global_response();
        responseData.Status_code = Basic_code.Status_code_error_general;
        responseData.Title = "Error";
        responseData.Message = Basic_message.Message_error_general;
        responseData.Data = null;

        var enableDebugMessage =
            Helper.Hel_convert_string_to_bool(_basic_configuration.Get_variable("ENABLE_ERROR_DEBUG_MESSAGE"));
        if (enableDebugMessage) responseData.Message = e.Message;
        _basic_logger.Custom_Specific_Log(_source, "Custom Exception Error ", e.Message);

        return StatusCode(StatusCodes.Status500InternalServerError, responseData);
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    public ObjectResult Make_validator_error_response(ModelStateDictionary ModelState)
    {
        var enableDebugMessage =
            Helper.Hel_convert_string_to_bool(_basic_configuration.Get_variable("ENABLE_ERROR_DEBUG_MESSAGE"));
        var errorMessage = string.Join("; ", ModelState.Values
            .SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage));

        if (enableDebugMessage) return Make_error_response(new Exception(errorMessage));

        _basic_logger.Custom_Specific_Log(_source, "Form Validator", errorMessage);
        var responseData = new Mod_base_global_response();
        responseData.Status_code = Basic_code.Status_code_error_general;
        responseData.Title = "Error";
        responseData.Message = Basic_message.Message_error_validate_form;
        responseData.Data = null;

        return StatusCode(StatusCodes.Status500InternalServerError, responseData);
    }
}
