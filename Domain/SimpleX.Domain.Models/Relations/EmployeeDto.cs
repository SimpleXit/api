using SimpleX.Domain.Models.Base;

namespace SimpleX.Domain.Models.Relations
{
    public class EmployeeDto : Dto<EmployeeDto>
    {
        private long _id;
        private string _firstName;
        private string _lastName;

        public long Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }
        public string FirstName
        {
            get => _firstName;
            set => SetProperty(ref _firstName, value);
        }
        public string LastName
        {
            get => _lastName;
            set => SetProperty(ref _lastName, value);
        }


        public override string ToString()
        {
            return $"{FirstName} {LastName}";
        }
    }


}
