namespace Application.DTOs;

public class LivroDTO
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public int GeneroId { get; set; }
    public int AutorId { get; set; }
    public string GeneroNome { get; set; } = string.Empty;
    public string AutorNome { get; set; } = string.Empty;
}

public class CreateLivroDTO
{
    public string Titulo { get; set; } = string.Empty;
    public int GeneroId { get; set; }
    public int AutorId { get; set; }
}

public class UpdateLivroDTO
{
    public string Titulo { get; set; } = string.Empty;
    public int GeneroId { get; set; }
    public int AutorId { get; set; }
} 