using Application.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        // Genero mappings
        CreateMap<Genero, GeneroDTO>();
        CreateMap<CreateGeneroDTO, Genero>();
        CreateMap<UpdateGeneroDTO, Genero>();

        // Autor mappings
        CreateMap<Autor, AutorDTO>();
        CreateMap<CreateAutorDTO, Autor>();
        CreateMap<UpdateAutorDTO, Autor>();

        // Livro mappings
        CreateMap<Livro, LivroDTO>()
            .ForMember(dest => dest.GeneroNome, opt => opt.MapFrom(src => src.Genero.Nome))
            .ForMember(dest => dest.AutorNome, opt => opt.MapFrom(src => src.Autor.Nome));
        
        CreateMap<CreateLivroDTO, Livro>();
        CreateMap<UpdateLivroDTO, Livro>();
    }
} 