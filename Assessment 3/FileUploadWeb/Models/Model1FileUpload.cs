namespace FileUploadWeb.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Model1FileUpload : DbContext
    {
        public Model1FileUpload()
            : base("name=Model1FileUpload")
        {
        }

        public virtual DbSet<Image> Images { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Image>()
                .Property(e => e.C_Path_)
                .IsUnicode(false);

            modelBuilder.Entity<Image>()
                .Property(e => e.C_Name_)
                .IsUnicode(false);
        }
    }
}
