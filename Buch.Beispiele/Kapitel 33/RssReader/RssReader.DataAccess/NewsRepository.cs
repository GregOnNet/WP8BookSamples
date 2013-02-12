using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.Phone.Info;
using RssReader.DataAccess.Util;

namespace RssReader.DataAccess
{
	public class NewsRepository
	{
		private XNamespace contentNs = "http://purl.org/rss/1.0/modules/content/";
		private const string FeedUrl = "http://www.spiegel.de/schlagzeilen/index.rss";

		public Task<List<NewsItem>> LoadLocalDataAsync()
		{
		    return Task.Run(() => LoadLocalNewsData());
		}

		private List<NewsItem> LoadLocalNewsData()
		{
			using (var db = NewsDataContext.Create())
			{
				db.ObjectTrackingEnabled = false;

				return db.News.OrderByDescending(item => item.PublishDate).ToList();
			}
		}

		public async Task<int> GetNewNewsCountAsync(DateTime newestLocalNewsDate)
		{
			long x = DeviceStatus.ApplicationMemoryUsageLimit - DeviceStatus.ApplicationCurrentMemoryUsage;
			Debug.WriteLine("*** Start GetNewNewsCountAsync - verfügbar: {0:F2} MB", x / 1024.0 / 1024.0);

			try
			{
				XDocument rssFeed = await LoadRssFeedAsync(FeedUrl);
				int newNewsCount = rssFeed.Descendants("item").Where(xi => DateTime.Parse(xi.Element("pubDate").Value) >= newestLocalNewsDate).Count();
				return newNewsCount;				
			}
			catch (Exception)
			{
				return 0;
			}
		}

		public async Task<int> SyncNewsAsync()
		{
			long x = DeviceStatus.ApplicationMemoryUsageLimit - DeviceStatus.ApplicationCurrentMemoryUsage;
			Debug.WriteLine("*** Start SyncNews - verfügbar: {0:F2} MB", x / 1024.0 / 1024.0);

			try
			{
				Task<DateTime> latestLocalNewsDateTask = GetNewestLocalNewsDateAsync();
				Task<XDocument> loadRssFeedTask = LoadRssFeedAsync(FeedUrl);

				Task waitingTask = Task.WhenAll(latestLocalNewsDateTask, loadRssFeedTask);
				await waitingTask;

				x = DeviceStatus.ApplicationMemoryUsageLimit - DeviceStatus.ApplicationCurrentMemoryUsage;
				Debug.WriteLine("*** Auf Tasks gewartet - verfügbar: {0:F2} MB", x / 1024.0 / 1024.0);

				if (waitingTask.Status != TaskStatus.RanToCompletion)
				{
					// Wenn was schief ging, weitere Verarbeitung beenden
					return 0;
				}

				XElement rssElement = loadRssFeedTask.Result.Root;
				var items = loadRssFeedTask.Result.Descendants("item");
				List<XElement> itemsToProcess = items.Where(xi => DateTime.Parse(xi.Element("pubDate").Value) >= latestLocalNewsDateTask.Result).ToList();

				x = DeviceStatus.ApplicationMemoryUsageLimit - DeviceStatus.ApplicationCurrentMemoryUsage;
				Debug.WriteLine("*** Feed gefiltert - verfügbar: {0:F2} MB", x / 1024.0 / 1024.0);

				SaveFilteredRssNews(itemsToProcess);

				x = DeviceStatus.ApplicationMemoryUsageLimit - DeviceStatus.ApplicationCurrentMemoryUsage;
				Debug.WriteLine("*** Feed verarbeitet - verfügbar: {0:F2} MB", x / 1024.0 / 1024.0);

				return itemsToProcess.Count;
			}
			catch (Exception ex)
			{
				return 0;
			}
		}

		private void SaveFilteredRssNews(List<XElement> rssNews)
		{
			if (rssNews.Count == 0)
			{
				return;
			}

			using (var db = NewsDataContext.Create())
			{
				foreach (XElement rssItem in rssNews)
				{
					NewsItem news = new NewsItem();
					news.IsRead = false;

					XElement linkElement = rssItem.Element("link");
					if (linkElement == null)
					{
						// Wenn Artikel-URL fehlt, ist die News nichts wert
						continue;
					}

					news.Link = rssItem.Element("link").Value;

					XElement descriptionElement = rssItem.Element("description");
					string preview = descriptionElement != null ? descriptionElement.Value : "";
					news.Preview = preview;

					XElement publishDateElement = rssItem.Element("pubDate");
					if (publishDateElement != null)
					{
						news.PublishDate = DateTime.Parse(publishDateElement.Value);
					}

					XElement thumbnailElement = rssItem.Element("enclosure");
					if (thumbnailElement != null && thumbnailElement.Attribute("url") != null)
					{
						news.Thumbnail = thumbnailElement.Attribute("url").Value;
					}

					XElement titleElement = rssItem.Element("title");
					if (titleElement != null)
					{
						news.Title = titleElement.Value;
					}

					db.News.InsertOnSubmit(news);
				}

				db.SubmitChanges();
			}
		}

		private Task<DateTime> GetNewestLocalNewsDateAsync()
		{
			return Task.Run<DateTime>(() => GetNewestLocalNewsDate());
		}

		public DateTime GetNewestLocalNewsDate()
		{
			using (var db = NewsDataContext.Create())
			{
				db.ObjectTrackingEnabled = false;
				return db.News.OrderByDescending(item => item.PublishDate).Select(news => news.PublishDate).Take(1).FirstOrDefault().GetValueOrDefault(DateTime.MinValue);
			}
		}

		private async Task<XDocument> LoadRssFeedAsync(string url)
		{
			// Downloaden automatisch abbrechen nach 20sek
			CancellationTokenSource cancelSource = new CancellationTokenSource();
			cancelSource.CancelAfter(TimeSpan.FromSeconds(20));

			// RSS-Feed downloaden
			WebClient downloader = new WebClient();
			downloader.Encoding = new LatinEncoding();
			string rss = await downloader.DownloadStringTaskAsync(new Uri(url, UriKind.Absolute), cancelSource.Token);

			// RSS als XDocument parsen und zurückliefern	
			return await Task.Run<XDocument>(() => XDocument.Parse(rss));
		}

		public void MarkAsRead(NewsItem news)
		{
			using (var db = NewsDataContext.Create())
			{
				var loadedNews = db.News.Single(n => n.Id == news.Id);
				loadedNews.IsRead = true;
				db.SubmitChanges();
			}

			news.IsRead = true;
		}

		public void DeleteNewsOlderThan(DateTime timeLimit)
		{
			using (var db = NewsDataContext.Create())
			{
				var newsToDelete = db.News.Where(n => n.PublishDate < timeLimit);
				db.News.DeleteAllOnSubmit(newsToDelete);
				db.SubmitChanges();
			}
		}
	}
}
