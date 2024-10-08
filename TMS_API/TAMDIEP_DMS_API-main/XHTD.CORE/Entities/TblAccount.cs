namespace XHTD.CORE.Entities;

public partial class TblAccount
{
    public int Id { get; set; }

    public string UserName { get; set; } = null!;

    public string? FullName { get; set; }

    public string? PassWord { get; set; }

    public int? GroupId { get; set; }

    public bool? State { get; set; }

    public string? DeviceId { get; set; }

    public DateTime? DeviceIdDayUpdate { get; set; }

    public DateTime? CreateDay { get; set; }

    public string? CreateBy { get; set; }

    public DateTime? UpdateDay { get; set; }

    public string? UpdateBy { get; set; }
}
