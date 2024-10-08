namespace XHTD.CORE.Entities;

public partial class TblRfidSign
{
    public int Id { get; set; }

    public string? Vehicle { get; set; }

    public string? RfidCode { get; set; }

    public string? Image { get; set; }

    public DateTime? CreateDay { get; set; }

    public string? CreateBy { get; set; }

    public DateTime? UpdateDay { get; set; }

    public string? UpdateBy { get; set; }
}
