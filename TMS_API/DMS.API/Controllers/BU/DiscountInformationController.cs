using Common;
using DMS.API.AppCode.Enum;
using DMS.BUSINESS.Services.BU;
using Microsoft.AspNetCore.Mvc;

namespace DMS.API.Controllers.BU
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountInformationController : ControllerBase
    {
        public readonly IDiscountInformationService _service;

        public DiscountInformationController(IDiscountInformationService service)
        {
            _service = service;
        }   

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] string Code)
        {
            var transferObject = new TransferObject();
            var result = await _service.getAll(Code);
            if (_service.Status)
            {
                transferObject.Data = result;
            }
            else
            {
                transferObject.Status = false;
                transferObject.MessageObject.MessageType = MessageType.Error;
                //transferObject.GetMessage("2000", _service);
            }
            return Ok(transferObject);
        }


    }
}
