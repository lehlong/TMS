namespace XHTD.CORE.Entities;

public partial class TblVehicle
{
    public int Idvehicle { get; set; }

    public string? Vehicle { get; set; }

    public double? Tonnage { get; set; }

    public double? TonnageDefault { get; set; }

    public string? NameDriver { get; set; }

    public string? IdCardNumber { get; set; }

    public double? HeightVehicle { get; set; }

    public double? WidthVehicle { get; set; }

    public double? LongVehicle { get; set; }

    public int? UnladenWeight1 { get; set; }

    public DateTime? UnladenWeight1LastUpdate { get; set; }

    public int? UnladenWeight2 { get; set; }

    public DateTime? UnladenWeight2LastUpdate { get; set; }

    public int? UnladenWeight3 { get; set; }

    public DateTime? UnladenWeight3LastUpdate { get; set; }

    public bool? IsSetMediumUnladenWeight { get; set; }

    public DateTime? CreateDay { get; set; }

    public string? CreateBy { get; set; }

    public DateTime? UpdateDay { get; set; }

    public string? UpdateBy { get; set; }
}
