namespace XHTD.CORE.Entities;

public partial class TblAccountGroupFunction
{
    public int Id { get; set; }

    public int? GroupId { get; set; }

    public int? FunctionId { get; set; }

    public bool? FAdd { get; set; }

    public bool? FEdit { get; set; }

    public bool? FDel { get; set; }

    public bool? FView { get; set; }

    public bool? FPrint { get; set; }

    public bool? FOther { get; set; }

    public DateTime? CreateDay { get; set; }

    public string? CreateBy { get; set; }

    public DateTime? UpdateDay { get; set; }

    public string? UpdateBy { get; set; }
}
