using Application.Common;
using Application.DTOs;

namespace Application.Interfaces;

public interface ILivroService
{
    Task<ApiResponse<List<LivroDTO>>> GetAllAsync();
    Task<ApiResponse<LivroDTO>> GetByIdAsync(int id);
    Task<ApiResponse<List<LivroDTO>>> GetByGeneroAsync(int generoId);
    Task<ApiResponse<List<LivroDTO>>> GetByAutorAsync(int autorId);
    Task<ApiResponse<LivroDTO>> CreateAsync(CreateLivroDTO createDto);
    Task<ApiResponse<LivroDTO>> UpdateAsync(int id, UpdateLivroDTO updateDto);
    Task<ApiResponse> DeleteAsync(int id);
} 