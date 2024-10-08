using AutoMapper;
using Common;
using DMS.CORE.Entities.MD;
using DMS.CORE.Entities.MN;
using System.ComponentModel.DataAnnotations;

namespace DMS.BUSINESS.Dtos.MN
{
    public class DriverVehicleDto : BaseDto, IMapFrom, IDto
    {
        [Key]
        public int Id { get; set; }

        public string VehicleCode { get; set; }

        public int DriverId { get; set; }

        public virtual VehicleDto Vehicle { get; set; }

        public virtual DriverDto Driver { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblMnDriverVehicle, DriverVehicleDto>().ReverseMap();
        }
    }

    public class DriverVehicleWithVehicleDto : IMapFrom, IDto
    {
        public string VehicleCode { get; set; }

        public virtual VehicleLiteDto Vehicle { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblMnDriverVehicle, DriverVehicleWithVehicleDto>().ReverseMap();
        }
    }

    public class DriverVehicleWithDriverDto : IMapFrom, IDto
    {
        public int DriverId { get; set; }

        public virtual DriverLiteDto Driver { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblMnDriverVehicle, DriverVehicleWithDriverDto>().ReverseMap();
        }
    }

    public class DriverVehicleWithVehicleLiteDto : IMapFrom, IDto
    {
        public string VehicleCode { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblMnDriverVehicle, DriverVehicleWithVehicleLiteDto>().ReverseMap();
        }
    }
}
