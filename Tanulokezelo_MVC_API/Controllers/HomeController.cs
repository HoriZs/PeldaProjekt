using System.Diagnostics;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Tanulokezelo_MVC_API.Models;

namespace Tanulokezelo_MVC_API.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient httpClient;
        //Http kliens API híváshoz
        public HomeController(IHttpClientFactory factory)
        {
            httpClient = factory.CreateClient();
            httpClient.BaseAddress = new Uri("https://localhost:7286");
            // Az alkalmazás portja / URL-je
        }
        public async Task<IActionResult> Index()
        {
            //API hívás: GET /api/studentapi
            var valasz = await httpClient.GetAsync("api/studentapi");
            //API -> JSON fájl érkezik

            var json = await valasz.Content.ReadAsStringAsync();
            var lista = JsonSerializer.Deserialize<List<StudentDTO>>(json,new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            return View(lista);
        }
        //Egy diák megjelenítése
        public async Task<IActionResult> Details(int om)
        {
            // API hívás - GET /api/studentapi/71610355
            var valasz = await httpClient.GetAsync($"api/studentapi/{om}");

            if (!valasz.IsSuccessStatusCode) return NotFound();

            // HTTP csomag -> JSON fájl (content)
            var json = await valasz.Content.ReadAsStringAsync();

            //JSON -> C# Objektum(StudentDTO)
            var tanulo = JsonSerializer.Deserialize<StudentDTO>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return View(tanulo);
        }


        // Ûrlap megnyitása
        public IActionResult Create()
        {
            return View();
        }
        //Tanuló felvétele
        [HttpPost]
        public async Task<IActionResult> Create(StudentDTO tanulo)
        {
            //C# objektum -> JSON
            var json = JsonSerializer.Serialize(tanulo);
            var data = new StringContent(json,Encoding.UTF8,"application/json");

            //API hívás: POST /api/studentapi/create
            await httpClient.PostAsync("/api/studentapi/create", data);

            return RedirectToAction("Index");
        }
        //Törlés
        public async Task<IActionResult> Delete(int om)
        {
            //API hívás: DELETE /api/studentapi/71610355
            await httpClient.DeleteAsync($"api/studentapi/delete/{om}");

            return RedirectToAction("Index");
        }
        //Módosítás
        public async Task<IActionResult> Update(int om)
        {
            //API hívás: GET /api/studentapi/71610255
            
            var valasz = await httpClient.GetAsync($"api/studentapi/{om}");

            if (!valasz.IsSuccessStatusCode) return NotFound();

            var json = await valasz.Content.ReadAsStringAsync();
            var tanulo = JsonSerializer.Deserialize<StudentDTO>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            
            
            return View(tanulo);
        }
        [HttpPost]
        public async Task<IActionResult> Update(StudentDTO tanulo)
        {
            // C# objektum -> JSON
            var json = JsonSerializer.Serialize(tanulo);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            //API hívás: PUT /api/studentapi/update/71610255
            await httpClient.PutAsync($"/api/studentapi/update/{tanulo.OMazonosito}", data);
            return RedirectToAction("Index");
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
