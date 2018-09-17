namespace Assignment_28263103.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CaloriesEaten")]
    public partial class CaloriesEaten
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Must enter the ammount of calories")]
        [Column("CaloriesEaten")]
        public double? CaloriesEaten1 { get; set; }

        [Required(ErrorMessage = "Must Select a type")]
        [StringLength(1)]
        public string Type { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Date { get; set; }

        [StringLength(128)]
        public string UserId { get; set; }
    }
}
