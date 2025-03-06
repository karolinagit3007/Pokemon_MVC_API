using Newtonsoft.Json;
using Pokemon_MVC_API.Models;

namespace Pokemon_MVC_API.Services
{
    public class PokemonService
    {
        private readonly HttpClient _httpClient;
        private readonly string apiURL = "https://pokeapi.co/api/v2/pokemon/";
        private readonly string speciesURL = "https://pokeapi.co/api/v2/pokemon-species/";

        public PokemonService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Método para obtener los detalles de un Pokémon específico
        public async Task<Pokemon> GetPokemonDetailsAsync(string name)
        {
            var response = await _httpClient.GetStringAsync($"{apiURL}{name}");
            dynamic pokemonData = JsonConvert.DeserializeObject(response);

            if (pokemonData == null || pokemonData.sprites == null)
            {
                throw new Exception("No se pudo obtener la información del Pokémon.");
            }

            // Obtener la descripción del Pokémon
            var speciesResponse = await _httpClient.GetStringAsync($"{speciesURL}{pokemonData.id}");
            dynamic speciesData = JsonConvert.DeserializeObject(speciesResponse);

            string description = "No hay descripción disponible";
            if (speciesData != null && speciesData.flavor_text_entries != null)
            {
                foreach (var entry in speciesData.flavor_text_entries)
                {
                    if (entry.language.name == "es")
                    {
                        description = entry.flavor_text;
                        break;
                    }
                }
            }

            return new Pokemon
            {
                Name = pokemonData.name,
                ImageUrl = pokemonData.sprites.front_default,
                Description = description,
                Height = pokemonData.height, // Altura del Pokémon
                Weight = pokemonData.weight, // Peso del Pokémon
                Abilities = pokemonData.abilities // Habilidades del Pokémon
            };
        }

        // Método existente para obtener la lista de Pokémon por página
        public async Task<List<Pokemon>> GetPokemonByPageAsync(int page, int pageSize)
        {
            int offset = (page - 1) * pageSize;
            var response = await _httpClient.GetStringAsync($"{apiURL}?offset={offset}&limit={pageSize}");
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
                    int pokemonId = pokemonData.id;

                    var speciesResponse = await _httpClient.GetStringAsync($"{speciesURL}{pokemonId}");
                    dynamic speciesData = JsonConvert.DeserializeObject(speciesResponse);

                    string description = "No hay descripción disponible";
                    if (speciesData != null && speciesData.flavor_text_entries != null)
                    {
                        foreach (var entry in speciesData.flavor_text_entries)
                        {
                            if (entry.language.name == "es")
                            {
                                description = entry.flavor_text;
                                break;
                            }
                        }
                    }

                    pokemonList.Add(new Pokemon
                    {
                        Name = pokemonData.name,
                        ImageUrl = pokemonData.sprites.front_default,
                        Description = description
                    });
                }
            }

            return pokemonList;
        }
    }



}