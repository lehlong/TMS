namespace XHTD.CORE.Entities;

public partial class TblDeviceOperating
{
    public int Id { get; set; }

    public int? GroupDevice { get; set; }

    public string? GroupDeviceCode { get; set; }

    public string? GroupDeviceName { get; set; }

    public string Code { get; set; } = null!;

    public string? Name { get; set; }

    public string? IpAddress { get; set; }

    public int? PortNumber { get; set; }

    public bool? State { get; set; }

    public DateTime? DayCreate { get; set; }

    public string? LogHistory { get; set; }

    public int? Flag { get; set; }
}
