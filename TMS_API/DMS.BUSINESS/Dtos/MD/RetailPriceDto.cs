using AutoMapper;
using Common;
using DMS.CORE.Entities.MD;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.BUSINESS.Dtos.MD
{
    public class RetailPriceDto : BaseMdDto, IDto, IMapFrom
    {
        [Description("STT")]
        public int OrdinalNumber { get; set; }

        [Key]
        [Description("Mã")]
        public string Code { get; set; }

        [Description("Tên")]
        public string GoodsCode { get; set; }

        [Description("Mã Vùng")]
        public string LocalCode { get; set; }

        [Description("Ngày bắt đầu")]
        public DateTime? FromDate { get; set; }

        [Description("Đến ngày")]
        public DateTime ToDate { get; set; }

        [Description("Giá cũ")]
        public float OldPrice { get; set; }

        [Description("Giá mới")]
        public float NewPrice { get; set; }

        [Description("Ngày tạo")]
        public DateTime? CreateDate { get; set; }

        [Description("Trạng thái")]
        public string State { get => this.IsActive == true ? "Đang hoạt động" : "Khóa"; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblMdRetailPrice, RetailPriceDto>().ReverseMap();
        }
    }
}
