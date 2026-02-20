using Kimbito.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Kimbito.Infra.Database;

public class KimbitoDbContext : DbContext
{
    public KimbitoDbContext(DbContextOptions<KimbitoDbContext> options)
        : base(options)
    {
    }

    public DbSet<Utilizador> Utilizadores { get; set; }
    public DbSet<Categoria> Categorias { get; set; }
    public DbSet<Produto> Produtos { get; set; }
    public DbSet<FormaPagamento> FormasPagamento { get; set; }
    public DbSet<Morada> Moradas { get; set; }
    public DbSet<Encomenda> Encomendas { get; set; }
    public DbSet<EncomendaItem> EncomendaItens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Utilizador>().HasKey(e => e.Id);
        modelBuilder.Entity<Categoria>().HasKey(e => e.Id);
        modelBuilder.Entity<Produto>().HasKey(e => e.Id);
        modelBuilder.Entity<Produto>().HasOne(p => p.Categoria).WithMany().HasForeignKey(p => p.CategoriaId);
        modelBuilder.Entity<FormaPagamento>().HasKey(e => e.Id);
        modelBuilder.Entity<Morada>().HasKey(e => e.Id);
        modelBuilder.Entity<Morada>().HasOne(m => m.Utilizador).WithMany().HasForeignKey(m => m.UtilizadorId);
        modelBuilder.Entity<Encomenda>().HasKey(e => e.Id);
        modelBuilder.Entity<Encomenda>().HasOne(e => e.Utilizador).WithMany().HasForeignKey(e => e.UtilizadorId);
        modelBuilder.Entity<Encomenda>().HasOne(e => e.Morada).WithMany().HasForeignKey(e => e.MoradaId);
        modelBuilder.Entity<Encomenda>().HasOne(e => e.FormaPagamento).WithMany().HasForeignKey(e => e.FormaPagamentoId);
        modelBuilder.Entity<Encomenda>().HasMany(e => e.Itens).WithOne(i => i.Encomenda).HasForeignKey(i => i.EncomendaId);
        modelBuilder.Entity<EncomendaItem>().HasKey(e => e.Id);
        modelBuilder.Entity<EncomendaItem>().HasOne(i => i.Produto).WithMany().HasForeignKey(i => i.ProdutoId);
    }
}