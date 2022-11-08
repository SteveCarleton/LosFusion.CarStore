using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LosFusion.CarStore.BusinessLogicLayer.Entities;

[Table("Cars")]
public class CarEntity
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Model { get; set; } = string.Empty;

    [Required]
    public Colors Color { get; set; }

    [Required]
    [Range(1900, 2022)]
    public int Year { get; set; }
}

public enum Colors
{
    Black,
    White,
    Blue,
    Red,
    Green
}