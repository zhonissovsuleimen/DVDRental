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
        [Required]
        [Column("title")]
        public string title { get; set; }
        [Required]
        [Column("year")]
        public int year { get; set; }
        [Column("overview")]
        public string overview { get; set; }

        //NotMapped properties
        [NotMapped]
        public double rentPrice { get { return getRentPrice(); } }
        [NotMapped]
        public string shortOverview { get { return getShortOverview(); } }
        [NotMapped]
        public string imageLink { get { return getImageLink().Result; } }

        //Methods
        private string getShortOverview(int length = 250)
        {
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
        private double getRentPrice(int price = -1)
        {
            double divisor = 3;
            if (price < 0 || price > this.price)
            {
                return this.price / divisor;
            }
            return price;
        }
    }
}
