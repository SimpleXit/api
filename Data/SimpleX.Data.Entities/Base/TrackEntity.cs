using System;
using System.ComponentModel.DataAnnotations;

namespace SimpleX.Data.Entities.Base
{
    public abstract class TrackEntity
    {
        protected TrackEntity() : base()
        {

        }

        [Required]
        public DateTime? CreatedOn { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        [Required]
        public DateTime? UpdatedOn { get; set; }

        [MaxLength(50)]
        public string UpdatedBy { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        [MaxLength(50)]
        public string DeletedBy { get; set; }

    }
}
