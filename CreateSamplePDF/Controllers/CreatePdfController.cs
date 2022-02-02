using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CreateSamplePDF.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreatePdfController : ControllerBase
    {
        private readonly IPDFGeneratorCore _pDFGeneratorCore;

        public CreatePdfController(IPDFGeneratorCore pDFGeneratorCore)
        {
            _pDFGeneratorCore = pDFGeneratorCore;
        }
        [HttpGet(Name = "GetCreatedPdf")]
        public IActionResult Get()
        {
            var response = _pDFGeneratorCore.CreateSamplePDf();
            return Ok(response);
        }
    }
}
