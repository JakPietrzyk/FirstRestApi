using System.ComponentModel.DataAnnotations;

namespace restApiTutorial.Dtos
{
    public record CreateItemDto
    {
        [Required] //in post cant be null 
        public string Name { get; init; }
        [Required]
        [Range(0, 1000)] //set range of inserting value
        public decimal Price { get; init; }
    }
}
