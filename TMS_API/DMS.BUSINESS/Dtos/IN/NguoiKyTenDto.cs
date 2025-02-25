using System.ComponentModel.DataAnnotations;
using AutoMapper;
using System.Text.Json.Serialization;
using System.ComponentModel;

using Common;
using DMS.CORE.Entities.IN;

namespace DMS.BUSINESS.Dtos.IN
{
    public class NguoiKyTenDto : BaseMdDto, IMapFrom, IDto
    {
        [JsonIgnore]
        [Description("Số thứ tự")]
        public int OrdinalNumber { get; set; }

        [Key]
        [Description("Mã")]
        public string Code { get; set; }

        [Description("Mã Header")]
        public string HeaderCode { get; set; }

        [Description("Phòng ban")]
        public string Department { get; set; }

        [Description("Chức vụ")]
        public string Position { get; set; }

        [Description("Người ký tên")]
        public string Signatory { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblInNguoiKyTen, NguoiKyTenDto>().ReverseMap();
        }
    }

}
