using SimpleX.Domain.Models.Base;
using SimpleX.Domain.Models.Shared;
using System;
using System.ComponentModel;

namespace SimpleX.Domain.Models.Relations
{
    public class AppointmentDto : Dto<AppointmentDto>
    {
        private long _id;
        private byte _storeID;
        private string _subject;
        private string _description;
        private string _location;
        private bool _allDay;
        private DateTime _start;
        private DateTime _end;
        private long? _categoryID;
        private int _statusID;
        private long? _customerID;
        private long? _employeeID;
        private long? _projectID;

        public long Id
            {
            get => _id;
            set => SetProperty(ref _id, value);
        }
        public byte StoreID
        {
            get => _storeID;
            set => SetProperty(ref _storeID, value);
        }
        public string Subject
        {
            get => _subject;
            set => SetProperty(ref _subject, value);
        }
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }
        public string Location
        {
            get => _location;
            set => SetProperty(ref _location, value);
        }
        public bool AllDay
        {
            get => _allDay;
            set => SetProperty(ref _allDay, value);
        }
        public DateTime Start
        {
            get => _start;
            set => SetProperty(ref _start, value);
        }
        public DateTime StartDate
        {
            get => _start.Date;
            set
            {
                Start = new DateTime(value.Year, value.Month, value.Day, _start.Hour, _start.Minute, 0, 0);
                NotifyPropertyChanged("StartDate");
            }
        }
        public TimeSpan StartTime
        {
            get => _end.TimeOfDay;
            set
            {
                Start = new DateTime(_start.Year, _start.Month, _start.Day, value.Hours, value.Minutes, 0, 0);
                NotifyPropertyChanged("StartTime");
            }
        }
        public DateTime End
        {
            get => _end;
            set => SetProperty(ref _end, value);
        }
        public DateTime EndDate
        {
            get => _end.Date;
            set
            {
                End = new DateTime(value.Year, value.Month, value.Day, _end.Hour, _end.Minute, 0, 0);
                NotifyPropertyChanged("EndDate");
            }
        }
        public TimeSpan EndTime
        {
            get => _end.TimeOfDay;
            set
            {
                End = new DateTime(_end.Year, _end.Month, _end.Day, value.Hours, value.Minutes, 0, 0);
                NotifyPropertyChanged("EndTime");
            }
        }
        public long? CategoryID
        {
            get => _categoryID;
            set => SetProperty(ref _categoryID, value);
        }
        public int StatusID
        {
            get => _statusID;
            set => SetProperty(ref _statusID, value);
        }
        public long? CustomerID
        {
            get => _customerID;
            set => SetProperty(ref _customerID, value);
        }
        public long? EmployeeID
        {
            get => _employeeID;
            set => SetProperty(ref _employeeID, value);
        }
        public long? ProjectID
        {
            get => _projectID;
            set => SetProperty(ref _projectID, value);
        }

        public BindingList<NoteDto> Notes { get; set; } = new BindingList<NoteDto>();

        public override string ToString()
        {
            return $"{Start.ToShortDateString()}: {Subject}";
        }
    }
}
