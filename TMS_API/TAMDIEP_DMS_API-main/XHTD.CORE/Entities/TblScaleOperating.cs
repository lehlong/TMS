namespace XHTD.CORE.Entities;

public partial class TblScaleOperating
{
    public int Id { get; set; }

    public string ScaleCode { get; set; } = null!;

    public string? ScaleName { get; set; }

    public bool? IsScaling { get; set; }

    public string? Vehicle { get; set; }

    public string? CardNo { get; set; }

    public string? DeliveryCode { get; set; }

    public bool? ScaleIn { get; set; }

    public bool? ScaleOut { get; set; }

    public DateTime? TimeIn { get; set; }

    public DateTime? CreateDay { get; set; }

    public DateTime? UpdateDay { get; set; }
}
