namespace XHTD.CORE.Entities;

public partial class TblLongVehicle
{
    public int Id { get; set; }

    public string? Vehicle { get; set; }

    public DateTime? CreateDay { get; set; }

    public string? CreateBy { get; set; }

    public DateTime? UpdateDay { get; set; }

    public string? UpdateBy { get; set; }
}
