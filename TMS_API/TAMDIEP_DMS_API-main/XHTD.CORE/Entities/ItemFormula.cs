namespace XHTD.CORE.Entities;

public partial class ItemFormula
{
    public Guid Id { get; set; }

    public string? ItemCode { get; set; }

    public double? Cement { get; set; }

    public double? Stone { get; set; }

    public double? Sand { get; set; }

    public double? Admixture { get; set; }

    public double? Water { get; set; }

    public virtual ICollection<Item> Items { get; set; } = new List<Item>();
}
