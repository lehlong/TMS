using AutoMapper;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Text.Json.Serialization;
using Common;

namespace DMS.CORE.Entities.MD
{
    public class VehicleTypeDto : BaseMdDto, IMapFrom, IDto
    {
        [JsonIgnore]
        [Description("STT")]
        public int OrdinalNumber { get; set; }

        [Key]
        [Description("Mã đơn loại phương tiện")]
        public string Code { get; set; }

        [Description("Tên loại phương tiện")]
        public string Name { get; set; }

        [Description("Trạng thái")]
        public string State { get => this.IsActive == true ? "Đang hoạt động" : "Khóa"; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblMdVehicleType, VehicleTypeDto>().ReverseMap();
        }
    }
}
