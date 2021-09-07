using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace migrate_test.Models
{
    public class Sample
    {
        [Key]
        [Column("SAMPLE_ID")]
        public int SampleID { get; set; }
        [Column("DATASET_ID")]
        public string DatasetID { get; set; }
        [Column("SAMPLE_TYPE")]
        public int SampleType { get; set; }
        [Column("METADATA")]
        public string Metadata { get; set; }
        [Column("IMAGE_COUNT")]
        public int ImageCount { get; set; }

        public List<Image> Images { get; set; }
    }
}
