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
using System.Data.Linq;

namespace MovieManagerDemo.DataAccess
{
    public class MoviesDataContext : DataContext
    {
        private const string ConnectionString = "Data Source=isostore:/MovieDB.sdf";

        public MoviesDataContext() : base(ConnectionString)
        {
        }
        
        public Table<Movie> Movies;
        public Table<MovieImage> MovieImages;
    }
}