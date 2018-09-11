namespace Assignment_28263103.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Comments : DbContext
    {
        public Comments()
            : base("name=Comments")
        {
        }

        public virtual DbSet<Comment> Comment { get; set; }
        public virtual DbSet<Post> Posts { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Comment>()
                .Property(e => e.Comment1)
                .IsFixedLength();

            modelBuilder.Entity<Comment>()
                .Property(e => e.DateTime)
                .IsFixedLength();

            modelBuilder.Entity<Post>()
                .Property(e => e.Post1)
                .IsFixedLength();

            modelBuilder.Entity<Post>()
                .Property(e => e.Approved)
                .IsFixedLength();

            modelBuilder.Entity<Post>()
                .Property(e => e.Date)
                .IsFixedLength();
        }

        public System.Data.Entity.DbSet<Assignment_28263103.Models.CaloriesBurnt> CaloriesBurnts { get; set; }

        public System.Data.Entity.DbSet<Assignment_28263103.Models.CaloriesEaten> CaloriesEatens { get; set; }
    }
}
