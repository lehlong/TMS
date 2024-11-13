using AutoMapper;
using DMS.BUSINESS.Common;
using DMS.BUSINESS.Dtos.MD;
using DMS.BUSINESS.Models;
using DMS.CORE;
using DMS.CORE.Entities.BU;
using DMS.CORE.Entities.MD;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Text = DocumentFormat.OpenXml.Wordprocessing.Text;
using PROJECT.Service.Extention;
using Aspose.Words;
using Table = DocumentFormat.OpenXml.Wordprocessing.Table;
using DocumentFormat.OpenXml;
using Paragraph = DocumentFormat.OpenXml.Wordprocessing.Paragraph;
using Run = DocumentFormat.OpenXml.Wordprocessing.Run;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace DMS.BUSINESS.Services.BU
{
    public interface IDiscountInformationService : IGenericService<TblMdGoods, GoodsDto>
    {
        Task<string> SaveFileHistory(MemoryStream outFileStream, string headerId);
        void ExportExcel(ref MemoryStream outFileStream, string path, string headerId);
        Task<DiscountInformationModel> getAll(string Code);
    }
    public class DiscountInformationService(AppDbContext dbContext, IMapper mapper) : GenericService<TblMdGoods, GoodsDto> (dbContext, mapper), IDiscountInformationService
    {
        public async Task<DiscountInformationModel> getAll(string Code)
        {
            try
            {
                var data = new DiscountInformationModel();
                var lstMarket = await _dbContext.TblMdMarket.OrderBy(x => x.Code).ToListAsync();
                var lstDiscountCompetitor = await _dbContext.TblInDiscountCompetitor.Where(x => x.HeaderCode == Code).ToListAsync();
                var lstMarketCompetitor = await _dbContext.TblMdMarketCompetitor.OrderBy(x => x.Code).ToListAsync();

                var lstDIL = await _dbContext.TblBuDiscountInformationList.Where(x => x.Code == Code).ToListAsync();
                data.lstDIL = lstDIL;
                var lstGoods = await _dbContext.TblMdGoods.OrderBy(x => x.CreateDate).ToListAsync();
                data.lstGoods = lstGoods;
                var lstCompetitor = await _dbContext.TblMdCompetitor.OrderBy(x => x.Code).ToListAsync();
                data.lstCompetitor = lstCompetitor;

                var orderMarket = 1;
                var plxna = 1300;
                var z11 = 1961;

                var row1 = new discout
                {
                    colA = "I",
                    colB = "KHO TRUNG TÂM (FOB)",
                    IsBold = true
                };
                foreach (var c in lstCompetitor)
                {
                    row1.gaps.Add(null);
                    row1.cuocVCs.Add(null);
                }

                foreach (var g in lstGoods)
                {
                    var ck = new CK
                    {
                        plxna = plxna,
                    };

                    foreach (var c in lstCompetitor)
                    {
                        var dt = new DT();

                        var ck1 = lstDiscountCompetitor.Where(x => x.CompetitorCode == c.Code && x.GoodsCode == g.Code).Sum(x => x.Discount ?? 0 );
                        dt.ckCl.Add(ck1);
                        dt.ckCl.Add(ck1 - plxna);

                        ck.DT.Add(dt);
                    }
                    row1.CK.Add(ck);
                }
                
                data.discount.Add(row1);

                data.discount.Add(new discout
                {
                    colA = "II",
                    colB = "KHO KHÁCH HÀNG (CIF)",
                    IsBold = true
                });

                foreach (var m in lstMarket)
                {
                    var d = new discout
                    {
                        colA = orderMarket.ToString(),
                        colB = m.Name,
                        col1 = m.Gap ?? 0,
                        col4 = m.CuocVCBQ ?? 0,
                    };
                    

                    foreach (var c in lstCompetitor)
                    {
                        var gap = lstMarketCompetitor.Where(x => x.CompetitorCode == c.Code && x.MarketCode == m.Code).Sum(x => x.Gap == 0 ? m.Gap + 120 : x.Gap);
                        var cuocVc = lstMarketCompetitor.Where(x => x.CompetitorCode == c.Code && x.MarketCode == m.Code).Sum(x => x.Gap == 0 ? m.CuocVCBQ + 200 : x.Gap * (decimal)z11 / 1000 ?? 0);
                        d.gaps.Add(Math.Round(gap ?? 0)); 
                        d.cuocVCs.Add(cuocVc != null ? Math.Round((decimal)cuocVc, 0) : 0);
                    }
                    foreach (var g in lstGoods)
                    {
                        var ck = new CK
                        {
                            plxna = plxna - m.CuocVCBQ,
                        };
                        foreach (var c in lstCompetitor)
                        {
                            var dt = new DT();
                            
                            var gap = lstMarketCompetitor.Where(x => x.CompetitorCode == c.Code && x.MarketCode == m.Code).Sum(x => x.Gap == 0 ? m.Gap + 120 : x.Gap);
                            var cuocVc = gap * z11 / 1000;
                            var ck1 = lstDiscountCompetitor.Where(x => x.CompetitorCode == c.Code && x.GoodsCode == g.Code).Sum(x => x.Discount - cuocVc) ;
                            dt.ckCl.Add(Math.Round((decimal)ck1, 0));
                            dt.ckCl.Add(Math.Round((decimal)(ck1 - (plxna - m.CuocVCBQ)), 0));

                            ck.DT.Add(dt);
                        }
                        d.CK.Add(ck);
                    }
                    data.discount.Add(d);
                    orderMarket++;
                }
                return data;
            }
            catch(Exception ex)
            {
                this.Status = false;
                this.Exception = ex;
                return new DiscountInformationModel();
            }
        }   

        public void ExportExcel(ref MemoryStream outFileStream, string path, string headerId)
        {
            try
            {
                FileStream fs = new FileStream(path, FileMode.Open, FileAccess.ReadWrite);
                IWorkbook templateWorkbook;
                templateWorkbook = new XSSFWorkbook(fs);
                fs.Close();

                //Define Style
                var styleCellNumber = GetCellStyleNumber(templateWorkbook);

                var font = templateWorkbook.CreateFont();
                font.FontHeightInPoints = 12;
                font.FontName = "Times New Roman";

                ICellStyle styleCellBold = templateWorkbook.CreateCellStyle(); // chữ in đậm
                var fontBold = templateWorkbook.CreateFont();
                fontBold.Boldweight = (short)FontBoldWeight.Bold;
                fontBold.FontHeightInPoints = 12;
                fontBold.FontName = "Times New Roman";

                //Get Data
                var data = getAll(headerId);

                var startRowPTCK = 0;
                ISheet sheetPTCK = templateWorkbook.GetSheetAt(1);
                styleCellBold.CloneStyleFrom(sheetPTCK.GetRow(1).Cells[0].CellStyle);

                for(var i = 0; i < data.Result.discount.Count(); i++)
                {
                    //var data
                }

                    templateWorkbook.Write(outFileStream);
            }
            catch (Exception ex)
            {

            }
        }
        public ICellStyle GetCellStyleNumber(IWorkbook templateWorkbook)
        {
            ICellStyle styleCellNumber = templateWorkbook.CreateCellStyle();
            styleCellNumber.DataFormat = templateWorkbook.CreateDataFormat().GetFormat("#,##0");
            return styleCellNumber;
        }

        public async Task<string> SaveFileHistory(MemoryStream outFileStream, string headerId)
        {
            byte[] data = outFileStream.ToArray();
            var path = "";
            using (MemoryStream memoryStream = new MemoryStream(data))
            {
                IFormFile file = ConvertMemoryStreamToIFormFile(memoryStream, "example.txt");
                var folderName = Path.Combine($"Upload/{DateTime.Now.Year}/{DateTime.Now.Month}");
                if (!Directory.Exists(folderName))
                {
                    Directory.CreateDirectory(folderName);
                }
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                if (file.Length > 0)
                {
                    var fileName = $"{DateTime.Now.Day}{DateTime.Now.Month}{DateTime.Now.Year}_{DateTime.Now.Hour}{DateTime.Now.Minute}{DateTime.Now.Second}PhanTichChietKhau.xlsx";
                    var fullPath = Path.Combine(pathToSave, fileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    path = $"Upload/{DateTime.Now.Year}/{DateTime.Now.Month}/{fileName}";
                    _dbContext.TblBuHistoryDownload.Add(new TblBuHistoryDownload
                    {
                        Code = Guid.NewGuid().ToString(),
                        HeaderCode = headerId,
                        Name = fileName,
                        Type = "xlsx",
                        Path = path
                    });
                    await _dbContext.SaveChangesAsync();
                }
            }
            return path;
        }
        public static IFormFile ConvertMemoryStreamToIFormFile(MemoryStream memoryStream, string fileName)
        {
            memoryStream.Position = 0; // Reset the stream position to the beginning
            IFormFile formFile = new FormFile(memoryStream, 0, memoryStream.Length, "file", fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = "application/octet-stream"
            };
            return formFile;

        }
    }

    
}
