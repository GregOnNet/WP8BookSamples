using System;
using System.ComponentModel;


namespace DataBindingSimple
{
    public class Datenmodell : INotifyPropertyChanged
    {
        
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, 
                                new PropertyChangedEventArgs
                                    (propertyName));
            }
        }
        
        private string _message;
        public string Message 
        {
            get
            {
                if(String.IsNullOrEmpty(_message))
                {
                    _message = @"Diese Nachricht übermittelt die Klasse Datenmodell!";
                }
                return _message;
            }
            set
            {
                _message = value;
                NotifyPropertyChanged("Message");
            }
        }

        
    }
}
