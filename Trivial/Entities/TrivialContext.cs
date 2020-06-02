using Microsoft.EntityFrameworkCore;

namespace Trivial.Entities
{
    public partial class TrivialContext : DbContext
    {
        public TrivialContext()
        {
        }

        public TrivialContext(DbContextOptions<TrivialContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Trivial> Trivial { get; set; }
    }
}
