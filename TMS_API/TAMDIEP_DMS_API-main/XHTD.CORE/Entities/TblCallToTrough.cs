namespace XHTD.CORE.Entities;

public partial class TblCallToTrough
{
    public int Id { get; set; }

    public int OrderId { get; set; }

    public string? DeliveryCode { get; set; }

    public string? Vehicle { get; set; }

    public decimal? SumNumber { get; set; }

    public string Machine { get; set; } = null!;

    public int? IndexTrough { get; set; }

    public int CountTry { get; set; }

    public int CountReindex { get; set; }

    public bool? IsDone { get; set; }

    public string? CallLog { get; set; }

    public DateTime? CreateDay { get; set; }

    public DateTime? UpdateDay { get; set; }
}
