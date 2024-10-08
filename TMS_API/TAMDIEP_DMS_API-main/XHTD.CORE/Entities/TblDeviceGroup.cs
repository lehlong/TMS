namespace XHTD.CORE.Entities;

public partial class TblDeviceGroup
{
    public int Id { get; set; }

    /// <summary>
    /// 0: Chưa xác định; 1: Thiết bị; 2: Server
    /// </summary>
    public int? TypeId { get; set; }

    public string Code { get; set; } = null!;

    public string? Name { get; set; }

    public string? IpAddress { get; set; }

    public int? PortNumber { get; set; }

    public bool? State { get; set; }

    public DateTime? CreateDay { get; set; }

    public string? CreateBy { get; set; }

    public DateTime? UpdateDay { get; set; }

    public string? UpdateBy { get; set; }
}
