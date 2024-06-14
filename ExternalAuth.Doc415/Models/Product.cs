using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductManagement.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }

    [Required]
    public DateTime DateAdded { get; set; }

    [Required]
    [Column(TypeName = "decimal(5, 2)")]
    [Range(0.1, double.MaxValue, ErrorMessage = "Price must be a positive value.")]
    public decimal Price { get; set; }

    public bool IsActive { get; set; } = true;

}
