namespace agit.Api.Models;

public class Mod_base_production
{
    public int Id { get; set; }
    public int Senin { get; set; }
    public int Selasa { get; set; }
    public int Rabo { get; set; }
    public int Kamis { get; set; }
    public int Jumat { get; set; }
    public int Sabtu { get; set; }
    public int Minggu { get; set; }
    public int Remap_senin { get; set; }
    public int Remap_selasa { get; set; }
    public int Remap_rabo { get; set; }
    public int Remap_kamis { get; set; }
    public int Remap_jumat { get; set; }
    public int Remap_sabtu { get; set; }
    public int Remap_minggu { get; set; }
    public DateTime Created_at { get; set; }
}
