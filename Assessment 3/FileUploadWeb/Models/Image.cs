namespace FileUploadWeb.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Image
    {
        [Key]
        [Column("​Id​")]
        public int C_Id_ { get; set; }

        [Column("​Path​")]
        [Required]
        [StringLength(50)]
        public string C_Path_ { get; set; }

        [Column("​Name​")]
        [Required]
        [StringLength(50)]
        public string C_Name_ { get; set; }
    }
}
