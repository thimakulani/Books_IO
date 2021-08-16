using Android.OS;
using Android.Views;
using AndroidX.Fragment.App;
using AndroidX.RecyclerView.Widget;
using Books_IO.Adapters;
using Books_IO.Models;
using Firebase.Auth;
using Plugin.CloudFirestore;
using System;
using System.Collections.Generic;

namespace Books_IO.Fragments
{
    public class SellingFragment : Fragment
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
            View view = inflater.Inflate(Resource.Layout.selling_fragment, container, false);
            ConnectViews(view);
            return view;
        }
        private readonly List<Books> Items = new List<Books>();
        private void ConnectViews(View view)
        {
            RecyclerView recycler_books_list = view.FindViewById<RecyclerView>(Resource.Id.recycler_selling);

            LinearLayoutManager linearLayoutManager = new LinearLayoutManager(view.Context);
            SellingAdapter adapter = new SellingAdapter(Items);
            recycler_books_list.SetLayoutManager(linearLayoutManager);
            recycler_books_list.SetAdapter(adapter); 
            
            CrossCloudFirestore
                .Current
                .Instance
                .Collection("BooksListings")
                .WhereEqualsTo("Student_Id", FirebaseAuth.Instance.Uid)
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
        }
    }
}