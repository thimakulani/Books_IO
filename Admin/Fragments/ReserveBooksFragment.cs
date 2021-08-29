using Android.OS;
using Android.Views;
using AndroidX.Fragment.App;
using AndroidX.RecyclerView.Widget;
using System.Collections.Generic;

namespace Admin.Fragments
{
    public class ReserveBooksFragment : Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.reserve_fragment, container, false);
            ConnectViews(view);
            return view;
        }
        private readonly List<string> items = new List<string>();
        private void ConnectViews(View view)
        {
            RecyclerView recycler = view.FindViewById<RecyclerView>(Resource.Id.recycler_reserve);
            recycler.SetLayoutManager(new LinearLayoutManager(view.Context));
            ReserveAdapter adapter = new ReserveAdapter(items);
            recycler.SetAdapter(adapter);
            adapter.NotifyDataSetChanged();
            adapter.BtnClick += Adapter_BtnClick;
            CrossCloudFirestore
                .Current
                .Instance
                .Collection("Reserved")
                .Document(FirebaseAuth.Instance.Uid)
                .Collection("Books")
                .AddSnapshotListener((values, error) =>
                {

                    if (!values.IsEmpty)
                    {
                        foreach (var dc in values.DocumentChanges)
                        {
                            switch (dc.Type)
                            {
                                case DocumentChangeType.Added:
                                    items.Add(dc.Document.Get<string>("Book_Id"));
                                    //Toast.MakeText(view.Context, dc.Document.Get<string>("Book_Id"), ToastLength.Long).Show();
                                    adapter.NotifyDataSetChanged();
                                    break;
                                case DocumentChangeType.Modified:
                                    break;
                                case DocumentChangeType.Removed:
                                    items.RemoveAt(dc.OldIndex);
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