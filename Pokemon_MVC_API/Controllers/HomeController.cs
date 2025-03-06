using Microsoft.AspNetCore.Mvc;
using Pokemon_MVC_API.Models;
using Pokemon_MVC_API.Services;

namespace Pokemon_MVC_API.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly PokemonService _pokemonService;

        public HomeController(ILogger<HomeController> logger, PokemonService pokemonService)
        {
            _logger = logger;
            _pokemonService = pokemonService;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            const int pageSize = 20;
            List<Pokemon> pokemonList = await _pokemonService.GetPokemonByPageAsync(page, pageSize);
            ViewBag.CurrentPage = page;
            return View(pokemonList);
        }

        // Nueva acci�n para mostrar los detalles de un Pok�mon
        public async Task<IActionResult> Details(string id) // "id" es el nombre del Pok�mon
        {
            try
            {
                // Obtener los detalles del Pok�mon
                var pokemon = await _pokemonService.GetPokemonDetailsAsync(id);
                if (pokemon == null)
                {
                    return NotFound(); // Si no se encuentra el Pok�mon, devuelve un error 404
                }
                return View(pokemon); // Pasar el Pok�mon a la vista de detalles
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error: {ex.Message}");
                return RedirectToAction("Index"); // Redirigir a la lista principal en caso de error
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }


}
