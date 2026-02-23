using CrystalBridgeService.Services;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace CrystalBridgeService.Controllers
{
    [RoutePrefix("api/reports")]
    public class ReportsController : ApiController
    {
        private readonly ICrystalReportService _reportService;

        public ReportsController()
        {
            _reportService = new CrystalReportService();
        }

        [HttpGet]
        [Route("health")]
        public IHttpActionResult Health()
        {
            return Ok(new { status = "ok" });
        }

        [HttpGet]
        [Route("GenerateLayout")]
        public HttpResponseMessage GenerateLayout(
            int idLayout,
            int idObject,
            string fileName = "Report",
            string printedByUserId = "")
        {
            try
            {
                var result = _reportService.GenerateLayoutPdf(idLayout, idObject, fileName, printedByUserId);
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new ByteArrayContent(result.Content);
                response.Content.Headers.ContentType = new MediaTypeHeaderValue(result.ContentType);
                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = result.FileName
                };
                return response;
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("GenerateLayoutDebug")]
        public IHttpActionResult GenerateLayoutDebug(
            int idLayout,
            int idObject,
            string fileName = "Report",
            string printedByUserId = "")
        {
            try
            {
                var result = _reportService.GenerateLayoutDebug(idLayout, idObject, fileName, printedByUserId);
                return Ok(result);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
