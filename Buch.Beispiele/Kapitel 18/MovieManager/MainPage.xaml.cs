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

namespace MovieManagerDemo
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            this.LoadMovies();
        }

        private void MoviesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Movie movie = this.MoviesList.SelectedItem as Movie;
            if (movie != null)
            {
                string detailsUri = "/MovieDetailsPage.xaml?id=" + movie.MovieId;
                this.NavigationService.Navigate(new Uri(detailsUri, UriKind.Relative));
            }
        }

        private void LoadMovies()
        {
            List<Movie> movies = (from m in App.DB.Movies
                                  orderby m.AddedDate descending
                                  select m).ToList();
            // Alternative:
            // List<Movie> movies = App.DB.Movies.OrderByDescending(m => m.AddedDate).ToList();;

            // Datenquelle für DataBinding setzen
            this.DataContext = movies;
        }

        private void AddMovie_Click(object sender, EventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/AddMoviePage.xaml", UriKind.Relative));
        }
    }
}