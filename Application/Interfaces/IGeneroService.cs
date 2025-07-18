using Application.Common;
using Application.DTOs;

namespace Application.Interfaces;

public interface IGeneroService
{
    Task<ApiResponse<List<GeneroDTO>>> GetAllAsync();
    Task<ApiResponse<GeneroDTO>> GetByIdAsync(int id);
    Task<ApiResponse<GeneroDTO>> CreateAsync(CreateGeneroDTO createDto);
    Task<ApiResponse<GeneroDTO>> UpdateAsync(int id, UpdateGeneroDTO updateDto);
    Task<ApiResponse> DeleteAsync(int id);
} 