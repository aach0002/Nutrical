namespace Assignment_28263103.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CaloriesBurnt")]
    public partial class CaloriesBurnt
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [StringLength(128)]
        public string UserId { get; set; }

        [Column("CaloriesBurnt")]
        public double? CaloriesBurnt1 { get; set; }
    }
}
