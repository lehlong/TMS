using DMS.CORE.Common;
using DMS.CORE.Entities.MD;
using DMS.CORE.Entities.MN;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMS.CORE.Entities.SO
{
    [Table("T_SO_ORDER")]
    public class TblSoOrder : ReferenceEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("CODE",TypeName = "VARCHAR2(50)")]
        public string Code { get; set; }

        [Column("PARTNER_ID_BUY")]
        public int? PartnerIdBuy { get; set; }

        [Column("PARTNER_ID_SELL")]
        public int? PartnerIdSell { get; set; }

        [Column("AREA_ID")]
        public int? AreaId { get; set; }

        [Column("VEHICLE_CODE")]
        public string? VehicleCode { get; set; }

        [Column("DRIVER_ID")]
        public int? DriverId { get; set; }

        [Column("ORDER_DATE")]
        public DateTime? OrderDate { get; set; }

        [Column("DELIVERY_CODE", TypeName = "VARCHAR2(20)")]
        public string? DeliveryCode { get; set; }

        [Column("TYPE",TypeName = "VARCHAR2(15)")]
        public string? Type { get; set; }

        [Column("PARENT_CODE")]
        public string? ParentCode { get; set; }

        [Column("STATE",TypeName = "VARCHAR2(20)")]
        public string? State { get; set; }

        [Column("NOTES",TypeName = "NVARCHAR2(500)")]
        public string? Notes { get; set; }

        [Column("IS_SYNC_ERP")]
        public bool? IsSyncErp { get; set; }

        [Column("CONTRACT_ID", TypeName = "VARCHAR2(50)")]
        public string? ContractId { get; set; }

        public virtual TblMnContract Contract { get; set; }

        [ForeignKey("ParentCode")]
        public virtual TblSoOrder Parent { get; set; }

        public virtual List<TblSoOrder> Childs { get; set; }

        [ForeignKey("DriverId")]
        public virtual TblMdDriver Driver { get; set; }

        [ForeignKey("PartnerIdBuy")]
        public virtual TblMdPartner? PartnerBuy { get; set; }

        [ForeignKey("PartnerIdSell")]
        public virtual TblMdPartner? PartnerSell { get; set; }

        [ForeignKey("AreaId")]
        public virtual TblMdArea Area { get; set; }

        [ForeignKey("VehicleCode")]
        public virtual TblMdVehicle Vehicle { get; set;}

        public List<TblSoOrderDetail> Details { get; set; }

        public List<TblSoOrderProcess> Processes { get; set; }
    }
}
