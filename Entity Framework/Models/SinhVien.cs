namespace Entity_Framework.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SinhVien")]
    public partial class SinhVien
    {
        [Key]
        [StringLength(20)]
        public string MSSV { get; set; }

        [StringLength(50)]
        public string HoTen { get; set; }

        public int? MaKhoa { get; set; }

        public double? DTB { get; set; }

        public virtual Khoa Khoa { get; set; }
    }
}
