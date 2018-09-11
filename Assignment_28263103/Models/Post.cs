namespace Assignment_28263103.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Post
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Post()
        {
            Comments = new HashSet<Comment>();
        }

        [Required]
        [StringLength(128)]
        public string UserId { get; set; }

        [Column("Post")]
        [Required]
        [StringLength(10)]
        public string Post1 { get; set; }

        [Required]
        [StringLength(10)]
        public string Approved { get; set; }

        [Column(TypeName = "timestamp")]
        [MaxLength(8)]
        [Timestamp]
        public byte[] Date { get; set; }

        public int id { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Comment> Comments { get; set; }
    }
}
