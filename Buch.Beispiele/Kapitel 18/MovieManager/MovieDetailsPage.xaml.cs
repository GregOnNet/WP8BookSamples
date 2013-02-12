using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using MovieManagerDemo.DataAccess;
using Microsoft.Phone.Tasks;
using System.IO;
using System.Data.Linq;

namespace MovieManagerDemo
{
    public partial class MovieDetailsPage : PhoneApplicationPage
    {
        private int movieId;
        private PhotoChooserTask photoTask;
        private Stream photoStream;
        private Movie selectedMovie;

        public MovieDetailsPage()
        {
            InitializeComponent();

            photoTask = new PhotoChooserTask();
            photoTask.Completed += new EventHandler<PhotoResult>(photoTask_Completed);
        }

        void photoTask_Completed(object sender, PhotoResult e)
        {
            this.photoStream = e.ChosenPhoto;
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (this.NavigationContext.QueryString.ContainsKey("id"))
            {
                movieId = int.Parse(this.NavigationContext.QueryString["id"]);
                this.selectedMovie = GetMovie(movieId);

                // Wenn Film gefunden wurde anzeigen
                if (this.selectedMovie != null)
                {
                    this.DisplayMovie(this.selectedMovie);

                    if (this.photoStream != null)
                    {
                        // Bild speichern
                        this.StoreMovieImage(this.photoStream);
                        this.photoStream = null;
                    }

                    this.MovieImages.ItemsSource = this.selectedMovie.Images;
                }
            }
        }

        private void DisplayMovie(Movie movie)
        {
            this.Title.Text = movie.Title;
            this.SubTitle.Text = movie.Subtitle;
            this.Genre.Text = movie.Genre;
            this.Summary.Text = movie.Summary;
        }

        private void StoreMovieImage(Stream movieImageStream)
        {
            byte[] photoBytes = new byte[movieImageStream.Length];
            movieImageStream.Read(photoBytes, 0, photoBytes.Length);

            MovieImage image = new MovieImage();
            image.ImageData = new Binary(photoBytes);

            this.selectedMovie.Images.Add(image);
            App.DB.SubmitChanges();
        }

        private void SaveMovie_Click(object sender, EventArgs e)
        {
            this.UpdateMovie(this.Title.Text, this.SubTitle.Text, this.Genre.Text, this.Summary.Text);
        }

        private void DeleteMovie_Click(object sender, EventArgs e)
        {
            this.DeleteMovie(this.movieId);
            this.NavigationService.GoBack();
        }

        // Datenbankzugriffe
        private Movie GetMovie(int id)
        {
            return App.DB.Movies.SingleOrDefault(m => m.MovieId == id);
        }

        private void UpdateMovie(string title, string subtitle, string genre, string summary)
        {
            // Film laden
            Movie movie = App.DB.Movies.Single(m => m.MovieId == this.movieId);
            
            // Daten ändern
            movie.Title = title;
            movie.Subtitle = subtitle;
            movie.Genre = genre;
            movie.Summary = summary;

            // Änderungen zurückspeichern
            App.DB.SubmitChanges();
        }

        private void DeleteMovie(int id)
        {
            // Film laden
            Movie movie = App.DB.Movies.Single(m => m.MovieId == id);

            // Film löschen und speichern
            App.DB.Movies.DeleteOnSubmit(movie);
            App.DB.SubmitChanges();
        }

        private void AddPicture_Click(object sender, EventArgs e)
        {
            this.photoTask.PixelHeight = 400;
            this.photoTask.PixelWidth = 400;
            this.photoTask.ShowCamera = true;
            this.photoTask.Show();
        }
    }
}