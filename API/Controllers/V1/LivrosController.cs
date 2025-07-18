using Application.Common;
using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces("application/json")]
public class LivrosController : ControllerBase
{
    private readonly ILivroService _livroService;

    public LivrosController(ILivroService livroService)
    {
        _livroService = livroService;
    }

    /// <summary>
    /// Obtém todos os livros
    /// </summary>
    /// <returns>Lista de livros com informações de gênero e autor</returns>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<List<LivroDTO>>), 200)]
    [ProducesResponseType(typeof(ApiResponse), 500)]
    public async Task<ActionResult<ApiResponse<List<LivroDTO>>>> GetAll()
    {
        var result = await _livroService.GetAllAsync();
        return Ok(result);
    }

    /// <summary>
    /// Obtém um livro por ID
    /// </summary>
    /// <param name="id">ID do livro</param>
    /// <returns>Livro encontrado com informações de gênero e autor</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<LivroDTO>), 200)]
    [ProducesResponseType(typeof(ApiResponse), 404)]
    [ProducesResponseType(typeof(ApiResponse), 500)]
    public async Task<ActionResult<ApiResponse<LivroDTO>>> GetById(int id)
    {
        var result = await _livroService.GetByIdAsync(id);
        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }

    /// <summary>
    /// Obtém livros por gênero
    /// </summary>
    /// <param name="generoId">ID do gênero</param>
    /// <returns>Lista de livros do gênero especificado</returns>
    [HttpGet("genero/{generoId}")]
    [ProducesResponseType(typeof(ApiResponse<List<LivroDTO>>), 200)]
    [ProducesResponseType(typeof(ApiResponse), 500)]
    public async Task<ActionResult<ApiResponse<List<LivroDTO>>>> GetByGenero(int generoId)
    {
        var result = await _livroService.GetByGeneroAsync(generoId);
        return Ok(result);
    }

    /// <summary>
    /// Obtém livros por autor
    /// </summary>
    /// <param name="autorId">ID do autor</param>
    /// <returns>Lista de livros do autor especificado</returns>
    [HttpGet("autor/{autorId}")]
    [ProducesResponseType(typeof(ApiResponse<List<LivroDTO>>), 200)]
    [ProducesResponseType(typeof(ApiResponse), 500)]
    public async Task<ActionResult<ApiResponse<List<LivroDTO>>>> GetByAutor(int autorId)
    {
        var result = await _livroService.GetByAutorAsync(autorId);
        return Ok(result);
    }

    /// <summary>
    /// Cria um novo livro
    /// </summary>
    /// <param name="createDto">Dados do livro a ser criado</param>
    /// <returns>Livro criado com informações de gênero e autor</returns>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<LivroDTO>), 201)]
    [ProducesResponseType(typeof(ApiResponse), 400)]
    [ProducesResponseType(typeof(ApiResponse), 500)]
    public async Task<ActionResult<ApiResponse<LivroDTO>>> Create([FromBody] CreateLivroDTO createDto)
    {
        var result = await _livroService.CreateAsync(createDto);
        if (!result.Success)
            return BadRequest(result);

        return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result);
    }

    /// <summary>
    /// Atualiza um livro existente
    /// </summary>
    /// <param name="id">ID do livro</param>
    /// <param name="updateDto">Dados atualizados do livro</param>
    /// <returns>Livro atualizado com informações de gênero e autor</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponse<LivroDTO>), 200)]
    [ProducesResponseType(typeof(ApiResponse), 400)]
    [ProducesResponseType(typeof(ApiResponse), 404)]
    [ProducesResponseType(typeof(ApiResponse), 500)]
    public async Task<ActionResult<ApiResponse<LivroDTO>>> Update(int id, [FromBody] UpdateLivroDTO updateDto)
    {
        var result = await _livroService.UpdateAsync(id, updateDto);
        if (!result.Success)
        {
            if (result.Message.Contains("não encontrado"))
                return NotFound(result);
            return BadRequest(result);
        }

        return Ok(result);
    }

    /// <summary>
    /// Exclui um livro
    /// </summary>
    /// <param name="id">ID do livro</param>
    /// <returns>Confirmação da exclusão</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponse), 200)]
    [ProducesResponseType(typeof(ApiResponse), 400)]
    [ProducesResponseType(typeof(ApiResponse), 404)]
    [ProducesResponseType(typeof(ApiResponse), 500)]
    public async Task<ActionResult<ApiResponse>> Delete(int id)
    {
        var result = await _livroService.DeleteAsync(id);
        if (!result.Success)
        {
            if (result.Message.Contains("não encontrado"))
                return NotFound(result);
            return BadRequest(result);
        }

        return Ok(result);
    }
} 