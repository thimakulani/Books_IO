using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace Books_IO.Models
{
    class ISBNSearch
    {
        public ISBNSearch()
        {
           
        }
        public async void GetIsbn()
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://isbnpool.p.rapidapi.com/book/?apikey=%7BAPIKEY%7D&isbn=%3CREQUIRED%3E"),
                Headers =
            {
                { "x-rapidapi-key", "a8f1121782msh47677863eeab683p19d625jsn1fa9d9f59efc" },
                { "x-rapidapi-host", "isbnpool.p.rapidapi.com" },
            },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                Console.WriteLine("weeeeee!!!!" + body);
            }
        }
    }
}