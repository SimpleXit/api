using SimpleX.Common.Enums;

namespace SimpleX.Data.Entities
{
    public class DocumentNumber
    {
        public int Year { get; set; }
        public DocumentJournal Journal { get; set; }
        public byte StoreID { get; set; }
        public byte LevelID { get; set; }
        public byte SourceID { get; set; }

        public int Number { get; set; }
    }

    public class DocumentSource
    {
        public DocumentJournal Journal { get; set; }
        public byte StoreID { get; set; }
        public byte LevelID { get; set; }
        public byte SourceID { get; set; }

        public string Description { get; set; }
    }

    public class DocumentLevel
    {
        public DocumentJournal Journal { get; set; }
        public byte StoreID { get; set; }
        public byte LevelID { get; set; }

        public string Description { get; set; }
    }

    public class DocumentDefaultTree
    {
        public DocumentJournal Journal { get; set; }
        public byte StoreID { get; set; }

        public byte ParentLevelID { get; set; }
        public byte ParentSourceID { get; set; }

        public int Sequence { get; set; }

        public byte ChildLevelID { get; set; }
        public byte ChildSourceID { get; set; }
    }
}
