using SimpleX.Domain.Models.Interfaces;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace SimpleX.Domain.Models.Base
{
    public abstract class Dto<T> : INotifyPropertyChanged, IIsDirty where T : class
    {
        public Dto()
        {
            //this.Validate();
        }

        private bool _isDirty = false;

        public void Init()
        {
            _isDirty = false;

            //this.Validate();
        }

        public bool IsDirty()
        {
            return _isDirty || IsChildDirty();
        }

        private bool IsChildDirty()
        {
            var listOfIsDirty = from p in this.GetType().GetProperties()
                                let pi = p.PropertyType
                                where pi.GetInterfaces().Any(i => i.Equals(typeof(IEnumerable)))
                                   && pi.IsGenericType
                                   && pi.GetGenericArguments().Any(a => a.GetInterfaces().Any(i => i.Equals(typeof(IIsDirty))))
                                select p;

            //Loop through all properties of type IEnumerable<IIsDirty>:
            foreach (PropertyInfo pi in listOfIsDirty.ToList())
            {
                //Return true if collection contains dirty element:
                if (((IEnumerable<IIsDirty>)pi.GetValue(this)).Any(i => i.IsDirty()))
                    return true;
            }
            return false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void SetProperty<P>(ref P prop, P value, [CallerMemberName] string propertyName = null)
        {
            try
            {
                if (EqualityComparer<P>.Default.Equals(prop, value)) return;

                prop = value;
                //this.Validate();
                NotifyPropertyChanged(propertyName);
                
                _isDirty = true;
            }
            catch 
            {
                throw;
            }
        }

        #region IDataErrorInfo Members

        //protected IValidator<T> validator;
        //private ValidationResult validationResult;

        //protected abstract IValidator<T> Validator { get; }

        //[JsonIgnore]
        //public bool IsValid
        //{
        //    get
        //    {
        //        if (validationResult != null)
        //            return validationResult.IsValid;
        //        else
        //            return true;
        //    }
        //}

        //public void Validate()
        //{
        //    validationResult = Validator.Validate(this as T);
        //    NotifyPropertyChanged("IsValid");
        //    NotifyPropertyChanged("Error");
        //}

        //[JsonIgnore]
        //public string Error
        //{
        //    get 
        //    {
        //        if (validationResult != null)
        //            return validationResult.ToString();
        //        else
        //            return string.Empty;
        //    }
        //}

        //public string this[string columnName]
        //{
        //    get
        //    {
        //        if (validationResult != null)
        //            return string.Join(Environment.NewLine, validationResult.Errors.Where(x => x.PropertyName == columnName));
        //        else
        //            return string.Empty;
        //    }
        //}

        #endregion
    }
}
