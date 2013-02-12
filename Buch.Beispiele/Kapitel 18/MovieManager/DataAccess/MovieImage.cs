using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MovieManagerDemo.DataAccess
{
    [Table]
    public class MovieImage
    {
        [Column(IsPrimaryKey = true, AutoSync = AutoSync.OnInsert, DbType = "INT IDENTITY", IsDbGenerated = true)]
        public int MovieImageId { get; set; }

        [Column(CanBeNull=false, DbType="image", UpdateCheck=UpdateCheck.Never)]
        public Binary ImageData { get; set; }

        [Column]
        private int movieId;

        private EntityRef<Movie> movie;

        [Association(Storage="movie", ThisKey="movieId", OtherKey="MovieId", IsForeignKey=true)]
        public Movie Movie
        {
            get { return this.movie.Entity; }
            set
            {
                this.movie.Entity = value;
                if (value != null)
                {
                    this.movieId = value.MovieId;
                }
            }
        }

        public ImageSource MovieImageSource
        {
            get
            {
                return this.GetImageSource(this.ImageData.ToArray());
            }
        }

        private ImageSource GetImageSource(byte[] imageBytes)
        {
            using (var ms = new MemoryStream(imageBytes))
            {
                var bi = new BitmapImage();
                bi.SetSource(ms);
                return bi;
            }
        }
    }
}
