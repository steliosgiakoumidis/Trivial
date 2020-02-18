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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                //optionsBuilder.UseSqlServer("Server=192.168.0.29,1433;Database=Trivial;User Id=SA;Password=Oresti18;");
                optionsBuilder.UseSqlServer("Server=stelios\\sqlexpress;Database=Trivial;Integrated Security=true;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Trivial>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Category)
                    .IsRequired()
                    .HasColumnName("category");

                entity.Property(e => e.CorrectAnswer)
                    .IsRequired()
                    .HasColumnName("correct_answer");

                entity.Property(e => e.Difficulty)
                    .IsRequired()
                    .HasColumnName("difficulty");

                entity.Property(e => e.IncorrectAnswers)
                    .IsRequired()
                    .HasColumnName("incorrect_answers");

                entity.Property(e => e.Question)
                    .IsRequired()
                    .HasColumnName("question");
            });
        }
    }
}
