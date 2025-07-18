using Application.Common;
using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class GeneroService : IGeneroService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GeneroService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ApiResponse<List<GeneroDTO>>> GetAllAsync()
    {
        try
        {
            var generos = await _context.Generos.ToListAsync();
            var generosDto = _mapper.Map<List<GeneroDTO>>(generos);
            return ApiResponse<List<GeneroDTO>>.SuccessResult(generosDto, "Gêneros recuperados com sucesso");
        }
        catch (Exception ex)
        {
            return ApiResponse<List<GeneroDTO>>.ErrorResult($"Erro ao recuperar gêneros: {ex.Message}");
        }
    }

    public async Task<ApiResponse<GeneroDTO>> GetByIdAsync(int id)
    {
        try
        {
            var genero = await _context.Generos.FindAsync(id);
            if (genero == null)
                return ApiResponse<GeneroDTO>.ErrorResult("Gênero não encontrado");

            var generoDto = _mapper.Map<GeneroDTO>(genero);
            return ApiResponse<GeneroDTO>.SuccessResult(generoDto, "Gênero recuperado com sucesso");
        }
        catch (Exception ex)
        {
            return ApiResponse<GeneroDTO>.ErrorResult($"Erro ao recuperar gênero: {ex.Message}");
        }
    }

    public async Task<ApiResponse<GeneroDTO>> CreateAsync(CreateGeneroDTO createDto)
    {
        try
        {
            var existingGenero = await _context.Generos.FirstOrDefaultAsync(g => g.Nome == createDto.Nome);
            if (existingGenero != null)
                return ApiResponse<GeneroDTO>.ErrorResult("Já existe um gênero com este nome");

            var genero = _mapper.Map<Genero>(createDto);
            _context.Generos.Add(genero);
            await _context.SaveChangesAsync();

            var generoDto = _mapper.Map<GeneroDTO>(genero);
            return ApiResponse<GeneroDTO>.SuccessResult(generoDto, "Gênero criado com sucesso");
        }
        catch (Exception ex)
        {
            return ApiResponse<GeneroDTO>.ErrorResult($"Erro ao criar gênero: {ex.Message}");
        }
    }

    public async Task<ApiResponse<GeneroDTO>> UpdateAsync(int id, UpdateGeneroDTO updateDto)
    {
        try
        {
            var genero = await _context.Generos.FindAsync(id);
            if (genero == null)
                return ApiResponse<GeneroDTO>.ErrorResult("Gênero não encontrado");

            var existingGenero = await _context.Generos.FirstOrDefaultAsync(g => g.Nome == updateDto.Nome && g.Id != id);
            if (existingGenero != null)
                return ApiResponse<GeneroDTO>.ErrorResult("Já existe um gênero com este nome");

            _mapper.Map(updateDto, genero);
            await _context.SaveChangesAsync();

            var generoDto = _mapper.Map<GeneroDTO>(genero);
            return ApiResponse<GeneroDTO>.SuccessResult(generoDto, "Gênero atualizado com sucesso");
        }
        catch (Exception ex)
        {
            return ApiResponse<GeneroDTO>.ErrorResult($"Erro ao atualizar gênero: {ex.Message}");
        }
    }

    public async Task<ApiResponse> DeleteAsync(int id)
    {
        try
        {
            var genero = await _context.Generos.FindAsync(id);
            if (genero == null)
                return ApiResponse.ErrorResult("Gênero não encontrado");

            var hasLivros = await _context.Livros.AnyAsync(l => l.GeneroId == id);
            if (hasLivros)
                return ApiResponse.ErrorResult("Não é possível excluir um gênero que possui livros associados");

            _context.Generos.Remove(genero);
            await _context.SaveChangesAsync();

            return ApiResponse.SuccessResult("Gênero excluído com sucesso");
        }
        catch (Exception ex)
        {
            return ApiResponse.ErrorResult($"Erro ao excluir gênero: {ex.Message}");
        }
    }
} 