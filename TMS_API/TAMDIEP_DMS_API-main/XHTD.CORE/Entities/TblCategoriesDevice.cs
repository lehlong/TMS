namespace XHTD.CORE.Entities;

public partial class TblCategoriesDevice
{
    public int Id { get; set; }

    /// <summary>
    /// Mã hạng mục
    /// </summary>
    public string? CatCode { get; set; }

    /// <summary>
    /// Mã thiết bị quản lý
    /// </summary>
    public string? ManCode { get; set; }

    /// <summary>
    /// Mã thiết bị
    /// </summary>
    public string Code { get; set; } = null!;

    public string? Name { get; set; }

    public string? IpAddress { get; set; }

    public int? PortNumber { get; set; }

    public int? PortNumberDeviceIn { get; set; }

    public int? PortNumberDeviceOut { get; set; }

    public int? PortNumberDeviceIn1 { get; set; }

    public int? PortNumberDeviceOut1 { get; set; }

    public int? PortNumberDeviceIn2 { get; set; }

    public int? PortNumberDeviceOut2 { get; set; }

    public string? Descriptioon { get; set; }

    public bool? State { get; set; }

    public int? ShowIndex { get; set; }

    public DateTime? CreateDay { get; set; }

    public string? CreateBy { get; set; }

    public DateTime? UpdateDay { get; set; }

    public string? UpdateBy { get; set; }
}
