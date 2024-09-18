using agit.Api.Helpers;
using agit.Api.Interface;
using agit.Api.Master;
using agit.Api.Models;
using agit.Api.Request;
using agit.Api.Response;

namespace agit.Api.Usecase;

public class Use_production : IN_use_production
{

    protected string _source;
    protected Basic_response _basic_response;

    public Use_production(Basic_response basic_response)
    {
        _source = GetType().Name;
        _basic_response = basic_response;
    }

    public async Task<Mod_base_data_response> Use_post_task1(Req_task1 request)
    {
        int[] hari = new int[] { request.Senin, request.Selasa, request.Rabo, request.Kamis, request.Jumat };
        int total = hari.Sum();
        int average = total / 5;
        var overtime = total % 5;

        var result = new Res_task1
        {
            Senin = average + overtime / 2,
            Selasa = average,
            Rabo = average,
            Kamis = average + overtime / 2,
            Jumat = average
        };

        if (overtime % 2 != 0) result.Senin += 1;

        return _basic_response.Reverse_success_data_response(
            Basic_code.Http_code_general, Basic_message.Message_login_success, result);
    }
}


