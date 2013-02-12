using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using Microsoft.Phone.Data.Linq.Mapping;

namespace RssReader.DataAccess
{
	[Table]
	[Index(Name = "IDX_PublishDate", Columns = "PublishDate")]
	public class NewsItem : INotifyPropertyChanged, INotifyPropertyChanging
	{
		private int _id;

		[Column(CanBeNull = false, DbType = "INT NOT NULL IDENTITY", IsDbGenerated = true, IsPrimaryKey = true, Storage = "_id", UpdateCheck = UpdateCheck.Never,
			AutoSync = AutoSync.OnInsert)]
		public int Id
		{
			get { return _id; }
			set
			{
				if (value != _id)
				{
					SendPropertyChanging("Id");
					_id = value;
					SendPropertyChanged("Id");
				}
			}
		}

		private string _title;

		[Column(CanBeNull = false, DbType = "NVARCHAR(200) NOT NULL", Storage = "_title", UpdateCheck = UpdateCheck.Never)]
		public string Title
		{
			get { return _title; }
			set
			{
				if (_title != value)
				{
					SendPropertyChanging("Title");
					_title = value;
					SendPropertyChanged("Title");
				}
			}
		}

		private string _preview;

		[Column(CanBeNull = true, DbType = "NVARCHAR(500)", Storage = "_preview", UpdateCheck = UpdateCheck.Never)]
		public string Preview
		{
			get { return _preview; }
			set
			{
				if (_preview != value)
				{
					SendPropertyChanging("Preview");
					_preview = value;
					SendPropertyChanged("Preview");
				}
			}
		}

		private string _link;

		[Column(CanBeNull = false, DbType = "NVARCHAR(200) NOT NULL", Storage = "_link", UpdateCheck = UpdateCheck.Never)]
		public string Link
		{
			get { return _link; }
			set
			{
				if (_link != value)
				{
					SendPropertyChanging("Link");
					_link = value;
					SendPropertyChanged("Link");
				}
			}
		}

		private DateTime? _publishDate;

		[Column(CanBeNull = true, DbType = "DATETIME NULL", Storage = "_publishDate", UpdateCheck = UpdateCheck.Never)]
		public DateTime? PublishDate
		{
			get { return _publishDate; }
			set
			{
				if (_publishDate != value)
				{
					SendPropertyChanging("PublishDate");
					_publishDate = value;
					SendPropertyChanged("PublishDate");
				}
			}
		}

		private string _thumbnail;

		[Column(CanBeNull = true, DbType = "NVARCHAR(200)", Storage = "_thumbnail", UpdateCheck = UpdateCheck.Never)]
		public string Thumbnail
		{
			get { return _thumbnail; }
			set
			{
				if (_thumbnail != value)
				{
					SendPropertyChanging("Thumbnail");
					_thumbnail = value;
					SendPropertyChanged("Thumbnail");
				}
			}
		}

		private bool _isRead;

		[Column(CanBeNull = false, DbType = "BIT NOT NULL DEFAULT 0", Storage = "_isRead", UpdateCheck = UpdateCheck.Never)]
		public bool IsRead
		{
			get { return _isRead; }
			set
			{
				if (_isRead != value)
				{
					SendPropertyChanging("IsRead");
					_isRead = value;
					SendPropertyChanged("IsRead");
				}
			}
		}

		[Column(IsVersion = true)]
		protected Binary Version;

		public event PropertyChangedEventHandler PropertyChanged;

		protected void SendPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler handler = PropertyChanged;
			if (handler != null)
			{
				handler(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		public event PropertyChangingEventHandler PropertyChanging;

		protected void SendPropertyChanging(string propertyName)
		{
			PropertyChangingEventHandler handler = PropertyChanging;
			if (handler != null)
			{
				handler(this, new PropertyChangingEventArgs(propertyName));
			}
		}
	}
}
