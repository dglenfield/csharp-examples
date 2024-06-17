using System.ComponentModel.DataAnnotations; // To use [Required] and [StringLength].

namespace Northwind.EntityModels;

public class Category
{
    public int CategoryId { get; set; }

    [Required]
    [StringLength(15)]
    public string CategoryName { get; set; } = null!;

    public string? Description { get; set; }
}
