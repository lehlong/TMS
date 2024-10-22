using Common;
using DMS.API.AppCode.Enum;
using DMS.API.AppCode.Extensions;
using DMS.BUSINESS.Dtos.BU;
using DMS.BUSINESS.Models;
using DMS.BUSINESS.Services.BU;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DMS.API.Controllers.BU
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalculateResultController : ControllerBase
    {
        public readonly ICalculateResultService _service;
        public CalculateResultController(ICalculateResultService service)
        {
            _service = service;
        }
        [HttpGet("GetCalculateResult")]
        public async Task<IActionResult> GetCalculateResult(QueryModel? model)
        {
            var transferObject = new TransferObject();
            var result = await _service.GetResult(model);
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
