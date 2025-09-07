using System.ComponentModel.DataAnnotations;

namespace Promise.ProductManagementSystem.ViewModels
{
    public class EditProduct
    {
        [Required]
        [StringLength(maximumLength: 50, ErrorMessage = "Name cannot exceed 50 characters.")]
        public string Name { get; set; }
        [StringLength(maximumLength: 200, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string? Description { get; set; }    
        public bool IsActive { get; set; }
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be a positive number.")]
        public decimal Price { get; set; }
    }
}
