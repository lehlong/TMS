using AutoMapper;
using System.ComponentModel.DataAnnotations;
using DMS.BUSINESS.Dtos.MN;
using System.ComponentModel;
using System.Text.Json.Serialization;
using Common;

namespace DMS.CORE.Entities.MD
{
    public class PartnerDto : BaseMdDto, IMapFrom, IDto
    {
        [JsonIgnore]
        [Description("STT")]
        public int OrdinalNumber { get; set; }

        [Key]
        public int Id { get; set; }

        [Description("Mã đối tác")]
        public string Code { get; set; }

        [Description("Tên")]
        public string Name { get; set; }

        [Description("Loại")]
        public string SaleTypeCode { get; set; }

        public bool IsCustomer { get; set; }
         
        public bool IsSupplier { get; set; }

        [Description("Địa chỉ")]
        public string? Address { get; set; }

        [Description("Số điện thoại")]
        public string? PhoneNumber { get; set; }

        [Description("Email")]
        public string? Email { get; set; }

        public int? ProvineId { get; set; }

        public int? DistrictId { get; set; }

        public int? WardId { get; set; }

        [Description("Mô tả")]
        public string? Description { get; set; }

        [Description("Kinh độ")]
        public string? Longitude { get; set; }

        [Description("Vĩ độ")]
        public string? Latitude { get; set; }

        public string? Representative { get; set; }

        [Description("Mã số thuế")]
        public string? TaxNumber { get; set; }

        public Guid? ReferenceId { get; set; }

        public virtual ProvineDto Provine { get; set; }

        public virtual DistrictDto District { get; set; }

        public virtual WardDto Ward { get; set; }

        public virtual SaleTypeDto SaleType { get; set; }

        public virtual List<PartnerReferenceBuyDto> ReferenceBuys { get; set; }

        public virtual List<PartnerReferenceSellDto> ReferenceSells { get; set; }

        public virtual List<PartnerVehicleDto> VehicleReferences { get; set; }

        public virtual List<PartnerAreaWithAreaDto> AreaReferences { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblMdPartner, PartnerDto>().ReverseMap();
        }
    }

    public class PartnerCreateDto : BaseMdDto, IMapFrom, IDto
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public string? SaleTypeCode { get; set; }

        public bool IsCustomer { get; set; }

        public bool IsSupplier { get; set; }

        public string? Address { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Email { get; set; }

        public int? ProvineId { get; set; }

        public int? DistrictId { get; set; }

        public int? WardId { get; set; }

        public string? Description { get; set; }

        public string? Longitude { get; set; }

        public string? Latitude { get; set; }

        public string? Representative { get; set; }

        public string? TaxNumber { get; set; }

        public virtual List<PartnerReferenceBuyCreateDto?>? ReferenceBuys { get; set; } 

        public virtual List<PartnerReferenceSellCreateDto?>? ReferenceSells { get; set; }

        public virtual List<PartnerVehicleCreatePartnerDto?>? VehicleReferences { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblMdPartner, PartnerCreateDto>().ReverseMap();
        }
    }

    public class PartnerUpdateDto : BaseMdDto, IMapFrom, IDto
    {
        [Key]
        public int Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string? SaleTypeCode { get; set; }

        public bool IsCustomer { get; set; }

        public bool IsSupplier { get; set; }

        public string? Address { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Email { get; set; }

        public int? ProvineId { get; set; }

        public int? DistrictId { get; set; }

        public int? WardId { get; set; }

        public string? Description { get; set; }

        public string? Longitude { get; set; }

        public string? Latitude { get; set; }

        public string? Representative { get; set; }

        public string? TaxNumber { get; set; }

        public virtual List<PartnerReferenceBuyCreateDto?>? ReferenceBuys { get; set; }

        public virtual List<PartnerReferenceSellCreateDto?>? ReferenceSells { get; set; }

        public virtual List<PartnerVehicleCreatePartnerDto>? VehicleReferences { get; set; }

        public virtual List<PartnerAreaWithAreaLiteDto> AreaReferences { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<PartnerUpdateDto, TblMdPartner>()
                .ReverseMap();
        }
    }

    public class PartnerLiteDto : BaseMdDto, IMapFrom, IDto
    {
        [Key]
        public int Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string SaleTypeCode { get; set; }

        public bool IsCustomer { get; set; }

        public bool IsSupplier { get; set; }

        public string? Address { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Email { get; set; }

        public int? ProvineId { get; set; }

        public int? DistrictId { get; set; }

        public int? WardId { get; set; }

        public string? Description { get; set; }

        public string? Longitude { get; set; }

        public string? Latitude { get; set; }

        public string? Representative { get; set; }

        public string? TaxNumber { get; set; }

        public Guid? ReferenceId { get; set; }

        public virtual ProvineDto Provine { get; set; }

        public virtual DistrictDto District { get; set; }

        public virtual WardDto Ward { get; set; }

        public virtual SaleTypeDto SaleType { get; set; }

        public virtual List<PartnerAreaWithAreaDto> AreaReferences { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblMdPartner, PartnerLiteDto>().ReverseMap();
        }
    }

    public class PartnerUpdateAreaDto : BaseMdDto, IMapFrom, IDto
    {
        [Key]
        public int Id { get; set; }

        public virtual List<PartnerAreaWithAreaLiteDto> AreaReferences { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<PartnerUpdateAreaDto, TblMdPartner>()
                .ReverseMap();
        }
    }
}
