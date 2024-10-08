namespace XHTD.CORE.Entities;

public partial class Item
{
    public Guid Id { get; set; }

    public string? Code { get; set; }

    public string? Name { get; set; }

    public string? UnitCode { get; set; }

    public string? UnitName { get; set; }

    public string? TypeCode { get; set; }

    public string? TypeName { get; set; }

    public string? UnitCode1 { get; set; }

    public string? TypeCode1 { get; set; }

    public double? CostPrice { get; set; }

    public double? SellPrice { get; set; }

    public double? Proportion { get; set; }

    public double? PercentageOfImpurities { get; set; }

    public bool? IsMainObject { get; set; }

    public bool? IsQuantitative { get; set; }

    public double? Cement { get; set; }

    public double? Stone { get; set; }

    public double? Sand { get; set; }

    public double? Admixture { get; set; }

    public double? Water { get; set; }

    public Guid? ItemFormulaId { get; set; }

    public virtual ItemFormula? ItemFormula { get; set; }
}
