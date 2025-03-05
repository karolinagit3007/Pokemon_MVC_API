using Newtonsoft.Json;
using Pokemon_MVC_API.Models;

namespace Pokemon_MVC_API.Services
{
    public class PokemonService
    {
        private readonly HttpClient _httpClient;
        private readonly string apiURL = "https://pokeapi.co/api/v2/pokemon/";

        // Constructor público que recibe HttpClient
        public PokemonService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Pokemon>> GetAllPokemonAsync()
        {
            var response = await _httpClient.GetStringAsync($"{apiURL}?limit=20"); // Obtén los primeros 20 Pokémon
            dynamic data = JsonConvert.DeserializeObject(response);

            if (data == null || data.results == null)
            {
                throw new Exception("No se pudo obtener la lista de Pokémon.");
            }

            List<Pokemon> pokemonList = new List<Pokemon>();

            foreach (var item in data.results)
            {
                string pokemonUrl = item.url;
                var pokemonResponse = await _httpClient.GetStringAsync(pokemonUrl);
                dynamic pokemonData = JsonConvert.DeserializeObject(pokemonResponse);

                if (pokemonData != null && pokemonData.sprites != null)
                {
                    pokemonList.Add(new Pokemon
                    {
                        name = pokemonData.name,
                        ImageUrl = pokemonData.sprites.front_default // URL de la imagen
                    });
                }
            }

            return pokemonList;
        }


    }
}
