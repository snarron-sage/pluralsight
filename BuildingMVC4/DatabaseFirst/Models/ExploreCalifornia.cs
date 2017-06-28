namespace DatabaseFirst.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ExploreCalifornia : DbContext
    {
        public ExploreCalifornia()
            : base("name=MyDbContext")
        {
        }

        public virtual DbSet<Rating> Ratings { get; set; }
        public virtual DbSet<Tour> Tours { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Rating>()
                .Property(e => e.Name)
                .IsFixedLength();

            modelBuilder.Entity<Rating>()
                .HasMany(e => e.Tours)
                .WithRequired(e => e.Rating)
                .WillCascadeOnDelete(false);
        }
    }
}
