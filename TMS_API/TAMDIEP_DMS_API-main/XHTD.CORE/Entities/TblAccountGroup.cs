namespace XHTD.CORE.Entities;

public partial class TblAccountGroup
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public bool? State { get; set; }

    public DateTime? CreateDay { get; set; }

    public string? CreateBy { get; set; }

    public DateTime? UpdateDay { get; set; }

    public string? UpdateBy { get; set; }
}
