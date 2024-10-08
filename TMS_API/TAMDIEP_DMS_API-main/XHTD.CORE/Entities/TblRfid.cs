namespace XHTD.CORE.Entities;

public partial class TblRfid
{
    public int Id { get; set; }

    public string Code { get; set; } = null!;

    public string? Vehicle { get; set; }

    public DateTime? DayReleased { get; set; }

    public DateTime? DayExpired { get; set; }

    public string? Note { get; set; }

    public bool? State { get; set; }

    public DateTime? LastEnter { get; set; }

    public DateTime? CreateDay { get; set; }

    public string? CreateBy { get; set; }

    public DateTime? UpdateDay { get; set; }

    public string? UpdateBy { get; set; }
}
