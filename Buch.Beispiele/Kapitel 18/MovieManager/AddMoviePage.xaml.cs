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
    public partial class AddMoviePage : PhoneApplicationPage
    {
        public AddMoviePage()
        {
            InitializeComponent();
        }

        private void AddMovie_Click(object sender, EventArgs e)
        {
            this.InsertMovie(this.Title.Text, this.Subtitle.Text, this.Genre.Text);
            this.NavigationService.GoBack();
        }

        private void InsertMovie(string title, string subtitle, string genre)
        {
            // Film-Objekt erzeugen
            Movie movie = new Movie();
            movie.Title = title;
            movie.Subtitle = subtitle;
            movie.Genre = genre;
            movie.Summary = string.Empty;
            movie.AddedDate = DateTime.Now;

            // Film hinzufügen und speichern
            App.DB.Movies.InsertOnSubmit(movie);
            App.DB.SubmitChanges();
        }
    }
}