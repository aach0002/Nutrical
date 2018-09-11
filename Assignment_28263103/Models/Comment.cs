namespace Assignment_28263103.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Comment
    {
        public int Id { get; set; }

        [Column("Comment")]
        [Required]
        [StringLength(10)]
        public string Comment1 { get; set; }

        public int PostId { get; set; }

        [Column(TypeName = "timestamp")]
        [MaxLength(8)]
        [Timestamp]
        public byte[] DateTime { get; set; }

        public virtual Post Post { get; set; }
    }
}
