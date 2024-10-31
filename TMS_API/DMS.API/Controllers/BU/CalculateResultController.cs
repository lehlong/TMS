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
        public async Task<IActionResult> GetCalculateResult([FromQuery] string code)
        {
            var transferObject = new TransferObject();
            var result = await _service.GetResult(code);
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

        [HttpGet("GetDataInput")]
        public async Task<IActionResult> GetDataInput([FromQuery] string code)
        {
            var transferObject = new TransferObject();
            var result = await _service.GetDataInput(code);
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
        [HttpGet("GetHistoryAction")]
        public async Task<IActionResult> GetHistoryAction([FromQuery] string code)
        {
            var transferObject = new TransferObject();
            var result = await _service.GetHistoryAction(code);
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

        [HttpPost("UpdateDataInput")]
        public async Task<IActionResult> UpdateDataInput([FromBody] InsertModel model)
        {
            var transferObject = new TransferObject();

            await _service.UpdateDataInput(model);
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

        [HttpGet("ExportExcel")]
        public async Task<IActionResult> ExportExcel([FromQuery] string headerId)
        {
            var transferObject = new TransferObject();
            MemoryStream outFileStream = new MemoryStream();
            var path = Path.GetFullPath("~/Template/CoSoTinhMucGiamGia.xlsx").Replace("~\\", "");
            _service.ExportExcel(ref outFileStream, path, headerId);
            if (_service.Status)
            {
                return File(outFileStream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", DateTime.Now.ToString() + "_CoSoTinhMucGiamGia" + ".xlsx");
               // return Ok(transferObject);
            }
            else
            {
                transferObject.Status = false;
                transferObject.MessageObject.MessageType = MessageType.Error;
                transferObject.GetMessage("2000", _service);
                return Ok(transferObject);
            }
        }
    }
}
