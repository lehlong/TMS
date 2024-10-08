using System.ComponentModel.DataAnnotations.Schema;

namespace XHTD.CORE.Entities;

public partial class TblTroughTypeProduct
{
    public int Id { get; set; }

    public string TroughCode { get; set; } = null!;

    public string TypeProduct { get; set; } = null!;

    public DateTime? CreateDay { get; set; }

    public string? CreateBy { get; set; }

    public DateTime? UpdateDay { get; set; }

    public string? UpdateBy { get; set; }

    [ForeignKey("TroughCode")]
    public virtual TblTrough Trough { get; set; }

    [ForeignKey("TypeProduct")]
    public virtual TblTypeProduct TypeProductITem { get; set; }
}
