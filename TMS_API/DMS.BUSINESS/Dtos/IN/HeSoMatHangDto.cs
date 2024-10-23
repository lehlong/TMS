using System.ComponentModel.DataAnnotations;
using AutoMapper;
using System.Text.Json.Serialization;
using System.ComponentModel;

using Common;
using DMS.CORE.Entities.IN;

namespace DMS.BUSINESS.Dtos.IN
{
    public class HeSoMatHangDto : BaseMdDto, IMapFrom, IDto
    {
        [JsonIgnore]
        [Description("Số thứ tự")]
        public int OrdinalNumber { get; set; }

        [Key]
        [Description("Mã")]
        public string Code { get; set; }

        [Description("Mã mặt hàng")]
        public string GoodsCode { get; set; }

        [Description("Hệ số VCF BQ Mùa miền")]
        public decimal HeSoVcf { get; set; }

        [Description("Thuế BVMT")]
        public decimal ThueBvmt { get; set; }

        [Description("L15 chưa VAT và BVMT ")]
        public decimal L15ChuaVatBvmt { get; set; }

        [Description("Từ ngày")]
        public DateTime? FromDate { get; set; }

        [Description("Đến ngày")]
        public DateTime? ToDate { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblInHeSoMatHang, HeSoMatHangDto>().ReverseMap();
        }
    }

}
