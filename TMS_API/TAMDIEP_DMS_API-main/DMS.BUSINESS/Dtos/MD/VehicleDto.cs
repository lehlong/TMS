using AutoMapper;
using Common;
using DMS.BUSINESS.Dtos.MN;
using System.ComponentModel.DataAnnotations;

namespace DMS.CORE.Entities.MD
{
    public class VehicleDto : BaseMdDto, IMapFrom, IDto
    {
        [Key]
        public string Code { get; set; }

        public string VehicleTypeCode { get; set; }

        public double? Tonnage { get; set; }

        public double? TareTonnage { get; set; }

        public double? Height { get; set; }

        public double? Width { get; set; }

        public double? Length { get; set; }

        public int? PartnerIdCreate { get; set; }

        public Guid? ReferenceId { get; set; } 

        public virtual VehicleTypeDto Type { get; set; }

        public virtual PartnerLiteDto PartnerCreate { get; set; }

        public List<PartnerVehicleWithoutVehicleDto> PartnerReferences { get; set; }

        public List<DriverVehicleWithDriverDto> DriverReferences { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblMdVehicle, VehicleDto>().ReverseMap();
        }
    }

    public class VehicleCreateUpdateDto : BaseMdDto, IMapFrom, IDto
    {
        [Key]
        public string Code { get; set; }

        public string VehicleTypeCode { get; set; }

        public double? Tonnage { get; set; }

        public double? TareTonnage { get; set; }

        public double? Height { get; set; }

        public double? Width { get; set; }

        public double? Length { get; set; }

        public int? PartnerIdCreate { get; set; }

        public List<PartnerVehicleCreateVehicleDto>? PartnerReferences { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblMdVehicle, VehicleCreateUpdateDto>().ForMember(x=>x.PartnerReferences,y=>y.Ignore()).ReverseMap();
        }
    }

    public class VehicleLiteDto : BaseMdDto, IMapFrom, IDto
    {
        [Key]
        public string Code { get; set; }

        public string VehicleTypeCode { get; set; }

        public double? Tonnage { get; set; }

        public double? TareTonnage { get; set; }

        public double? Height { get; set; }

        public double? Width { get; set; }

        public double? Length { get; set; }

        public int? PartnerIdCreate { get; set; }

        public Guid? ReferenceId { get; set; }

        public virtual VehicleTypeDto Type { get; set; }

        public virtual PartnerLiteDto PartnerCreate { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblMdVehicle, VehicleLiteDto>().ReverseMap();
        }
    }
}
