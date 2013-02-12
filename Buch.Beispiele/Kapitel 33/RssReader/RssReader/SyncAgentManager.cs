using System;
using Microsoft.Phone.Scheduler;

namespace RssReader
{
	public static class SyncAgentManager
	{
		private const string AgentName = "RssSyncAgent";
		private const string AgentIntensiveName = "RssSyncIntensiveAgent";

		public static bool ActivateAgent()
		{
			// Wenn der Task bereits hinzugefügt wurde. dann diesen stoppen
			PeriodicTask syncTask = ScheduledActionService.Find(AgentName) as PeriodicTask;
			if (syncTask != null)
			{
				StopAgent(AgentName);
			}

			// Task anlegen und Beschreibung für Einstellungs-UI setzen
			syncTask = new PeriodicTask(AgentName);
			syncTask.Description = "Download der neusten Nachrichten von Spiegel Online";

			ResourceIntensiveTask intensiveTask = ScheduledActionService.Find(AgentIntensiveName) as ResourceIntensiveTask;
			if (intensiveTask != null)
			{
				StopAgent(AgentIntensiveName);
			}			
			intensiveTask = new ResourceIntensiveTask(AgentIntensiveName);
			
			try
			{
				// Tasks im System registrieren für Ausführung
				ScheduledActionService.Add(syncTask);
				ScheduledActionService.Add(intensiveTask);

#if DEBUG
				// Für's Debuggen: Tasks in 30 bzw. 90sek ausführen
				ScheduledActionService.LaunchForTest(AgentName, TimeSpan.FromSeconds(30));
				ScheduledActionService.LaunchForTest(AgentIntensiveName, TimeSpan.FromSeconds(90));
#endif

				return true;
			}
			catch (InvalidOperationException ex)
			{
				return false;
			}
		}

		public static void StopAgents()
		{
			StopAgent(AgentName);
			StopAgent(AgentIntensiveName);
		}

		public static void StopAgent(string agentName)
		{
			try
			{
				ScheduledActionService.Remove(agentName);
			}
			catch (Exception)
			{
			}
		}
	}
}
