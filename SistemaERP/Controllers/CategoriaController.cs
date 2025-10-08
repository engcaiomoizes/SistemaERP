using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SistemaERP.Models;
using System.Data;
using System.Text.Json.Serialization;

namespace SistemaERP.Controllers
{
    public class CategoriaController : Controller
    {
        Uri baseUrl = new Uri("http://10.10.254.20:5000/api/v1.0/");
        private readonly HttpClient _httpClient;

        private readonly ILogger<CategoriaController> _logger;
        public CategoriaController(ILogger<CategoriaController> logger)
        {
            _logger = logger;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = baseUrl;
        }
        [HttpGet]
        public IActionResult Index()
        {
            IEnumerable<CategoriaViewModel> categorias = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = baseUrl;
                var responseTask = client.GetAsync("categoria");
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    readTask.Wait();
                    categorias = JsonConvert.DeserializeObject<IEnumerable<CategoriaViewModel>>(readTask.Result);
                }
                else
                {
                    categorias = Enumerable.Empty<CategoriaViewModel>();
                    ModelState.AddModelError(string.Empty, "Ocorreu um erro no servidor. Por favor, tente novamente mais tarde.");
                }
            }

            return View(categorias);
        }

        [HttpPost]
        public async Task<IActionResult> Form(CategoriaViewModel model)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = baseUrl;
                    var json = JsonConvert.SerializeObject(model);
                    var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                    var response = await client.PostAsync("categoria", content);
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Ocorreu um erro ao criar a categoria. Por favor, tente novamente.");
                    }
                }
            }
            
            return View(model);
        }

        [HttpGet]
        public IActionResult Form()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = baseUrl;
                    var response = await client.DeleteAsync($"categoria/id:int?id={id}");

                    if (response.IsSuccessStatusCode)
                    {
                        return Json(new { success = true });
                        //return RedirectToAction("Index");
                    } else
                    {
                        ModelState.AddModelError(string.Empty, "Ocorreu um erro ao tentar excluir a Categoria.");
                        return Json(new { success = false, message = "Ocorreu um erro ao tentar excluir a Categoria." });
                    }
                }
            }

            return Json(new { success = false, message = "Modelo inválido!" });
        }
    }
}
