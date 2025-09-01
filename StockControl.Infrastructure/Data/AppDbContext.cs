using Microsoft.EntityFrameworkCore;
using StockControl.Models;

namespace StockControl.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Material> Materiais => Set<Material>();
        public DbSet<MovimentoEstoque> MovimentosEstoque => Set<MovimentoEstoque>();
        public DbSet<Usuario> Usuarios { get; set; } = null!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Material>(e =>
            {
                e.Property(x => x.Nome).IsRequired().HasMaxLength(120);
                e.Property(x => x.UnidadeMedida).IsRequired().HasMaxLength(20);
                e.Property(x => x.CustoUnitario).HasPrecision(18,2);
            });

            modelBuilder.Entity<MovimentoEstoque>(e =>
            {
                e.Property(x => x.ValorTotal).HasPrecision(18,2);
                e.Property(x => x.Tipo).IsRequired().HasMaxLength(10);
                e.HasOne(x => x.Material)
                    .WithMany()
                    .HasForeignKey(x => x.MaterialId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}