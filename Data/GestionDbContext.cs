using Microsoft.EntityFrameworkCore;
using todoItemProject.Models;

namespace todoItemProject.Data
{
    public class GestionDbContext:DbContext
    {
        public GestionDbContext(DbContextOptions<GestionDbContext> options)
            :base(options)
        {

        }
            public DbSet<Category> categories{ get; set; }
            public DbSet<Product> products { get; set; }
      


    }
}
