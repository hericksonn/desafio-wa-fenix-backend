using Application.Common;
using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class AutorService : IAutorService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public AutorService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ApiResponse<List<AutorDTO>>> GetAllAsync()
    {
        try
        {
            var autores = await _context.Autores.ToListAsync();
            var autoresDto = _mapper.Map<List<AutorDTO>>(autores);
            return ApiResponse<List<AutorDTO>>.SuccessResult(autoresDto, "Autores recuperados com sucesso");
        }
        catch (Exception ex)
        {
            return ApiResponse<List<AutorDTO>>.ErrorResult($"Erro ao recuperar autores: {ex.Message}");
        }
    }

    public async Task<ApiResponse<AutorDTO>> GetByIdAsync(int id)
    {
        try
        {
            var autor = await _context.Autores.FindAsync(id);
            if (autor == null)
                return ApiResponse<AutorDTO>.ErrorResult("Autor não encontrado");

            var autorDto = _mapper.Map<AutorDTO>(autor);
            return ApiResponse<AutorDTO>.SuccessResult(autorDto, "Autor recuperado com sucesso");
        }
        catch (Exception ex)
        {
            return ApiResponse<AutorDTO>.ErrorResult($"Erro ao recuperar autor: {ex.Message}");
        }
    }

    public async Task<ApiResponse<AutorDTO>> CreateAsync(CreateAutorDTO createDto)
    {
        try
        {
            var existingAutor = await _context.Autores.FirstOrDefaultAsync(a => a.Nome == createDto.Nome);
            if (existingAutor != null)
                return ApiResponse<AutorDTO>.ErrorResult("Já existe um autor com este nome");

            var autor = _mapper.Map<Autor>(createDto);
            _context.Autores.Add(autor);
            await _context.SaveChangesAsync();

            var autorDto = _mapper.Map<AutorDTO>(autor);
            return ApiResponse<AutorDTO>.SuccessResult(autorDto, "Autor criado com sucesso");
        }
        catch (Exception ex)
        {
            return ApiResponse<AutorDTO>.ErrorResult($"Erro ao criar autor: {ex.Message}");
        }
    }

    public async Task<ApiResponse<AutorDTO>> UpdateAsync(int id, UpdateAutorDTO updateDto)
    {
        try
        {
            var autor = await _context.Autores.FindAsync(id);
            if (autor == null)
                return ApiResponse<AutorDTO>.ErrorResult("Autor não encontrado");

            var existingAutor = await _context.Autores.FirstOrDefaultAsync(a => a.Nome == updateDto.Nome && a.Id != id);
            if (existingAutor != null)
                return ApiResponse<AutorDTO>.ErrorResult("Já existe um autor com este nome");

            _mapper.Map(updateDto, autor);
            await _context.SaveChangesAsync();

            var autorDto = _mapper.Map<AutorDTO>(autor);
            return ApiResponse<AutorDTO>.SuccessResult(autorDto, "Autor atualizado com sucesso");
        }
        catch (Exception ex)
        {
            return ApiResponse<AutorDTO>.ErrorResult($"Erro ao atualizar autor: {ex.Message}");
        }
    }

    public async Task<ApiResponse> DeleteAsync(int id)
    {
        try
        {
            var autor = await _context.Autores.FindAsync(id);
            if (autor == null)
                return ApiResponse.ErrorResult("Autor não encontrado");

            var hasLivros = await _context.Livros.AnyAsync(l => l.AutorId == id);
            if (hasLivros)
                return ApiResponse.ErrorResult("Não é possível excluir um autor que possui livros associados");

            _context.Autores.Remove(autor);
            await _context.SaveChangesAsync();

            return ApiResponse.SuccessResult("Autor excluído com sucesso");
        }
        catch (Exception ex)
        {
            return ApiResponse.ErrorResult($"Erro ao excluir autor: {ex.Message}");
        }
    }
} 