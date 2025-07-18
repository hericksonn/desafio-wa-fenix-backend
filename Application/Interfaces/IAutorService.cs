using Application.Common;
using Application.DTOs;

namespace Application.Interfaces;

public interface IAutorService
{
    Task<ApiResponse<List<AutorDTO>>> GetAllAsync();
    Task<ApiResponse<AutorDTO>> GetByIdAsync(int id);
    Task<ApiResponse<AutorDTO>> CreateAsync(CreateAutorDTO createDto);
    Task<ApiResponse<AutorDTO>> UpdateAsync(int id, UpdateAutorDTO updateDto);
    Task<ApiResponse> DeleteAsync(int id);
} 