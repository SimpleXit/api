using SimpleX.Data.Entities.Interfaces;

namespace SimpleX.Data.Entities
{
    public class DocumentTree : IEntity
    {
        #region Public Properties

        public long Id { get; set; }

        public long DocumentFromID { get; set; }
        public long DocumentToID { get; set; }

        #endregion

        #region Navigation Properties

        public Document DocumentFrom { get; set; }
        public Document DocumentTo { get; set; }

        #endregion
    }
}
