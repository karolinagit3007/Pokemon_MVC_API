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

        public async Task<IActionResult> Index()
        {
            List<Pokemon> pokemonList = new List<Pokemon>();

            try
            {
                pokemonList = await _pokemonService.GetAllPokemonAsync(); // Obtener todos los Pokémon
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error: {ex.Message}");
            }

            return View(pokemonList); // Pasar la lista de Pokémon a la vista
        }


        public IActionResult Privacy()
        {
            return View();
        }
    }



}
