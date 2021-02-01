using SimpleX.Data.Entities.Base;

namespace SimpleX.Data.Entities
{
    public abstract class Translation : TrackEntity
    {
        public long Id { get; set; }
        public string LanguageCode { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class CategoryTranslation : Translation
    {
        public long CategoryID { get; set; }
    }

    public class CountryTranslation : Translation
    {
        public long CountryID { get; set; }
    }

    public class EnumTranslation : Translation
    {
        public long EnumID { get; set; }
    }

    public class LanguageTranslation : Translation
    {
        public long LanguageID { get; set; }
    }

    public class AppMenuTranslation : Translation
    {
        public long MenuID { get; set; }
    }

    public class ProductTranslation : Translation
    {
        public long ProductID { get; set; }
    }

    public class ProjectTranslation : Translation
    {
        public long ProjectID { get; set; }
    }

    public class UnitTranslation : Translation
    {
        public long UnitID { get; set; }
    }

    public class AttributeTranslation : Translation
    {
        public long AttributeID { get; set; }
    }

}