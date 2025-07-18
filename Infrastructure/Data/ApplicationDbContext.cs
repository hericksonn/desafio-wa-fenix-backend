using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Genero> Generos { get; set; }
    public DbSet<Autor> Autores { get; set; }
    public DbSet<Livro> Livros { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Genero>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nome).IsRequired().HasMaxLength(100);
            entity.HasIndex(e => e.Nome).IsUnique();
        });

        modelBuilder.Entity<Autor>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nome).IsRequired().HasMaxLength(100);
            entity.HasIndex(e => e.Nome).IsUnique();
        });

        modelBuilder.Entity<Livro>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Titulo).IsRequired().HasMaxLength(200);
            
            entity.HasOne(e => e.Genero)
                  .WithMany(g => g.Livros)
                  .HasForeignKey(e => e.GeneroId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Autor)
                  .WithMany(a => a.Livros)
                  .HasForeignKey(e => e.AutorId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => e.Titulo);
            entity.HasIndex(e => new { e.GeneroId, e.AutorId });
        });
    }
} 