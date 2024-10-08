namespace XHTD.CORE.Entities;

public partial class TblPrintConfig
{
    public int Id { get; set; }

    public string? KeyName { get; set; }

    public string? ValueName { get; set; }

    public DateTime? CreateDay { get; set; }

    public string? CreateBy { get; set; }

    public DateTime? UpdateDay { get; set; }

    public string? UpdateBy { get; set; }
}
