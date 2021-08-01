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
    public class Student
    {
        [Id]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string ImageUrl { get; set; }
    }
}