using AutoMapper;
using Common;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DMS.CORE.Entities.MD
{
    public class SaleTypeDto : BaseMdDto, IMapFrom, IDto
    {
        [Key]
        public string Code { get; set; }

        public string Name { get; set; }

        [JsonIgnore]
        public virtual List<TblMdPartner>? Partners { get; set; }
       
        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblMdSaleType, SaleTypeDto>().ReverseMap();
        }
    }
}
