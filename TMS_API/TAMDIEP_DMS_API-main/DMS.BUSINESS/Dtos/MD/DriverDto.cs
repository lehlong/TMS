using System.ComponentModel.DataAnnotations;
using AutoMapper;
using DMS.BUSINESS.Dtos.AD;
using System.Text.Json.Serialization;
using System.ComponentModel;
using DMS.BUSINESS.Dtos.MN;
using Common;

namespace DMS.CORE.Entities.MD
{
    public class DriverDto : BaseMdDto, IMapFrom, IDto
    {
        [JsonIgnore]
        [Description("STT")]
        public int OrdinalNumber { get; set; }

        [Key]
        public int Id { get; set; }

        [Description("Tên đầy đủ")]
        public string FullName { get; set; }

        [Description("Số CCCD")]
        public string CccdNumber { get; set; }

        [Description("Ghi chú")]
        public string Notes { get; set; }

        [Description("Tài khoản đăng nhập")]
        public string UserName { get; set; }

        public Guid? ReferenceId { get; set; } = Guid.NewGuid();

        public int? PartnerIdCreate { get; set; }

        public string? PhoneNumber { get; set; }

        public virtual AccountDto Account { get; set; }

        public virtual List<DriverVehicleWithVehicleDto> VehicleReferences { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblMdDriver, DriverDto>().ReverseMap();
        }
    }

    public class DriverCreateDto : BaseMdDto, IMapFrom, IDto
    {
        public string FullName { get; set; }

        public string? CccdNumber { get; set; }

        public string? Notes { get; set; }

        public string? UserName { get; set; }

        public int? PartnerIdCreate { get; set; }

        public string? PhoneNumber { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblMdDriver, DriverCreateDto>().ReverseMap();
        }
    }

    public class DriverUpdateDto : BaseMdDto, IMapFrom, IDto
    {
        [Key]
        public int Id { get; set; }

        public string FullName { get; set; }

        public string? CccdNumber { get; set; }

        public string? Notes { get; set; }

        public string? UserName { get; set; }

        public int? PartnerIdCreate { get; set; }

        public string? PhoneNumber { get; set; }

        public virtual List<DriverVehicleWithVehicleLiteDto> VehicleReferences { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblMdDriver, DriverUpdateDto>().ReverseMap();
        }
    }

    public class DriverLiteDto : IMapFrom, IDto
    {
        public string FullName { get; set; }

        public string UserName { get; set; }

        public Guid? ReferenceId { get; set; } = Guid.NewGuid();

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblMdDriver, DriverLiteDto>().ReverseMap();
        }
    }
}
