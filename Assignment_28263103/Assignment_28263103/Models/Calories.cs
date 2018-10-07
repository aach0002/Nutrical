namespace Assignment_28263103.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Calories : DbContext
    {
        public Calories()
            : base("name=Calories")
        {
        }

        public virtual DbSet<CaloriesBurnt> CaloriesBurnts { get; set; }
        public virtual DbSet<CaloriesEaten> CaloriesEatens { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CaloriesEaten>()
                .Property(e => e.Type)
                .IsFixedLength();
        }
    }
}
