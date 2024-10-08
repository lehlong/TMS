namespace XHTD.CORE.Entities;

public partial class TblStoreOrderOperatingVoice
{
    public int Id { get; set; }

    public int? OrderId { get; set; }

    public string? DeliveryCode { get; set; }

    public int? Step { get; set; }

    public string? VoiceText { get; set; }

    public int? IndexNumber { get; set; }

    public DateTime? CreateDay { get; set; }

    public string? CreateBy { get; set; }

    public DateTime? UpdateDay { get; set; }

    public string? UpdateBy { get; set; }
}
