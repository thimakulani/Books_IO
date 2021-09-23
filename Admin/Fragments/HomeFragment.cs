using Admin.Adapters;
using Admin.Dialogs;
using Admin.Models;
using Android.OS;
using Android.Views;
using AndroidX.Fragment.App;
using AndroidX.RecyclerView.Widget;
using Google.Android.Material.FloatingActionButton;
using Plugin.CloudFirestore;
using System;
using System.Collections.Generic;

namespace Admin.Fragments
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
        private readonly List<Books> Items = new List<Books>();
        private void ConnectViews(View view)
        {
            RecyclerView recycler_books_list = view.FindViewById<RecyclerView>(Resource.Id.recycler_books_list);

            LinearLayoutManager linearLayoutManager = new LinearLayoutManager(view.Context);
            ListingAdapter adapter = new ListingAdapter(Items);
            recycler_books_list.SetLayoutManager(linearLayoutManager);
            recycler_books_list.SetAdapter(adapter);
            adapter.NotifyDataSetChanged();
            adapter.ItemClick += Adapter_ItemClick;








            CrossCloudFirestore
                .Current
                .Instance
                .Collection("BooksListings")
                .WhereEqualsTo("Status", "Pending")
                .WhereEqualsTo("FileType", "IMG")
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
                                    adapter.NotifyItemChanged(data.OldIndex);
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

        }

        private void Adapter_ItemClick(object sender, ListingAdapterClickEventArgs e)
        {
            ViewBookDlgFragment dlg = new ViewBookDlgFragment(Items[e.Position].Id);
            dlg.Show(ChildFragmentManager.BeginTransaction(), null);
        }


    }

}