using Microsoft.EntityFrameworkCore;

namespace Harry_Potter.Modelos
{
    public class ContextoBD : DbContext
    {
        public ContextoBD(DbContextOptions<ContextoBD> options)
            : base(options)
        {
        }

        public DbSet<Película> Películas { get; set; }
    }
}

