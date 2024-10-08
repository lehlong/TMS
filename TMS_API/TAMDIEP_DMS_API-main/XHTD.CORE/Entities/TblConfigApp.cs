namespace XHTD.CORE.Entities;

public partial class TblConfigApp
{
    public int Id { get; set; }

    public string KeyItem { get; set; } = null!;

    public string? ValueItem { get; set; }

    public DateTime? CreateDay { get; set; }

    public string? CreateBy { get; set; }

    public DateTime? UpdateDay { get; set; }

    public string? UpdateBy { get; set; }
}
