namespace XHTD.CORE.Entities;

public partial class TblTrough
{
    public int Id { get; set; }

    public string Code { get; set; } = null!;

    public string? Name { get; set; }

    public string? Machine { get; set; }

    public double? Height { get; set; }

    public double? Width { get; set; }

    public double? Long { get; set; }

    public bool? Working { get; set; }

    public bool? Problem { get; set; }

    public bool? State { get; set; }

    public string? DeliveryCodeCurrent { get; set; }

    public double? PlanQuantityCurrent { get; set; }

    public double? CountQuantityCurrent { get; set; }

    public bool? IsPicking { get; set; }

    public string? TransportNameCurrent { get; set; }

    public DateTime? CheckInTimeCurrent { get; set; }

    public bool? IsInviting { get; set; }

    public string? LineCode { get; set; }

    public DateTime? CreateDay { get; set; }

    public string? CreateBy { get; set; }

    public DateTime? UpdateDay { get; set; }

    public string? UpdateBy { get; set; }

    public List<TblTroughTypeProduct> TypeProductReferences { get; set; }
}
