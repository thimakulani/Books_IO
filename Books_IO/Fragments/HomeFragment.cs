using Android.OS;
using Android.Views;
using AndroidX.Fragment.App;
using AndroidX.RecyclerView.Widget;
using AndroidX.ViewPager.Widget;
using Books_IO.Adapters;
using Books_IO.Dialogs;
using Books_IO.Models;
using Google.Android.Material.FloatingActionButton;
using Java.Util;
using Plugin.CloudFirestore;
using System;
using System.Collections.Generic;

namespace Books_IO.Fragments
{
    public class HomeFragment : Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.home_fragment, container, false);
            ConnectViews(view);
            return view;
        }
        private ExtendedFloatingActionButton fab_add_book;
        private readonly List<Books> Items = new List<Books>();
        private void ConnectViews(View view)
        {
            fab_add_book = view.FindViewById<ExtendedFloatingActionButton>(Resource.Id.fab_add_book);
            RecyclerView recycler_books_list = view.FindViewById<RecyclerView>(Resource.Id.recycler_books_list);

            LinearLayoutManager linearLayoutManager = new LinearLayoutManager(view.Context);
            ListingAdapter adapter = new ListingAdapter(Items);
            recycler_books_list.SetLayoutManager(linearLayoutManager);
            recycler_books_list.SetAdapter(adapter);
            adapter.ItemClick += Adapter_ItemClick;
            CrossCloudFirestore
                .Current
                .Instance
                .Collection("BooksListings")
                .WhereEqualsTo("Status", "Approved")
                .AddSnapshotListener((values, error) =>
                {
                    if (!values.IsEmpty)
                    {
                        foreach (var data in values.DocumentChanges)
                        {
                            switch (data.Type)
                            {
                                case DocumentChangeType.Added:
                                    Items.Add(data.Document.ToObject<Books>());
                                    adapter.NotifyDataSetChanged();
                                    break;
                                case DocumentChangeType.Modified:
                                    Items[data.OldIndex] = data.Document.ToObject<Books>();
                                    adapter.NotifyDataSetChanged();
                                    break;
                                case DocumentChangeType.Removed:
                                    Items.RemoveAt(data.OldIndex);
                                    adapter.NotifyDataSetChanged();
                                    break;
                                default:
                                    break;
                            }
                        }
                    }

                });

            fab_add_book.Click += Fab_add_book_Click;
        }

        private void Adapter_ItemClick(object sender, ListingAdapterClickEventArgs e)
        {
            ViewBookDlgFragment dlg = new ViewBookDlgFragment(Items[e.Position].Id);
            dlg.Show(ChildFragmentManager.BeginTransaction(), null);
        }

        private void Fab_add_book_Click(object sender, EventArgs e)
        {
            AddBookToListing dlg = new AddBookToListing();
            dlg.Show(ChildFragmentManager.BeginTransaction(), "");
        }
    }
 
}