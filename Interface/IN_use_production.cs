using agit.Api.Models;
using agit.Api.Request;

namespace agit.Api.Interface;

public interface IN_use_production
{
    Task<Mod_base_data_response> Use_post_task1(Req_task1 request);
    Task<Mod_base_data_response> Use_post_task2(Req_task2 request);
}
