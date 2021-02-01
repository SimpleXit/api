using System;

namespace SimpleX.Data.Entities.Interfaces
{
    public interface IEntity
    {
        long Id { get; set; }
    }

    public interface ITrackEntity : IEntity
    {
        DateTime? CreatedOn { get; set; }
        string CreatedBy { get; set; }
        DateTime? UpdatedOn { get; set; }
        string UpdatedBy { get; set; }
    }
}
