using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace DVDRental.Models
{
    public class Movie
    {
        public int id { get; set; }
        [Required]
        [Column("price")]
        public double price { get; set; }

        //Movie info properties
        //Placing [Required] here gave me an error when creating Movie via TMDB id, so I assumed we can live without it.
        [Column("title")]
        public string title { get; set; }
        [Required]
        [Column("year")]
        public int year { get; set; }
        [Column("overview")]
        public string overview { get; set; }

        //NotMapped properties
        [NotMapped]
        public int tmdbId { get { return getTMDBId().Result; } }
        [NotMapped]
        public double rentPrice { get { return getRentPrice(); } }
        [NotMapped]
        public string shortOverview { get { return getShortOverview(); } }
        [NotMapped]
        public string imageLink { get { return getImageLink().Result; } }

        //Constructors
        public Movie() {}
        public Movie(int tmdbId, double price)
        {
            var movie = getMovieByTMDBId(tmdbId, price).Result;
            if (movie != null)
            {
                this.title = movie.title;
                this.price = price;
                this.year = movie.year;
                this.overview = movie.overview;
            }
        }
        public Movie(int id, string title, int year, string overview)
        {
            this.id = id;
            this.title = title;
            this.year = year;
            this.overview = overview;
        }
        public Movie(int id, double price, string title, int year, string overview)
        {
            this.id = id;
            this.price = price;
            this.title = title;
            this.year = year;
            this.overview = overview;
        }
        public Movie(double price, string title, int year, string overview)
        {
            this.title = title;
            this.year = year;
            this.overview = overview;
        }

        //Methods
        private string getShortOverview(int length = 250)
        {
            if(overview == null) { return null; }
            string dots = overview.Length < length ? "" : "...";
            length = Math.Min(overview.Length, length);
            return overview.Substring(0, length) + dots;
        }
        private async Task<string> getImageLink()
        {
            using (var httpClient = new HttpClient())
            {
                string key = Data.APIKeys.TheMovieDB_api_key;
                string url = $"https://api.themoviedb.org/3/search/movie?api_key={key}&query={title}";
                string imageLink = "https://www.themoviedb.org/t/p/w600_and_h900_bestv2";
                var task = await httpClient.GetAsync(url).ConfigureAwait(false);
                if (task.IsSuccessStatusCode)
                {
                    var content = task.Content.ReadAsStringAsync();
                    var jsonString = content.Result;
                    var parsedObject = JObject.Parse(jsonString);
                    var linqList = parsedObject.SelectToken("results").ToArray();
                    foreach (var linq in linqList)
                    {
                        JObject obj = JObject.Parse(linq.ToString());
                        if (obj["title"].ToString() == title && obj["release_date"].ToString().Substring(0, 4) == year.ToString())
                        {
                            imageLink += obj["poster_path"].ToString();
                            return imageLink;
                        }
                    }
                    return "";
                }
                return "";
            }
        }
        private double getRentPrice()
        {
            double divisor = 3;
            return this.price / divisor;
        }
        private async Task<int> getTMDBId()
        {
            using (var httpClient = new HttpClient())
            {
                string key = Data.APIKeys.TheMovieDB_api_key;
                string url = $"https://api.themoviedb.org/3/search/movie?api_key={key}&query={title}";
                var task = await httpClient.GetAsync(url).ConfigureAwait(false);
                if (task.IsSuccessStatusCode)
                {
                    var content = task.Content.ReadAsStringAsync();
                    var jsonString = content.Result;
                    var parsedObject = JObject.Parse(jsonString);
                    var linqList = parsedObject.SelectToken("results").ToArray();
                    foreach (var linq in linqList)
                    {
                        JObject obj = JObject.Parse(linq.ToString());
                        if (obj["title"].ToString() == title && obj["release_date"].ToString().Substring(0, 4) == year.ToString())
                        {
                            return int.Parse(obj["id"].ToString());
                        }
                    }
                    return -1;
                }
                return -1;
            }
        }
        private async Task<Movie> getMovieByTMDBId(int theMovieDbId, double price)
        {
            using (var httpClient = new HttpClient())
            {
                string key = Data.APIKeys.TheMovieDB_api_key;
                string url = $"https://api.themoviedb.org/3/movie/{theMovieDbId}?api_key={key}";
                var task = await httpClient.GetAsync(url).ConfigureAwait(false);
                if (task.IsSuccessStatusCode)
                {
                    var content = task.Content.ReadAsStringAsync();
                    var jsonString = content.Result;
                    var parsedObject = JObject.Parse(jsonString);
                    string title = parsedObject["original_title"].ToString();
                    int year = int.Parse(parsedObject["release_date"].ToString().Substring(0, 4));
                    string overview = parsedObject["overview"].ToString();
                    return new Movie(price, title, year, overview);
                }
                return null;
            }
        }
    }
}
