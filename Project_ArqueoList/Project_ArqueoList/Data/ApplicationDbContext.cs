using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Project_ArqueoList.Models;
using System.Reflection.Emit;

namespace Project_ArqueoList.Data
{
    public class ApplicationDbContext : IdentityDbContext<Utilizador>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {}
        public DbSet<Utilizador> Utilizador { get; set; }

        public DbSet<Administrador> Administradores { get; set; }

        public DbSet<Utente> Utentes { get; set; }

        public DbSet<Autor> Autores { get; set; }

        public DbSet<Artigo> Artigos { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public DbSet<Validacao> Validacao { get; set; }

        public DbSet<Artigo_Tag> ArtigoTags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Relação de Validação - Utilizador
            modelBuilder.Entity<Validacao>()
                .HasOne(v => v.Utilizador)
                .WithMany(u => u.ListaValidacao)
                .HasForeignKey(v => v.ID_Administrador)
                .IsRequired();

            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = "adm", Name = "Admin", NormalizedName = "Administrador" },
                new IdentityRole { Id = "utente", Name = "Utente", NormalizedName = "Utente" },
                new IdentityRole { Id = "autor", Name = "Autor", NormalizedName = "Autor"}
            );
        }
    }
}
