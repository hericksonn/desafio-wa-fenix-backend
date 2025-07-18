using Application.Common;
using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces("application/json")]
public class AutoresController : ControllerBase
{
    private readonly IAutorService _autorService;

    public AutoresController(IAutorService autorService)
    {
        _autorService = autorService;
    }

    /// <summary>
    /// Obtém todos os autores
    /// </summary>
    /// <returns>Lista de autores</returns>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<List<AutorDTO>>), 200)]
    [ProducesResponseType(typeof(ApiResponse), 500)]
    public async Task<ActionResult<ApiResponse<List<AutorDTO>>>> GetAll()
    {
        var result = await _autorService.GetAllAsync();
        return Ok(result);
    }

    /// <summary>
    /// Obtém um autor por ID
    /// </summary>
    /// <param name="id">ID do autor</param>
    /// <returns>Autor encontrado</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<AutorDTO>), 200)]
    [ProducesResponseType(typeof(ApiResponse), 404)]
    [ProducesResponseType(typeof(ApiResponse), 500)]
    public async Task<ActionResult<ApiResponse<AutorDTO>>> GetById(int id)
    {
        var result = await _autorService.GetByIdAsync(id);
        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }

    /// <summary>
    /// Cria um novo autor
    /// </summary>
    /// <param name="createDto">Dados do autor a ser criado</param>
    /// <returns>Autor criado</returns>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<AutorDTO>), 201)]
    [ProducesResponseType(typeof(ApiResponse), 400)]
    [ProducesResponseType(typeof(ApiResponse), 500)]
    public async Task<ActionResult<ApiResponse<AutorDTO>>> Create([FromBody] CreateAutorDTO createDto)
    {
        var result = await _autorService.CreateAsync(createDto);
        if (!result.Success)
            return BadRequest(result);

        return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result);
    }

    /// <summary>
    /// Atualiza um autor existente
    /// </summary>
    /// <param name="id">ID do autor</param>
    /// <param name="updateDto">Dados atualizados do autor</param>
    /// <returns>Autor atualizado</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponse<AutorDTO>), 200)]
    [ProducesResponseType(typeof(ApiResponse), 400)]
    [ProducesResponseType(typeof(ApiResponse), 404)]
    [ProducesResponseType(typeof(ApiResponse), 500)]
    public async Task<ActionResult<ApiResponse<AutorDTO>>> Update(int id, [FromBody] UpdateAutorDTO updateDto)
    {
        var result = await _autorService.UpdateAsync(id, updateDto);
        if (!result.Success)
        {
            if (result.Message.Contains("não encontrado"))
                return NotFound(result);
            return BadRequest(result);
        }

        return Ok(result);
    }

    /// <summary>
    /// Exclui um autor
    /// </summary>
    /// <param name="id">ID do autor</param>
    /// <returns>Confirmação da exclusão</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponse), 200)]
    [ProducesResponseType(typeof(ApiResponse), 400)]
    [ProducesResponseType(typeof(ApiResponse), 404)]
    [ProducesResponseType(typeof(ApiResponse), 500)]
    public async Task<ActionResult<ApiResponse>> Delete(int id)
    {
        var result = await _autorService.DeleteAsync(id);
        if (!result.Success)
        {
            if (result.Message.Contains("não encontrado"))
                return NotFound(result);
            return BadRequest(result);
        }

        return Ok(result);
    }
} 