using Prism.Mvvm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace WPFValidationINotifyDataErrorInfo
{
    public class MainViewModel : BindableBase, INotifyDataErrorInfo
    {
        private string _userName;
        private readonly Dictionary<string, List<string>> _errorsByPropertyName = new Dictionary<string, List<string>>();

        public MainViewModel()
        {
            UserName = null;
        }

        public string UserName
        {
            get => _userName;
            set
            {
                _userName = value;
                ValidateUserName();
                RaisePropertyChanged();
            }
        }

        public bool HasErrors => _errorsByPropertyName.Any();

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public IEnumerable GetErrors(string propertyName)
        {
            return _errorsByPropertyName.ContainsKey(propertyName) ?
                _errorsByPropertyName[propertyName] : null;
        }

        private void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        private void ValidateUserName()
        {
            ClearErrors(nameof(UserName));
            if (string.IsNullOrWhiteSpace(UserName))
                AddError(nameof(UserName), "Username cannot be empty.");
            if (string.Equals(UserName, "Admin", StringComparison.OrdinalIgnoreCase))
                AddError(nameof(UserName), "Admin is not valid username.");
            if (UserName == null || UserName?.Length <= 5)
                AddError(nameof(UserName), "Username must be at least 6 characters long.");
        }

        private void AddError(string propertyName, string error)
        {
            if (!_errorsByPropertyName.ContainsKey(propertyName))
                _errorsByPropertyName[propertyName] = new List<string>();

            if (!_errorsByPropertyName[propertyName].Contains(error))
            {
                _errorsByPropertyName[propertyName].Add(error);
                OnErrorsChanged(propertyName);
            }
        }

        private void ClearErrors(string propertyName)
        {
            if (_errorsByPropertyName.ContainsKey(propertyName))
            {
                _errorsByPropertyName.Remove(propertyName);
                OnErrorsChanged(propertyName);
            }
        }
    }
}
