using Common;
using DMS.API.AppCode.Enum;
using DMS.API.AppCode.Extensions;
using DMS.BUSINESS.Dtos.MN;
using DMS.BUSINESS.Services.MN;
using Microsoft.AspNetCore.Mvc;

namespace DMS.API.Controllers.MN
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountSaleOfficeController(IAccountSaleOfficeService service) : ControllerBase
    {
        public readonly IAccountSaleOfficeService _service = service;

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] AccountSaleOfficeUpdateDto param)
        {
            var transferObject = new TransferObject();
            await _service.Update(param);
            if (_service.Status)
            {
                transferObject.Status = true;
                transferObject.MessageObject.MessageType = MessageType.Success;
                transferObject.GetMessage("0103", _service);
            }
            else
            {
                transferObject.Status = false;
                transferObject.MessageObject.MessageType = MessageType.Error;
                transferObject.GetMessage("0104", _service);
            }
            return Ok(transferObject);
        }

        [HttpPut("Delete")]
        public async Task<IActionResult> Delete([FromBody] AccountSaleOfficeUpdateDto param)
        {
            var transferObject = new TransferObject();
            await _service.Delete(param);
            if (_service.Status)
            {
                transferObject.Status = true;
                transferObject.MessageObject.MessageType = MessageType.Success;
                transferObject.GetMessage("0103", _service);
            }
            else
            {
                transferObject.Status = false;
                transferObject.MessageObject.MessageType = MessageType.Error;
                transferObject.GetMessage("0104", _service);
            }
            return Ok(transferObject);
        }

        [HttpGet("GetAllDistinct")]
        public async Task<IActionResult> GetAllDistinct()
        {
            var transferObject = new TransferObject();
            var result = await _service.GetAllDistinct();
            if (_service.Status)
            {
                transferObject.Data = result;
            }
            else
            {
                transferObject.Status = false;
                transferObject.MessageObject.MessageType = MessageType.Error;
                transferObject.GetMessage("0001", _service);
            }
            return Ok(transferObject);
        }
    }
}
