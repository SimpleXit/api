using SimpleX.Common.Enums;
using SimpleX.Data.Entities.Base;
using SimpleX.Data.Entities.Interfaces;
using System;
using System.Collections.Generic;

namespace SimpleX.Data.Entities
{
    public class Employee : TrackEntity, IEntity
    {
        #region Public Properties

        public long Id { get; set; }
        public string Number { get; set; }
        public byte StoreID { get; set; }
        public long? CategoryID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender? Gender { get; set; }
        public string Nationality { get; set; }
        public string NationalNumber { get; set; } //Rijksregisternummer
        public string IdentityCardNumber { get; set; }
        public DateTime? BirthDate { get; set; }
        public string BirthPlace { get; set; }
        public MaritalStatus? MaritalStatus { get; set; }
        public string LanguageCode { get; set; }
        public string IBAN { get; set; }
        public string BIC { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }


        #endregion

        #region Navigation Properties

        public EmployeeContactInfo ContactInfo { get; set; } = new EmployeeContactInfo();

        public List<EmployeeAddress> Addresses { get; set; } = new List<EmployeeAddress>();
        public List<Appointment> Appointments { get; set; } = new List<Appointment>();
        public List<EmployeeAttachment> Attachments { get; set; } = new List<EmployeeAttachment>();
        public List<Communication> Communications { get; set; } = new List<Communication>();
        public List<EmployeeNote> Notes { get; set; } = new List<EmployeeNote>();
        public List<EmployeePerson> Persons { get; set; } = new List<EmployeePerson>();

        #endregion
    }
}
