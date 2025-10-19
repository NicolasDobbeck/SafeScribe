using Microsoft.EntityFrameworkCore;
using SafeScribe.Models;

namespace SafeScribe.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        /// <summary>
        /// Tabela de Usuários
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// Tabela de Notas
        /// </summary>
        public DbSet<Note> Notes { get; set; }
    }
}