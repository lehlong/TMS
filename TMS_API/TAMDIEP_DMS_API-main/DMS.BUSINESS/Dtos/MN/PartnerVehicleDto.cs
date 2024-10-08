using AutoMapper;
using Common;
using DMS.CORE.Entities.MD;
using DMS.CORE.Entities.MN;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DMS.BUSINESS.Dtos.MN
{
    public class PartnerVehicleDto : IMapFrom, IDto
    {
        [Key]
        public int Id { get; set; }

        public string VehicleCode { get; set; }

        public int PartnerId { get; set; }

        public virtual VehicleDto Vehicle { get; set; }

        [JsonIgnore]
        public virtual PartnerDto Partner { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblMnPartnerVehicle, PartnerVehicleDto>().ReverseMap();
        }
    }

    public class PartnerVehicleCreatePartnerDto : IMapFrom, IDto
    {
        public string VehicleCode { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblMnPartnerVehicle, PartnerVehicleCreatePartnerDto>().ReverseMap();
        }
    }

    public class PartnerVehicleCreateVehicleDto : IMapFrom, IDto
    {
        public int PartnerId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblMnPartnerVehicle, PartnerVehicleCreateVehicleDto>().ReverseMap();
        }
    }

    public class PartnerVehicleWithoutVehicleDto : IMapFrom, IDto
    {
        [Key]
        public int Id { get; set; }

        public int PartnerId { get; set; }

        public virtual PartnerLiteDto Partner { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblMnPartnerVehicle, PartnerVehicleWithoutVehicleDto>().ReverseMap();
        }
    }
}
