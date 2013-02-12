using System.Data.Linq;

namespace RssReader.DataAccess
{
	internal class NewsDataContext : DataContext
	{
		private const string ConnectionString = "Data Source='isostore:/NewsDB.sdf'";

		private NewsDataContext() : base(ConnectionString) {}
		
		public Table<NewsItem> News;

		public static NewsDataContext Create()
		{
			NewsDataContext db = new NewsDataContext();
			if (!db.DatabaseExists())
			{
				db.CreateDatabase();
			}

			return db;
		}
	}
}
