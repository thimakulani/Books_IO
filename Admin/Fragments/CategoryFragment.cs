using Admin.Adapters;
using Admin.Models;
using Android.Content;
using Android.OS;
using Android.Views;
using AndroidHUD;
using AndroidX.Fragment.App;
using AndroidX.RecyclerView.Widget;
using Firebase.Auth;
using FirebaseAdmin.Messaging;
using Google.Android.Material.Button;
using Google.Android.Material.TextField;
using Google.Android.Material.TextView;
using Plugin.CloudFirestore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Admin.Fragments
{
    public class CategoryFragment : Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.category_fragment, container, false);
            ConnectViews(view);
            return view;
        }
        //private List<Books> Items = new List<Books>();
        private List<DistinctBooks> BooksItems = new List<DistinctBooks>();
        private async void ConnectViews(View view)
        {
            RecyclerView recycler_categories = view.FindViewById<RecyclerView>(Resource.Id.recycler_categories);

            var query = await CrossCloudFirestore.Current
                .Instance
                .Collection("BooksListings")
                .WhereEqualsTo("Status", "Approved")
                .GetAsync();
           
            foreach (var item in query.Documents)
            {
                
                var books = item.ToObject<Books>();
                
                if(BooksItems.Any(x=>x.ISBN == books.ISBN))
                {
                    var pos = BooksItems.FindIndex(x => x.ISBN == books.ISBN);
                    if (pos >= 0)
                    {
                        BooksItems[pos].Counter = BooksItems[pos].Counter + 1;
                    }
                }
                else
                {
                    BooksItems.Add(new DistinctBooks { ISBN = books.ISBN, Counter = 1, Title = books.Title });
                }

            }
            CatergotyAdapter adapter = new CatergotyAdapter(BooksItems);
            recycler_categories.SetAdapter(adapter);
            recycler_categories.SetLayoutManager(new LinearLayoutManager(view.Context));
            
            /*
             var groups = userInfoList
            .GroupBy(n => n.metric)
            .Select(n => new
            {
                MetricName = n.Key,
                MetricCount = n.Count()
            }
            )
            .OrderBy(n => n.MetricName);
             */

        }
    }
}