using System.ComponentModel.DataAnnotations;

namespace agit.Api.Request;

public class Req_task1
{
    [Required(ErrorMessage = "Hari senin tidak boleh kosong")]
    //
    public int Senin { get; set; }

    [Required(ErrorMessage = "Hari Selasa tidak boleh kosong")]
    //
    public int Selasa { get; set; }

    [Required(ErrorMessage = "Hari Rabo tidak boleh kosong")]
    //
    public int Rabo { get; set; }

    [Required(ErrorMessage = "Hari Kamis tidak boleh kosong")]
    //
    public int Kamis { get; set; }

    [Required(ErrorMessage = "Hari Jumat tidak boleh kosong")]
    //
    public int Jumat { get; set; }
}
