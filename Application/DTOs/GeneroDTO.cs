namespace Application.DTOs;

public class GeneroDTO
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
}

public class CreateGeneroDTO
{
    public string Nome { get; set; } = string.Empty;
}

public class UpdateGeneroDTO
{
    public string Nome { get; set; } = string.Empty;
} 