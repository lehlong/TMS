using AutoMapper;
using Common;
using DMS.BUSINESS.Common.Enum;
using DMS.CORE.Entities.MD;
using DMS.CORE.Entities.SO;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DMS.BUSINESS.Dtos.SO
{
    public class OrderDto : BaseDto, IMapFrom, IDto
    {
        [Key]
        public string Code { get; set; }

        public int? PartnerIdBuy { get; set; }

        public int? PartnerIdSell { get; set; }

        public int? AreaId { get; set; }

        public string? VehicleCode { get; set; }

        public int? DriverId { get; set; }

        public DateTime? OrderDate { get; set; }

        public string? DeliveryCode { get; set; }

        public string? Type { get; set; }

        public string? ParentCode { get; set; }

        public string? State { get; set; }

        public string? Notes { get; set; }

        public string? ContractId { get; set; }

        public Guid? ReferenceId { get; set; }

        public virtual DriverDto Driver { get; set; }

        public virtual PartnerLiteDto PartnerBuy { get; set; }

        public virtual PartnerLiteDto PartnerSell { get; set; }

        public virtual AreaDto Area { get; set; }

        public virtual VehicleDto Vehicle { get; set; }

        public List<OrderDetailDto> Details { get; set; }

        public List<OrderProcessDto> Processes { get; set; }

        public virtual List<OrderChildDto> Childs { get; set; }

        public virtual OrderParentDto Parent { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<OrderDto, TblSoOrder>().ReverseMap();
        }
    }

    public class OrderChildDto : BaseDto, IMapFrom, IDto
    {
        [Key]
        public string Code { get; set; }

        public int? PartnerIdBuy { get; set; }

        public int? PartnerIdSell { get; set; }

        public int? AreaId { get; set; }

        public string? VehicleCode { get; set; }

        public int? DriverId { get; set; }

        public DateTime? OrderDate { get; set; }

        public string? DeliveryCode { get; set; }

        public string? Type { get; set; }

        public string? ParentCode { get; set; }

        public string? State { get; set; }

        public string? Notes { get; set; }

        public string? ContractId { get; set; }

        public Guid? ReferenceId { get; set; }

        public virtual DriverDto Driver { get; set; }

        public virtual PartnerLiteDto PartnerBuy { get; set; }

        public virtual PartnerLiteDto PartnerSell { get; set; }

        public virtual AreaDto Area { get; set; }

        public virtual VehicleDto Vehicle { get; set; }

        public List<OrderDetailDto> Details { get; set; }

        public List<OrderProcessDto> Processes { get; set; }

        public virtual List<OrderChildDto> Childs { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<OrderChildDto, TblSoOrder>().ReverseMap();
        }
    }

    public class OrderParentDto : BaseDto, IMapFrom, IDto
    {
        [Key]
        public string Code { get; set; }

        public int? PartnerIdBuy { get; set; }

        public int? PartnerIdSell { get; set; }

        public int? AreaId { get; set; }

        public string? VehicleCode { get; set; }

        public int? DriverId { get; set; }

        public DateTime? OrderDate { get; set; }

        public string? DeliveryCode { get; set; }

        public string? Type { get; set; }

        public string? ParentCode { get; set; }

        public string? State { get; set; }

        public string? Notes { get; set; }

        public string? ContractId { get; set; }

        public Guid? ReferenceId { get; set; }

        public virtual DriverDto Driver { get; set; }

        public virtual PartnerLiteDto PartnerBuy { get; set; }

        public virtual PartnerLiteDto PartnerSell { get; set; }

        public virtual AreaDto Area { get; set; }

        public virtual VehicleDto Vehicle { get; set; }

        public List<OrderDetailDto> Details { get; set; }

        public List<OrderProcessDto> Processes { get; set; }

        public virtual OrderParentDto Parent { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<OrderParentDto, TblSoOrder>().ReverseMap();
        }
    }

    public class OrderCreateDto : IMapFrom, IDto
    {
        [JsonIgnore]
        public string? Code { get; set; }

        public int? PartnerIdBuy { get; set; }

        public int? PartnerIdSell { get; set; }

        public int? AreaId { get; set; }

        public string? VehicleCode { get; set; }

        public int? DriverId { get; set; }

        public DateTime? OrderDate { get; set; }

        public string? DeliveryCode { get; set; }

        [JsonIgnore]
        public string? Type { get; set; }

        public string? Notes { get; set; }

        [JsonIgnore]
        public string State { get => OrderState.KHOI_TAO.ToString(); }

        public Guid? ReferenceId { get; set; } = Guid.NewGuid();

        public string? ContractId { get; set; }

        public List<OrderDetailCreateDto>? Details { get; set; }

        public List<OrderChildLiteDto>? Childs { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<OrderCreateDto, TblSoOrder>().ForMember(x => x.Childs, y => y.Ignore()).ReverseMap();
        }
    }

    public class OrderUpdateDto : IMapFrom, IDto
    {
        [Key]
        public string Code { get; set; }

        public int? AreaId { get; set; }

        public string? VehicleCode { get; set; }

        public int? DriverId { get; set; }

        public DateTime? OrderDate { get; set; }

        public string? DeliveryCode { get; set; }

        public string? ParentCode { get; set; }

        public string? Notes { get; set; }

        public string? ContractId { get; set; }

        public List<OrderDetailCreateDto> Details { get; set; }

        public List<OrderChildLiteDto>? Childs { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<OrderUpdateDto, TblSoOrder>().ForMember(x => x.Childs, y => y.Ignore()).ReverseMap();
        }
    }

    public class OrderUpdateStateDto : IMapFrom, IDto
    {
        [Key]
        public string Code { get; set; }

        [JsonIgnore]
        public string? State { get; set; }

        public string? Comment { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblSoOrder, OrderUpdateStateDto>().ReverseMap();
        }
    }
    public class OrderChildLiteDto : IMapFrom, IDto
    {
        [Key]
        public string Code { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblSoOrder, OrderChildLiteDto>().ReverseMap();
        }
    }
}
