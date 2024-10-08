namespace XHTD.CORE.Entities;

public partial class TblMachineTypeProduct
{
    public int Id { get; set; }

    public string MachineCode { get; set; } = null!;

    public string TypeProduct { get; set; } = null!;

    public DateTime? CreateDay { get; set; }

    public string? CreateBy { get; set; }

    public DateTime? UpdateDay { get; set; }

    public string? UpdateBy { get; set; }
}
