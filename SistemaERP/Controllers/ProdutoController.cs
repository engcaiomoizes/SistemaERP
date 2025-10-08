using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SistemaERP.Models;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SistemaERP.Controllers
{
    public class ProdutoController : Controller
    {
        Uri baseUrl = new Uri("http://10.10.254.20:5000/api/v1.0/");
        private readonly HttpClient _httpClient;

        private readonly ILogger<ProdutoController> _logger;

        public ProdutoController(ILogger<ProdutoController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            IEnumerable<ProdutoViewModel> produtos = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = baseUrl;
                var responseTask = client.GetAsync("produto");
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    readTask.Wait();
                    produtos = JsonConvert.DeserializeObject<IEnumerable<ProdutoViewModel>>(readTask.Result);
                } else
                {
                    produtos = Enumerable.Empty<ProdutoViewModel>();
                    ModelState.AddModelError(string.Empty, "Ocorreu um erro no servidor. Por favor, tente novamente mais tarde.");
                }
            }

            return View(produtos);
        }

        public IEnumerable<CategoriaViewModel> GetCategorias()
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
                } else
                {
                    categorias = Enumerable.Empty<CategoriaViewModel>();
                    ModelState.AddModelError(string.Empty, "Ocorreu um erro no servidor. Por favor, tente novamente mais tarde.");
                }
            }

            return categorias;
        }

        [HttpGet]
        public IActionResult Form()
        {
            Console.WriteLine("Entrou aqui");
            var categorias = GetCategorias();

            var viewModel = new ProdutoCategoriaRequestModel
            {
                Produto = new ProdutoRequestModel(),
                Categorias = categorias
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProdutoCategoriaRequestModel model)
        {
            if (ModelState.IsValid)
            {
                var produto = model.Produto;

                if (!string.IsNullOrEmpty(produto.Preco))
                {
                    produto.Preco = produto.Preco.Replace(".", "").Replace(",", ".");
                }

                using (var client = new HttpClient())
                {
                    client.BaseAddress = baseUrl;
                    var json = JsonConvert.SerializeObject(produto);
                    var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                    var response = await client.PostAsync("produto", content);
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Ocorreu um erro ao cadastrar o Produto. Por favor, tente novamente.");
                    }
                }
            } else
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"Erro: {error.ErrorMessage}");
                }
            }

            model.Categorias = GetCategorias();
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
                    var response = await client.DeleteAsync($"produto/id:int?id={id}");

                    if (response.IsSuccessStatusCode)
                    {
                        return Json(new { success = true });
                        //return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Ocorreu um erro ao tentar excluir o Produto.");
                        return Json(new { success = false, message = "Ocorreu um erro ao tentar excluir o Produto." });
                    }
                }
            }

            return Json(new { success = false, message = "Modelo inválido!" });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
