using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using RssReader.DataAccess;

namespace RssReader
{
	public partial class MainPage : PhoneApplicationPage
	{
		private Task<List<NewsItem>> loadDataTask;
		private ProgressIndicator indicator;
		private NewsRepository repository;
		
		public MainPage()
		{
			InitializeComponent();

			// Repository für Laden/Sync erzeugen
			repository = new NewsRepository();

			// ProgressIndicator erzeugen und anzeigen
			indicator = new ProgressIndicator();
			indicator.Text = "Lade lokale Daten...";
			indicator.IsIndeterminate = true;
			indicator.IsVisible = true;
			SystemTray.SetProgressIndicator(this, indicator);

			// Asynchrone Laden der lokalen Daten starten
			loadDataTask = repository.LoadLocalDataAsync();
		}
		
		private async void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
		{
			// Lokale Daten anzeigen
			List<NewsItem> localData = await loadDataTask;
			DataContext = localData;

			if (App.AgentsEnabled)
			{
				// Agent neu starten, um zeitliches ablaufen zu verhindern
				SyncAgentManager.ActivateAgent();
			}

			// Tile Counter zurücksetzen
			ClearTileCounter();

			// Sync anstoßen und Anzeige aktualisieren
			indicator.Text = "Synchronisiere...";
			await repository.SyncNewsAsync();
			indicator.IsVisible = false;
			DataContext = await repository.LoadLocalDataAsync();
		}

		private void ClearTileCounter()
		{
			ShellTile appTile = ShellTile.ActiveTiles.First();
			appTile.Update(new StandardTileData() { Count = 0 });
		}
		
		private void ListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
		{
			// Ausgewählte News abrufen
			NewsItem selectedNews = NewsListBox.SelectedItem as NewsItem;
			if (selectedNews == null)
			{
				return;
			}

			// News als gelesen markieren
			repository.MarkAsRead(selectedNews);
			
			// News im Browser anzeigen
			WebBrowserTask task = new WebBrowserTask();
			task.Uri = new Uri(selectedNews.Link, UriKind.Absolute);
			task.Show();
		}

		private void Settings_Click(object sender, EventArgs e)
		{
			NavigationService.Navigate(new Uri("/SettingsPage.xaml", UriKind.Relative));
		}
	}
}