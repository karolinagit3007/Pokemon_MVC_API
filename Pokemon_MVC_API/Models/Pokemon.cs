namespace Pokemon_MVC_API.Models
{
    public class Pokemon
    {
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public int Height { get; set; } // Altura en decímetros
        public int Weight { get; set; } // Peso en hectogramos
        public dynamic Abilities { get; set; } // Lista de habilidades
    }
}