using Microsoft.EntityFrameworkCore;

namespace eagles_food_backend.Data
{
    public class LunchDbContext : DbContext
    {
        public LunchDbContext(DbContextOptions<LunchDbContext> options) : base(options)
        {
                
        }
    }
}
