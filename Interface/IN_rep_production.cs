using agit.Api.Models;

namespace agit.Api.Interface;

public interface IN_rep_production
{
    Task<bool> Rep_production_insert(Mod_base_production modProduction);
}
