using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace InAppPurchase.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public bool IsInDesigner
        {
            get { return DesignerProperties.IsInDesignTool; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void SendPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
