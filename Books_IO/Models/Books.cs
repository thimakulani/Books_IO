using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Plugin.CloudFirestore.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Books_IO.Models
{
    public class Books
    {
        public string Title { get; set; }
        public string Edition { get; set; }
        public string ISBN { get; set; }
        public string Author { get; set; }
        [Id]
        public string Id { get; set; }
    }
}