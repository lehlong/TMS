namespace XHTD.CORE.Entities;

public partial class TblNotification
{
    public int Id { get; set; }

    public string? UserNameSender { get; set; }

    public string? UserNameReceiver { get; set; }

    public string? ContentMessage { get; set; }

    public DateTime? DayCreate { get; set; }

    public bool? IsView { get; set; }

    public DateTime? TimeView { get; set; }

    public DateTime? CreateDay { get; set; }

    public string? CreateBy { get; set; }

    public DateTime? UpdateDay { get; set; }

    public string? UpdateBy { get; set; }
}
