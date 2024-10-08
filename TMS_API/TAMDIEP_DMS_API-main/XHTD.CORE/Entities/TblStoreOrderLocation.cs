namespace XHTD.CORE.Entities;

public partial class TblStoreOrderLocation
{
    public int Id { get; set; }

    public int? OrderId { get; set; }

    public string? DeliveryCode { get; set; }

    public string? Vehicle { get; set; }

    public string? DriverUserName { get; set; }

    public string? DriverName { get; set; }

    public decimal? Longitude { get; set; }

    public decimal? Latitude { get; set; }

    public DateTime? CreateDay { get; set; }

    public string? CreateBy { get; set; }

    public DateTime? UpdateDay { get; set; }

    public string? UpdateBy { get; set; }
}
