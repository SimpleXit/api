using SimpleX.Domain.Models.Base;
using System;

namespace SimpleX.Domain.Models.Shared
{
    public class NoteDto : Dto<NoteDto>
    {
        private long _id;
        private string _title;
        private string _description;

        private DateTime? _createdOn;
        private string _createdBy;
        private DateTime? _updatedOn;
        private string _updatedBy;


        public long Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        public DateTime? CreatedOn
        {
            get => _createdOn;
            set => SetProperty(ref _createdOn, value);
        }
        public string CreatedBy
        {
            get => _createdBy;
            set => SetProperty(ref _createdBy, value);
        }
        public DateTime? UpdatedOn
        {
            get => _updatedOn;
            set => SetProperty(ref _updatedOn, value);
        }
        public string UpdatedBy
        {
            get => _updatedBy;
            set => SetProperty(ref _updatedBy, value);
        }

        public override string ToString()
        {
            return $"{Title}: {Description}";
        }
    }


}
