using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Server.Modules.Countries.Entities;

namespace Server.Modules.Commanderies.Entities;

/// <summary>
/// Represents a territorial administrative division within a country.
/// The <c>CommanderyEntity</c> defines the essential properties and relationships
/// associated with a commandery, including its demographics, economic data,
/// societal metrics, and associated country.
/// </summary>
[Table("commanderies")]
public class CommanderyEntity
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("country_id")]
    public int CountryId { get; set; }

    [ForeignKey(nameof(CountryId))]
    public required CountryEntity Country { get; set; }

    [Required]
    [Column("code")]
    public required string Code { get; set; }

    [Required]
    [Column("name")]
    public required string Name { get; set; }

    [Required]
    [Column("population")]
    public int Population { get; set; }

    [Required]
    [Column("wealth")]
    public int Wealth { get; set; }

    [Required]
    [Column("unrest")]
    public int Unrest { get; set; }

    [Required]
    [Column("defense")]
    public int Defense { get; set; }

    [Column("history")]
    public string History { get; set; } = string.Empty;
}