using Microsoft.AspNetCore.Mvc;

namespace SistemaERP.Controllers
{
    public class CaixaController : Controller
    {
        private readonly ILogger<CaixaController> _logger;
        public CaixaController(ILogger<CaixaController> logger)
        {
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
