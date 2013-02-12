using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Data.Linq.Mapping;
using System.Data.Linq;
using System.ComponentModel;

namespace MovieManagerDemo.DataAccess
{
    [Table]
    public class Movie : INotifyPropertyChanging, INotifyPropertyChanged
    {
        private EntitySet<MovieImage> images;

        [Association(Storage="images", ThisKey="MovieId", OtherKey="movieId")]
        public EntitySet<MovieImage> Images
        {
            get { return this.images; }
            set { this.images.Assign(value); }
        }

        public Movie()
        {
            this.images = new EntitySet<MovieImage>(
                new Action<MovieImage>(Add_MovieImage),
                new Action<MovieImage>(Detach_MovieImage));
        }

        private void Add_MovieImage(MovieImage image)
        {
            image.Movie = this;
        }

        private void Detach_MovieImage(MovieImage image)
        {
            image.Movie = null;
        }

        private int movieId;
        // Interne ID
        [Column(Storage= "movieId", IsPrimaryKey = true, AutoSync = AutoSync.OnInsert, DbType = "INT IDENTITY", IsDbGenerated = true)]
        public int MovieId
        {
            get { return this.movieId; }
            set
            {
                if (this.movieId != value)
                {
                    this.SendPropertyChanging("MovieId");
                    this.movieId = value;
                    this.SendPropertyChanged("MovieId");
                }
            }
        }

        private string title;
        // Titel
        [Column(Storage = "title", CanBeNull = false, DbType = "nvarchar(250)")]
        public string Title
        {
            get { return this.title; }
            set
            {
                if (this.title != value)
                {
                    this.SendPropertyChanging("Title");
                    this.title = value;
                    this.SendPropertyChanged("Title");
                }
            }
        }

        private string subtitle;
        // Untertitel
        [Column(Storage = "subtitle", DbType = "nvarchar(250)")]
        public string Subtitle
        {
            get { return this.subtitle; }
            set
            {
                if (this.subtitle != value)
                {
                    this.SendPropertyChanging("Subtitle");
                    this.subtitle = value;
                    this.SendPropertyChanged("Subtitle");
                }
            }
        }

        private string genre;
        // Genre
        [Column(Storage = "genre", DbType = "nvarchar(50)")]
        public string Genre
        {
            get { return this.genre; }
            set
            {
                if (this.genre != value)
                {
                    this.SendPropertyChanging("Genre");
                    this.genre = value;
                    this.SendPropertyChanged("Genre");
                }
            }
        }

        private string summary;
        // Zusammenfassung
        [Column(Storage = "summary", CanBeNull = true, DbType = "ntext", UpdateCheck = UpdateCheck.Never)]
        public string Summary
        {
            get { return this.summary; }
            set
            {
                if (this.summary != value)
                {
                    this.SendPropertyChanging("Summary");
                    this.summary = value;
                    this.SendPropertyChanged("Summary");
                }
            }
        }

        private DateTime addedDate;
        // Zur Sammlung hinzugefügt am
        [Column(Storage = "addedDate", CanBeNull = false)]
        public DateTime AddedDate
        {
            get { return this.addedDate; }
            set
            {
                if (this.addedDate != value)
                {
                    this.SendPropertyChanging("AddedDate");
                    this.addedDate = value;
                    this.SendPropertyChanged("AddedDate");
                }
            }
        }

        private DateTime? publishedDate;
        [Column(Storage = "publishedDate", CanBeNull = true)]
        public DateTime? PublishedDate
        {
            get { return this.publishedDate; }
            set
            {
                if (this.publishedDate != value)
                {
                    this.SendPropertyChanging("PublishedDate");
                    this.publishedDate = value;
                    this.SendPropertyChanged("PublishedDate");
                }
            }
        }

        [Column(IsVersion = true)]
        private Binary version;

        public event PropertyChangingEventHandler PropertyChanging;

        protected void SendPropertyChanging(string propertyName)
        {
            PropertyChangingEventHandler handler = this.PropertyChanging;
            if (handler != null)
            {
                handler(this, new PropertyChangingEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void SendPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}