using Application.Common;
using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces("application/json")]
public class GenerosController : ControllerBase
{
    private readonly IGeneroService _generoService;

    public GenerosController(IGeneroService generoService)
    {
        _generoService = generoService;
    }

    /// <summary>
    /// Obtém todos os gêneros
    /// </summary>
    /// <returns>Lista de gêneros</returns>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<List<GeneroDTO>>), 200)]
    [ProducesResponseType(typeof(ApiResponse), 500)]
    public async Task<ActionResult<ApiResponse<List<GeneroDTO>>>> GetAll()
    {
        var result = await _generoService.GetAllAsync();
        return Ok(result);
    }

    /// <summary>
    /// Obtém um gênero por ID
    /// </summary>
    /// <param name="id">ID do gênero</param>
    /// <returns>Gênero encontrado</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<GeneroDTO>), 200)]
    [ProducesResponseType(typeof(ApiResponse), 404)]
    [ProducesResponseType(typeof(ApiResponse), 500)]
    public async Task<ActionResult<ApiResponse<GeneroDTO>>> GetById(int id)
    {
        var result = await _generoService.GetByIdAsync(id);
        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }

    /// <summary>
    /// Cria um novo gênero
    /// </summary>
    /// <param name="createDto">Dados do gênero a ser criado</param>
    /// <returns>Gênero criado</returns>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<GeneroDTO>), 201)]
    [ProducesResponseType(typeof(ApiResponse), 400)]
    [ProducesResponseType(typeof(ApiResponse), 500)]
    public async Task<ActionResult<ApiResponse<GeneroDTO>>> Create([FromBody] CreateGeneroDTO createDto)
    {
        var result = await _generoService.CreateAsync(createDto);
        if (!result.Success)
            return BadRequest(result);

        return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result);
    }

    /// <summary>
    /// Atualiza um gênero existente
    /// </summary>
    /// <param name="id">ID do gênero</param>
    /// <param name="updateDto">Dados atualizados do gênero</param>
    /// <returns>Gênero atualizado</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponse<GeneroDTO>), 200)]
    [ProducesResponseType(typeof(ApiResponse), 400)]
    [ProducesResponseType(typeof(ApiResponse), 404)]
    [ProducesResponseType(typeof(ApiResponse), 500)]
    public async Task<ActionResult<ApiResponse<GeneroDTO>>> Update(int id, [FromBody] UpdateGeneroDTO updateDto)
    {
        var result = await _generoService.UpdateAsync(id, updateDto);
        if (!result.Success)
        {
            if (result.Message.Contains("não encontrado"))
                return NotFound(result);
            return BadRequest(result);
        }

        return Ok(result);
    }

    /// <summary>
    /// Exclui um gênero
    /// </summary>
    /// <param name="id">ID do gênero</param>
    /// <returns>Confirmação da exclusão</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponse), 200)]
    [ProducesResponseType(typeof(ApiResponse), 400)]
    [ProducesResponseType(typeof(ApiResponse), 404)]
    [ProducesResponseType(typeof(ApiResponse), 500)]
    public async Task<ActionResult<ApiResponse>> Delete(int id)
    {
        var result = await _generoService.DeleteAsync(id);
        if (!result.Success)
        {
            if (result.Message.Contains("não encontrado"))
                return NotFound(result);
            return BadRequest(result);
        }

        return Ok(result);
    }
} 