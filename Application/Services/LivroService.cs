using Application.Common;
using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class LivroService : ILivroService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public LivroService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ApiResponse<List<LivroDTO>>> GetAllAsync()
    {
        try
        {
            var livros = await _context.Livros
                .Include(l => l.Genero)
                .Include(l => l.Autor)
                .ToListAsync();
            
            var livrosDto = _mapper.Map<List<LivroDTO>>(livros);
            return ApiResponse<List<LivroDTO>>.SuccessResult(livrosDto, "Livros recuperados com sucesso");
        }
        catch (Exception ex)
        {
            return ApiResponse<List<LivroDTO>>.ErrorResult($"Erro ao recuperar livros: {ex.Message}");
        }
    }

    public async Task<ApiResponse<LivroDTO>> GetByIdAsync(int id)
    {
        try
        {
            var livro = await _context.Livros
                .Include(l => l.Genero)
                .Include(l => l.Autor)
                .FirstOrDefaultAsync(l => l.Id == id);

            if (livro == null)
                return ApiResponse<LivroDTO>.ErrorResult("Livro não encontrado");

            var livroDto = _mapper.Map<LivroDTO>(livro);
            return ApiResponse<LivroDTO>.SuccessResult(livroDto, "Livro recuperado com sucesso");
        }
        catch (Exception ex)
        {
            return ApiResponse<LivroDTO>.ErrorResult($"Erro ao recuperar livro: {ex.Message}");
        }
    }

    public async Task<ApiResponse<List<LivroDTO>>> GetByGeneroAsync(int generoId)
    {
        try
        {
            var livros = await _context.Livros
                .Include(l => l.Genero)
                .Include(l => l.Autor)
                .Where(l => l.GeneroId == generoId)
                .ToListAsync();

            var livrosDto = _mapper.Map<List<LivroDTO>>(livros);
            return ApiResponse<List<LivroDTO>>.SuccessResult(livrosDto, "Livros por gênero recuperados com sucesso");
        }
        catch (Exception ex)
        {
            return ApiResponse<List<LivroDTO>>.ErrorResult($"Erro ao recuperar livros por gênero: {ex.Message}");
        }
    }

    public async Task<ApiResponse<List<LivroDTO>>> GetByAutorAsync(int autorId)
    {
        try
        {
            var livros = await _context.Livros
                .Include(l => l.Genero)
                .Include(l => l.Autor)
                .Where(l => l.AutorId == autorId)
                .ToListAsync();

            var livrosDto = _mapper.Map<List<LivroDTO>>(livros);
            return ApiResponse<List<LivroDTO>>.SuccessResult(livrosDto, "Livros por autor recuperados com sucesso");
        }
        catch (Exception ex)
        {
            return ApiResponse<List<LivroDTO>>.ErrorResult($"Erro ao recuperar livros por autor: {ex.Message}");
        }
    }

    public async Task<ApiResponse<LivroDTO>> CreateAsync(CreateLivroDTO createDto)
    {
        try
        {
            var genero = await _context.Generos.FindAsync(createDto.GeneroId);
            if (genero == null)
                return ApiResponse<LivroDTO>.ErrorResult("Gênero não encontrado");

            var autor = await _context.Autores.FindAsync(createDto.AutorId);
            if (autor == null)
                return ApiResponse<LivroDTO>.ErrorResult("Autor não encontrado");

            var livro = _mapper.Map<Livro>(createDto);
            _context.Livros.Add(livro);
            await _context.SaveChangesAsync();

            await _context.Entry(livro).Reference(l => l.Genero).LoadAsync();
            await _context.Entry(livro).Reference(l => l.Autor).LoadAsync();

            var livroDto = _mapper.Map<LivroDTO>(livro);
            return ApiResponse<LivroDTO>.SuccessResult(livroDto, "Livro criado com sucesso");
        }
        catch (Exception ex)
        {
            return ApiResponse<LivroDTO>.ErrorResult($"Erro ao criar livro: {ex.Message}");
        }
    }

    public async Task<ApiResponse<LivroDTO>> UpdateAsync(int id, UpdateLivroDTO updateDto)
    {
        try
        {
            var livro = await _context.Livros.FindAsync(id);
            if (livro == null)
                return ApiResponse<LivroDTO>.ErrorResult("Livro não encontrado");

            // Validar se o gênero existe
            var genero = await _context.Generos.FindAsync(updateDto.GeneroId);
            if (genero == null)
                return ApiResponse<LivroDTO>.ErrorResult("Gênero não encontrado");

            // Validar se o autor existe
            var autor = await _context.Autores.FindAsync(updateDto.AutorId);
            if (autor == null)
                return ApiResponse<LivroDTO>.ErrorResult("Autor não encontrado");

            _mapper.Map(updateDto, livro);
            await _context.SaveChangesAsync();

            // Recarregar com relacionamentos
            await _context.Entry(livro).Reference(l => l.Genero).LoadAsync();
            await _context.Entry(livro).Reference(l => l.Autor).LoadAsync();

            var livroDto = _mapper.Map<LivroDTO>(livro);
            return ApiResponse<LivroDTO>.SuccessResult(livroDto, "Livro atualizado com sucesso");
        }
        catch (Exception ex)
        {
            return ApiResponse<LivroDTO>.ErrorResult($"Erro ao atualizar livro: {ex.Message}");
        }
    }

    public async Task<ApiResponse> DeleteAsync(int id)
    {
        try
        {
            var livro = await _context.Livros.FindAsync(id);
            if (livro == null)
                return ApiResponse.ErrorResult("Livro não encontrado");

            _context.Livros.Remove(livro);
            await _context.SaveChangesAsync();

            return ApiResponse.SuccessResult("Livro excluído com sucesso");
        }
        catch (Exception ex)
        {
            return ApiResponse.ErrorResult($"Erro ao excluir livro: {ex.Message}");
        }
    }
} 