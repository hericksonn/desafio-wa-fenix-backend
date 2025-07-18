namespace Application.DTOs;

public class AutorDTO
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
}

public class CreateAutorDTO
{
    public string Nome { get; set; } = string.Empty;
}

public class UpdateAutorDTO
{
    public string Nome { get; set; } = string.Empty;
} 