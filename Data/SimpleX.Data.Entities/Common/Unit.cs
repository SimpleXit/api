using System.Collections.Generic;

namespace SimpleX.Data.Entities
{
    public class Unit
    {
        #region Public Properties

        public long Id { get; set; }
        public string UnitCode { get; set; }

        #endregion

        #region Navigation Properties

        public List<UnitTranslation> Translations { get; set; }

        #endregion
    }
}
