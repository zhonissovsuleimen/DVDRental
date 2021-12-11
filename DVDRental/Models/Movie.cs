using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DVDRental.Models
{
    public class Movie
    {
        public int id { get; set; }
        [Required]
        [Column("price")]
        public double price { get; set; }
        [ForeignKey("movieId")]
        public List<Copy> Copies { get; set; }

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
        public int tmdbId { get { return getTMDBId(); } }
        [NotMapped]
        public double rentPrice { get { return getRentPrice(); } }
        [NotMapped]
        public string shortOverview { get { return getShortOverview(); } }
        [NotMapped]
        public string imageLink { get { return getImageLink(); } }

        //Constructors
        public Movie() {}
        public Movie(int tmdbId, double price)
        {
            var movie = getMovieByTMDBId(tmdbId, price);
            if (movie != null)
            {
                this.title = movie.title;
                this.price = price;
                this.year = movie.year;
                this.overview = movie.overview;
            }
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
        private string getImageLink()
        {
            string key = Data.APIKeys.TheMovieDB_api_key;
            string url = $"https://api.themoviedb.org/3/search/movie?api_key={key}&query={title}";
            string imageLink = "https://www.themoviedb.org/t/p/w600_and_h900_bestv2";
            var client = new RestClient($"https://api.themoviedb.org/3/search/movie?api_key={key}&query={title}");
            try
            {
                var request = new RestRequest(Method.GET);
                request.AddHeader("api_key", key);
                IRestResponse response = client.Execute(request);
                var jsonString = response.Content;
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
            }
            catch (Exception)
            {
                return "";
            }
            return "";
            
        }
        private double getRentPrice()
        {
            double divisor = 3;
            return Math.Round(this.price / divisor,2);
        }
        private int getTMDBId()
        {
            string key = Data.APIKeys.TheMovieDB_api_key;
            var client = new RestClient($"https://api.themoviedb.org/3/search/movie?api_key={key}&query={title}");
            try
            {
                var request = new RestRequest(Method.GET);
                request.AddHeader("api_key", key);
                IRestResponse response = client.Execute(request);
                var jsonString = response.Content;
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
            }
            catch (Exception)
            {
                return -1;
            }
            return -1;
        }
        private Movie getMovieByTMDBId(int theMovieDbId, double price)
        {
            string key = Data.APIKeys.TheMovieDB_api_key;
            var client = new RestClient($"https://api.themoviedb.org/3/movie/{theMovieDbId}?api_key={key}");
            var request = new RestRequest(Method.GET);
            request.AddHeader("api_key", key);
            try
            {
                IRestResponse response = client.Execute(request);
                var jsonString = response.Content;
                var parsedObject = JObject.Parse(jsonString);
                if (parsedObject["success"] != null && parsedObject["success"].ToString() == "False") { return null; }
                string title = parsedObject["original_title"].ToString();
                int year = int.Parse(parsedObject["release_date"].ToString().Substring(0, 4));
                string overview = parsedObject["overview"].ToString();
                return new Movie(price, title, year, overview);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
