namespace XHTD.CORE.Entities;

public partial class TblCategoriesDevicesLog
{
    public int Id { get; set; }

    public string? Code { get; set; }

    /// <summary>
    /// 1: Mở; 2: Đóng; 3: Khác
    /// </summary>
    public int? ActionType { get; set; }

    public string? ActionInfo { get; set; }

    public DateTime? ActionDate { get; set; }

    public DateTime? CreateDay { get; set; }

    public string? CreateBy { get; set; }

    public DateTime? UpdateDay { get; set; }

    public string? UpdateBy { get; set; }
}
