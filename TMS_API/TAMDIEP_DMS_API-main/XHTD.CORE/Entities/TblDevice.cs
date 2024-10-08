namespace XHTD.CORE.Entities;

public partial class TblDevice
{
    public int Id { get; set; }

    public string CodeParent { get; set; } = null!;

    public string Code { get; set; } = null!;

    public string? Name { get; set; }

    public int? OperId { get; set; }

    public int? DoorOrAuxoutId { get; set; }

    public int? OutputAddrType { get; set; }

    public int? DoorAction { get; set; }

    public int? InputPort { get; set; }

    public int? OutputPort { get; set; }

    public string? Ipaddress { get; set; }

    public string? Port { get; set; }

    public DateTime? CreateDay { get; set; }

    public string? CreateBy { get; set; }

    public DateTime? UpdateDay { get; set; }

    public string? UpdateBy { get; set; }
}
