using System;
using System.Windows;
using Microsoft.Phone.Controls;

namespace Phone.Helpers.Services
{
    public static class AppNavigationService
    {
        /// Information zum Frame http://msdn.microsoft.com/en-us/library/ff402536(v=vs.92).aspx
        /// </summary>
        private static PhoneApplicationFrame _rootFrame
            = (PhoneApplicationFrame)Application.Current.RootVisual;
        
        /// <summary>
        /// Navigiert in der History um einen Eintrag zurück.
        /// </summary>
        public static void GoBack()
        {
            _rootFrame.GoBack();
        }

        /// <summary>
        /// Navigiert zur Ziel-Uri
        /// </summary>
        /// <param name="targetUri">Ziel-Uri</param>
        public static void NavigateTo(Uri targetUri)
        {
            _rootFrame.Navigate(targetUri);
        }
    }
}
