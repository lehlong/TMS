namespace XHTD.CORE.Entities;

public partial class TblDriver
{
    public int Id { get; set; }

    public string? FullName { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public DateTime? Birthday { get; set; }

    public string? Gender { get; set; }

    public string? IdCard { get; set; }

    public string? IdCardImgFront { get; set; }

    public string? IdCardImgBack { get; set; }

    public string? Address { get; set; }

    public string? UserName { get; set; }

    public bool? State { get; set; }

    public DateTime? CreateDay { get; set; }

    public string? CreateBy { get; set; }

    public DateTime? UpdateDay { get; set; }

    public string? UpdateBy { get; set; }
}
