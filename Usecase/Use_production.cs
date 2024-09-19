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
    private readonly IN_rep_production _repository_production;

    public Use_production(Basic_response basic_response, IN_rep_production rep_production)
    {
        _source = GetType().Name;
        _basic_response = basic_response;
        _repository_production = rep_production;
    }

    public async Task<Mod_base_data_response> Use_post_task1(Req_task1 request)
    {
        int[] hari = new int[] { request.Senin, request.Selasa, request.Rabo, request.Kamis, request.Jumat };
        int total = hari.Sum();
        int average = total / hari.Length;
        var overtime = total % hari.Length;

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

    public async Task<Mod_base_data_response> Use_post_task2(Req_task2 request)
    {
        int[] hari = new int[] { request.Senin, request.Selasa, request.Rabo, request.Kamis, request.Jumat, request.Sabtu, request.Minggu };
        var libur = 0;

        foreach (var item in hari)
        {
            if (item == 0) libur++;

        }

        int total_produksi_seminggu = hari.Sum();
        int hari_produksi = hari.Length - libur;
        int minim_produksi = total_produksi_seminggu / hari_produksi;
        int hari_overload = 0;

        foreach (var item in hari)
        {
            if (item > minim_produksi)
            {
                hari_overload++;
            }
        }

        int har_produksi_normal = hari_produksi - hari_overload;
        int max_produksi = (total_produksi_seminggu - (har_produksi_normal * minim_produksi)) / hari_overload;

        // rata rata 4
        // tota 27
        // up 3
        // produksi 6 hr
        // up total  27 -(4 * 3) = 15
        // up 15 / 3 = 5

        for (int i = 0; i < hari.Length; i++)
        {
            if (hari[i] > max_produksi)
            {
                hari[i] = max_produksi;
            }

            if (hari[i] < minim_produksi && hari[i] != 0)
            {
                hari[i] = minim_produksi;
            }
        }

        var result = new Res_task2
        {
            Senin = hari[0],
            Selasa = hari[1],
            Rabo = hari[2],
            Kamis = hari[3],
            Jumat = hari[4],
            Sabtu = hari[5],
            Minggu = hari[6],
        };

        var payload = new Mod_base_production { };
        payload.Senin = request.Senin;
        payload.Selasa = request.Selasa;
        payload.Rabo = request.Rabo;
        payload.Kamis = request.Kamis;
        payload.Jumat = request.Jumat;
        payload.Sabtu = request.Sabtu;
        payload.Minggu = request.Minggu;
        payload.Remap_senin = result.Senin;
        payload.Remap_selasa = result.Selasa;
        payload.Remap_rabo = result.Rabo;
        payload.Remap_kamis = result.Kamis;
        payload.Remap_jumat = result.Jumat;
        payload.Remap_sabtu = result.Sabtu;
        payload.Remap_minggu = result.Minggu;
        await _repository_production.Rep_production_insert(payload);

        return _basic_response.Reverse_success_data_response(
            Basic_code.Http_code_general, Basic_message.Message_login_success, payload);
    }

    public async Task<Mod_base_production[]> Use_get_data_production()
    {
        var result = await _repository_production.Rep_production_get_all();

        return result;
    }
}


