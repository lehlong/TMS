namespace XHTD.CORE.Entities;

public partial class TblStoreOrderOperatingPriority
{
    public int Id { get; set; }

    public string? TypeProduct { get; set; }

    public int Priority { get; set; }

    public DateTime? CreateDay { get; set; }

    public string? CreateBy { get; set; }

    public DateTime? UpdateDay { get; set; }

    public string? UpdateBy { get; set; }
}
