using agit.Api.Interface;
using agit.Api.Master;
using agit.Api.Request;

namespace agit.Api.Controllers;
using Microsoft.AspNetCore.Mvc;

[Route("api/production")]
[ApiController]
public class Production_controller : Basic_controller
{
    private readonly IN_use_production _use_production;

    public Production_controller(Basic_response basic_response, Basic_logger basic_logger,
        Basic_configuration basic_configuration,
        IN_use_production use_production)
        : base(basic_response, basic_logger, basic_configuration)
    {
        _source = GetType().Name;
        _use_production = use_production;
    }

    [HttpPost("task1")]
    public async Task<IActionResult> Con_find_user_by_id([FromBody] Req_task1 request)
    {
        try
        {
            if (!ModelState.IsValid) return Make_validator_error_response(ModelState);
            var result = await _use_production.Use_post_task1(request);
            return Make_sucess_response(result);
        }
        catch (Exception e)
        {
            return Make_error_response(e);
        }
    }

    [HttpPost("task2")]
    public async Task<IActionResult> Con_find_user_by_id([FromBody] Req_task2 request)
    {
        try
        {
            if (!ModelState.IsValid) return Make_validator_error_response(ModelState);
            var result = await _use_production.Use_post_task2(request);
            return Make_sucess_response(result);
        }
        catch (Exception e)
        {
            return Make_error_response(e);
        }
    }

    [HttpGet("task2/view")]
    public async Task<IActionResult> Con_view_data()
    {
        try
        {
            var result = await _use_production.Use_get_data_production();
            return View(result);
        }
        catch (Exception e)
        {
            return Make_error_response(e);
        }
    }

}
