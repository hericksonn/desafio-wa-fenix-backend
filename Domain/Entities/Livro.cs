using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

[Table("Livros")]
public class Livro
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(200)]
    public string Titulo { get; set; } = string.Empty;

    [Required]
    public int GeneroId { get; set; }

    [Required]
    public int AutorId { get; set; }

    [ForeignKey("GeneroId")]
    public virtual Genero Genero { get; set; } = null!;

    [ForeignKey("AutorId")]
    public virtual Autor Autor { get; set; } = null!;
} 