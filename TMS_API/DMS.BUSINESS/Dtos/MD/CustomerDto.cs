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
    public class CustomerDto : BaseMdDto, IDto, IMapFrom
    {
        [Description("STT")]
        public int OrdinalNumber { get; set; }

        [Key]
        [Description("Mã")]
        public string Code { get; set; }

        [Description("Tên")]
        public string Name { get; set; }

        [Description("Số điện thoại")]
        public string? Phone { get; set; }

        [Description("email")]
        public string? Email { get; set; }

        [Description("địa chỉ")]
        public string? Address { get; set; }

        [Description("thông tin mua hàng")]
        public string? BuyInfo { get; set; }

        [Description("Lãi vay ngân hàng")]
        public decimal? BankLoanInterest { get; set; }

        [Description("Mã phương thức mua hàng")]
        public string? SalesMethodCode { get; set; }

        [Description("Mã Vùng")]
        public string? LocalCode { get; set; }

        [Description("Mã thị trường")]
        public string? MarketCode { get; set; }


        [Description("Trạng thái")]
        public string State { get => this.IsActive == true ? "Đang hoạt động" : "Khóa"; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblMdCustomer, CustomerDto>().ReverseMap();
        }
    }
}
