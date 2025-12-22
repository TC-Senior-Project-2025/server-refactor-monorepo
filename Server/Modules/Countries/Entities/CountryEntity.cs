
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Server.Modules.Countries.Entities;

[Table("countries")]
[Index(nameof(Code), IsUnique = true)]
public class CountryEntity
{
    [Key]
    [Column("id")]
    public required int Id { get; set; }
    
    [Column("code")]
    public required string Code { get; set; }

    [Required]
    [Column("name")]
    public required string Name { get; set; }

    [Required]
    [Column("efficiency")]
    public int Efficiency { get; set; } = 50;

    [Required]
    [Column("treasury")]
    public int Treasury { get; set; } = 50;

    [Required]
    [Column("stability")]
    public int Stability { get; set; } = 50;

    [Required]
    [Column("manpower")]
    public int Manpower { get; set; } = 50;

    [Required]
    [Column("prestige")]
    public int Prestige { get; set; } = 50;

    [Column("history")]
    public string? History { get; set; }
}