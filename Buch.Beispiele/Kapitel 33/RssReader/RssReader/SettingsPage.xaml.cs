using System.Windows;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using System.IO.IsolatedStorage;

namespace RssReader
{
	public partial class SettingsPage : PhoneApplicationPage
	{
		private bool isInitialized;

		public SettingsPage()
		{
			InitializeComponent();
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);

			isInitialized = false;
			SyncEnabled.IsChecked = App.AgentsEnabled;
		}

		private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
		{
			isInitialized = true;
		}

		private void SyncEnabled_Checked(object sender, RoutedEventArgs e)
		{
			if (!isInitialized)
			{
				return;
			}

			if (SyncAgentManager.ActivateAgent())
			{
				App.AgentsEnabled = true;
			}
			else
			{
				MessageBox.Show("Die Hintergrund-Synchronisierug konnte nicht erfolgreich registriert werden.");
			}
		}

		private void SyncEnabled_Unchecked(object sender, RoutedEventArgs e)
		{
			if (!isInitialized)
			{
				return;
			}

			SyncAgentManager.StopAgents();
			App.AgentsEnabled = false;
		}
		
	}
}