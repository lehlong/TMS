using System.ComponentModel.DataAnnotations;
using AutoMapper;
using System.ComponentModel;
using Common;

namespace DMS.CORE.Entities.MD
{
    public class TypeItemDto : BaseMdDto, IMapFrom, IDto
    { 
     
        [Description("STT")]
        public int OrdinalNumber { get; set; }

        [Key]
        [Description("Mã loại hàng hóa")]
        public string Code { get; set; }

        [Description("Tên loại hàng hóa")]
        public string Name { get; set; }

        [Description("Thứ tự hiển thị")]
        public int OrderNumber { get; set; }

        [Description("Trạng thái")]
        public string State { get => this.IsActive == true ? "Đang hoạt động" : "Khóa"; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblMdTypeItem, TypeItemDto>().ReverseMap();
        }
    }
}
