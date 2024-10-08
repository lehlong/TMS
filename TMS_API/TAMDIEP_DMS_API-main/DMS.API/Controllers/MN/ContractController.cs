using Common;
using DMS.API.AppCode.Enum;
using DMS.API.AppCode.Extensions;
using DMS.BUSINESS.Dtos.MN;
using DMS.BUSINESS.Filter.MN;
using DMS.BUSINESS.Services.MN;
using Microsoft.AspNetCore.Mvc;

namespace DMS.API.Controllers.MN
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContractController(IContractService service) : ControllerBase
    {
        public readonly IContractService _service = service;

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery]ContractGetAllFilter filter)
        {
            var transferObject = new TransferObject();
            var result = await _service.GetAll(filter);
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


        [HttpPost("Sync")]
        public IActionResult Sync()
        {
            var transferObject = new TransferObject();
            if (_service.Status)
            {
                transferObject.Status = true;
                transferObject.MessageObject.MessageType = MessageType.Success;
                transferObject.GetMessage("0100", _service);
            }
            else
            {
                transferObject.Status = false;
                transferObject.MessageObject.MessageType = MessageType.Error;
                transferObject.GetMessage("0101", _service);
            }
            return Ok(transferObject);
        }
    }
}
