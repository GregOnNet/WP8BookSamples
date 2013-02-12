using System;
using System.Diagnostics;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Phone.Info;
using Microsoft.Phone.Scheduler;
using Microsoft.Phone.Shell;
using RssReader.DataAccess;

namespace RssReader.Agents
{
	public class RssSyncScheduledAgent : ScheduledTaskAgent
	{
		private static volatile bool _classInitialized;

		/// <remarks>
		/// ScheduledAgent constructor, initializes the UnhandledException handler
		/// </remarks>
		public RssSyncScheduledAgent()
		{
			if (!_classInitialized)
			{
				_classInitialized = true;
				// Subscribe to the managed exception handler
				Deployment.Current.Dispatcher.BeginInvoke(delegate
				{
					Application.Current.UnhandledException += ScheduledAgent_UnhandledException;
				});
			}
		}

		/// Code to execute on Unhandled Exceptions
		private void ScheduledAgent_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
		{
			if (System.Diagnostics.Debugger.IsAttached)
			{
				// An unhandled exception has occurred; break into the debugger
				System.Diagnostics.Debugger.Break();
			}
		}

		/// <summary>
		/// Agent that runs a scheduled task
		/// </summary>
		/// <param name="task">
		/// The invoked task
		/// </param>
		/// <remarks>
		/// This method is called when a periodic or resource intensive task is invoked
		/// </remarks>
		protected override void OnInvoke(ScheduledTask task)
		{
			OutputAvailableMemory("Start");
						
			try
			{
				DateTime newsDate = (DateTime)IsolatedStorageSettings.ApplicationSettings["NewsDate"];
				NewsRepository repository = new NewsRepository();

				OutputAvailableMemory("Repo erzeugt");

				repository.GetNewNewsCountAsync(newsDate).ContinueWith(t =>
				{
					if (t.Status == TaskStatus.RanToCompletion)
					{
						ShellTile appTile = ShellTile.ActiveTiles.First();
						appTile.Update(new StandardTileData() { Count = t.Result });
					}

					if (task is ResourceIntensiveTask)
					{
						// Bei einem RessourceIntensiveTask alte Nachrichten löschen
						DateTime timeLimit = DateTime.Now.AddMonths(-1);
						repository.DeleteNewsOlderThan(timeLimit);
					}

					OutputAvailableMemory("Ende");
					NotifyComplete();
				});			
			}
			catch (Exception ex)
			{
				// Nichts machen
				NotifyComplete();
			}			
		}

		[Conditional("DEBUG")]
		private void OutputAvailableMemory(string message)
		{
			long x = DeviceStatus.ApplicationMemoryUsageLimit - DeviceStatus.ApplicationCurrentMemoryUsage;
			Debug.WriteLine("*** {0} - verfügbar: {1} Bytes", message, x);
		}
	}
}