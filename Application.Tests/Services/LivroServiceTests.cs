using Application.Common;
using Application.DTOs;
using Application.Services;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Application.Tests.Services;

public class LivroServiceTests
{
    private readonly DbContextOptions<ApplicationDbContext> _options;
    private readonly IMapper _mapper;

    public LivroServiceTests()
    {
        _options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Livro, LivroDTO>()
                .ForMember(dest => dest.GeneroNome, opt => opt.MapFrom(src => src.Genero.Nome))
                .ForMember(dest => dest.AutorNome, opt => opt.MapFrom(src => src.Autor.Nome));
            
            cfg.CreateMap<CreateLivroDTO, Livro>();
            cfg.CreateMap<UpdateLivroDTO, Livro>();
            cfg.CreateMap<Genero, GeneroDTO>();
            cfg.CreateMap<Autor, AutorDTO>();
        });

        _mapper = mapperConfig.CreateMapper();
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllLivros()
    {
        // Arrange
        using var context = new ApplicationDbContext(_options);
        var service = new LivroService(context, _mapper);

        // Adicionar dados de teste
        var genero = new Genero { Nome = "Ficção" };
        var autor = new Autor { Nome = "João Silva" };
        context.Generos.Add(genero);
        context.Autores.Add(autor);
        await context.SaveChangesAsync();

        var livro = new Livro { Titulo = "Teste Livro", GeneroId = genero.Id, AutorId = autor.Id };
        context.Livros.Add(livro);
        await context.SaveChangesAsync();

        // Act
        var result = await service.GetAllAsync();

        // Assert
        Assert.True(result.Success);
        Assert.Single(result.Data!);
        Assert.Equal("Teste Livro", result.Data!.First().Titulo);
    }

    [Fact]
    public async Task GetByIdAsync_WithValidId_ShouldReturnLivro()
    {
        // Arrange
        using var context = new ApplicationDbContext(_options);
        var service = new LivroService(context, _mapper);

        var genero = new Genero { Nome = "Ficção" };
        var autor = new Autor { Nome = "João Silva" };
        context.Generos.Add(genero);
        context.Autores.Add(autor);
        await context.SaveChangesAsync();

        var livro = new Livro { Titulo = "Teste Livro", GeneroId = genero.Id, AutorId = autor.Id };
        context.Livros.Add(livro);
        await context.SaveChangesAsync();

        // Act
        var result = await service.GetByIdAsync(livro.Id);

        // Assert
        Assert.True(result.Success);
        Assert.Equal("Teste Livro", result.Data!.Titulo);
    }

    [Fact]
    public async Task GetByIdAsync_WithInvalidId_ShouldReturnError()
    {
        // Arrange
        using var context = new ApplicationDbContext(_options);
        var service = new LivroService(context, _mapper);

        // Act
        var result = await service.GetByIdAsync(999);

        // Assert
        Assert.False(result.Success);
        Assert.Contains("não encontrado", result.Message);
    }

    [Fact]
    public async Task CreateAsync_WithValidData_ShouldCreateLivro()
    {
        // Arrange
        using var context = new ApplicationDbContext(_options);
        var service = new LivroService(context, _mapper);

        var genero = new Genero { Nome = "Ficção" };
        var autor = new Autor { Nome = "João Silva" };
        context.Generos.Add(genero);
        context.Autores.Add(autor);
        await context.SaveChangesAsync();

        var createDto = new CreateLivroDTO
        {
            Titulo = "Novo Livro",
            GeneroId = genero.Id,
            AutorId = autor.Id
        };

        // Act
        var result = await service.CreateAsync(createDto);

        // Assert
        Assert.True(result.Success);
        Assert.Equal("Novo Livro", result.Data!.Titulo);
        Assert.Equal(genero.Id, result.Data.GeneroId);
        Assert.Equal(autor.Id, result.Data.AutorId);
    }

    [Fact]
    public async Task CreateAsync_WithInvalidGeneroId_ShouldReturnError()
    {
        // Arrange
        using var context = new ApplicationDbContext(_options);
        var service = new LivroService(context, _mapper);

        var autor = new Autor { Nome = "João Silva" };
        context.Autores.Add(autor);
        await context.SaveChangesAsync();

        var createDto = new CreateLivroDTO
        {
            Titulo = "Novo Livro",
            GeneroId = 999, // ID inválido
            AutorId = autor.Id
        };

        // Act
        var result = await service.CreateAsync(createDto);

        // Assert
        Assert.False(result.Success);
        Assert.Contains("Gênero não encontrado", result.Message);
    }

    [Fact]
    public async Task UpdateAsync_WithValidData_ShouldUpdateLivro()
    {
        // Arrange
        using var context = new ApplicationDbContext(_options);
        var service = new LivroService(context, _mapper);

        var genero = new Genero { Nome = "Ficção" };
        var autor = new Autor { Nome = "João Silva" };
        context.Generos.Add(genero);
        context.Autores.Add(autor);
        await context.SaveChangesAsync();

        var livro = new Livro { Titulo = "Livro Original", GeneroId = genero.Id, AutorId = autor.Id };
        context.Livros.Add(livro);
        await context.SaveChangesAsync();

        var updateDto = new UpdateLivroDTO
        {
            Titulo = "Livro Atualizado",
            GeneroId = genero.Id,
            AutorId = autor.Id
        };

        // Act
        var result = await service.UpdateAsync(livro.Id, updateDto);

        // Assert
        Assert.True(result.Success);
        Assert.Equal("Livro Atualizado", result.Data!.Titulo);
    }

    [Fact]
    public async Task DeleteAsync_WithValidId_ShouldDeleteLivro()
    {
        // Arrange
        using var context = new ApplicationDbContext(_options);
        var service = new LivroService(context, _mapper);

        var genero = new Genero { Nome = "Ficção" };
        var autor = new Autor { Nome = "João Silva" };
        context.Generos.Add(genero);
        context.Autores.Add(autor);
        await context.SaveChangesAsync();

        var livro = new Livro { Titulo = "Livro para Deletar", GeneroId = genero.Id, AutorId = autor.Id };
        context.Livros.Add(livro);
        await context.SaveChangesAsync();

        // Act
        var result = await service.DeleteAsync(livro.Id);

        // Assert
        Assert.True(result.Success);
        Assert.Empty(context.Livros);
    }
} 