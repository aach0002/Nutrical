namespace Assignment_28263103.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class UserDetail
    {
        [Key]
        public string UserId { get; set; }

        public double Height { get; set; }

        public int Age { get; set; }

        public double Weight { get; set; }
    }
}
