using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using System.Runtime.CompilerServices;

namespace DunnettMSP2.ViewModels
{
    //a base view model to simplify View-to-ViewModel bound properties
    public abstract class BaseViewModel : BindableObject
    {

        private Dictionary<string, object> _boundProperties = new Dictionary<string, object>();

        //changes an existing property or adds a new one to the dictionary
        protected void SetProperty<T>(T value, [CallerMemberName] string propertyName = "")
        {
            if (!_boundProperties.ContainsKey(propertyName))
                _boundProperties.Add(propertyName, default(T));

            var oldValue = GetProperty<T>(propertyName);
            if (!EqualityComparer<T>.Default.Equals(oldValue, value))
            {
                _boundProperties[propertyName] = value;
                OnPropertyChanged(propertyName);
            }
        }

        //get the property if it exists in the dictionary
        protected T GetProperty<T>([CallerMemberName] string propertyName = "")
        {
            if (!_boundProperties.ContainsKey(propertyName))
                return default;

            return (T)_boundProperties[propertyName];
        }
    }
}
