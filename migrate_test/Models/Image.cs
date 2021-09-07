using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace migrate_test.Models
{
    public class Image
    {
        [Key]
        [Column("IMAGE_ID")]
        public string ImageID { get; set; }

        [ForeignKey("FK_Sample")]
        [Column("SAMPLE_ID")]
        public int SampleID { get; set; }

        [Required]
        [Column("IMAGE_NO")]
        public int ImageNO { get; set; }

        [Column("IMAGE_CODE")]
        public string ImageCode { get; set; }

        [Column("ORIGINAL_FILENAME")]
        public string OriginalFilename { get; set; }

        [Column("IMAGE_SCHEMA")]
        public string ImageScheme { get; set; }
    }
}
