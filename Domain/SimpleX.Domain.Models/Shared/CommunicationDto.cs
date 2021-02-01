using SimpleX.Common.Enums;
using SimpleX.Domain.Models.Base;
using System;

namespace SimpleX.Domain.Models.Shared
{
    public class CommunicationDto : Dto<CommunicationDto>
    {
        private long _id;
        private long? _employeeID;
        private DateTime _communicationDate;
        private CommunicationType _communicationType;
        private string _description;
        private string _remarks;
        private string _correspondents;

        public long Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }
        public long? EmployeeID
        {
            get => _employeeID;
            set => SetProperty(ref _employeeID, value);
        }

        public DateTime CommunicationDate
        {
            get => _communicationDate;
            set => SetProperty(ref _communicationDate, value);
        }
        public CommunicationType CommunicationType
        {
            get => _communicationType;
            set => SetProperty(ref _communicationType, value);
        }
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }
        public string Remarks
        {
            get => _remarks;
            set => SetProperty(ref _remarks, value);
        }
        public string Correspondents
        {
            get => _correspondents;
            set => SetProperty(ref _correspondents, value);
        }

        public override string ToString()
        {
            return $"{CommunicationDate.ToShortDateString()}: {CommunicationType.ToString()}";
        }
    }


}
