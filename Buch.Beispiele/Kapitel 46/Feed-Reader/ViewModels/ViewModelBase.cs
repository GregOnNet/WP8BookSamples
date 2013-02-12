using System.ComponentModel;
using System.Runtime.CompilerServices;
using AsyncFeedReader.Annotations;
using System.Diagnostics;

namespace AsyncFeedReader.ViewModels
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public bool InDesigner
        {
            get { return DesignerProperties.IsInDesignTool; }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool _isLoading;

        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                if (_isLoading == value)
                {
                    return;
                }

                _isLoading = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
