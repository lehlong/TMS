namespace XHTD.CORE.Entities;

public partial class TblCompany
{
    public string ComId { get; set; } = null!;

    public string? ComManager { get; set; }

    public string? ComName { get; set; }

    public string? ComAddress { get; set; }

    public string? ComPhone { get; set; }

    public string? ComFax { get; set; }

    public string? ComEmail { get; set; }

    public string? ComTax { get; set; }

    public string? ComWebsite { get; set; }

    public DateTime? CreateDay { get; set; }

    public string? CreateBy { get; set; }

    public DateTime? UpdateDay { get; set; }

    public string? UpdateBy { get; set; }
}
