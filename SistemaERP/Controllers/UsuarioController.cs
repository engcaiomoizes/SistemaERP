using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SistemaERP.Models;

namespace SistemaERP.Controllers
{
    public class UsuarioController : Controller
    {
        Uri baseUrl = new Uri("http://10.10.254.20:5000/api/v1.0/");
        private readonly HttpClient _httpClient;

        private readonly ILogger<UsuarioController> _logger;

        public UsuarioController(ILogger<UsuarioController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            IEnumerable<UsuarioViewModel> usuarios = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = baseUrl;
                var responseTask = client.GetAsync("usuario");
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    readTask.Wait();
                    usuarios = JsonConvert.DeserializeObject<IEnumerable<UsuarioViewModel>>(readTask.Result);
                } else
                {
                    usuarios = Enumerable.Empty<UsuarioViewModel>();
                    ModelState.AddModelError(string.Empty, "Ocorreu um erro no servidor. Por favor, tente novamente mais tarde.");
                }
            }

            return View(usuarios);
        }

        public IActionResult Form()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(UsuarioRequestModel model)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = baseUrl;
                    var json = JsonConvert.SerializeObject(model);
                    var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                    var response = await client.PostAsync("usuario", content);
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    } else
                    {
                        ModelState.AddModelError(string.Empty, "Ocorreu um erro ao cadastrar o Usuário. Por favor, tente novamente.");
                    }
                }
            }

            return View("Form", model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = baseUrl;
                    var response = await client.DeleteAsync($"usuario/id:int?id={id}");

                    if (response.IsSuccessStatusCode)
                    {
                        return Json(new { success = true });
                    } else
                    {
                        ModelState.AddModelError(string.Empty, "Ocorreu um erro ao tentar excluir o Usuário.");
                        return Json(new { success = false, message = "Ocorreu um erro ao tentar excluir o Usuário." });
                    }
                }
            }

            return Json(new { success = false, message = "Modelo inválido!" });
        }
    }
}
