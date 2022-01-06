using Admin.Adapters;
using Admin.Dialogs;
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
        private Context context;
        private readonly List<string> items = new List<string>();
        private MaterialButton BtnSearchStudentId;
        private TextInputEditText InputStudentNo;
        private RecyclerView recycler;
        private MaterialTextView txt_total_count;
        private void ConnectViews(View view)
        {
            context = view.Context;
            txt_total_count = view.FindViewById<MaterialTextView>(Resource.Id.txt_total_count);
            recycler = view.FindViewById<RecyclerView>(Resource.Id.recycler_reserve);
            BtnSearchStudentId = view.FindViewById<MaterialButton>(Resource.Id.btn_search_student_no);
            InputStudentNo = view.FindViewById<TextInputEditText>(Resource.Id.InputStudentNo);
            recycler.SetLayoutManager(new LinearLayoutManager(view.Context));
            txt_total_count.Text = $"TOTAL BOOKS: {0}";
            //ReserveAdapter adapter;//= new ReserveAdapter(items);
            //recycler.SetAdapter(adapter);
            //adapter.NotifyDataSetChanged();
            // adapter.BtnClick += Adapter_BtnClick;
            BtnSearchStudentId.Click += BtnSearchStudentId_Click;


            //CrossCloudFirestore
            //    .Current
            //    .Instance
            //    .Collection("Reserved")
            //    .Document(FieldPath.DocumentId.ToString())
            //    .Collection("Books")
            //    .AddSnapshotListener((values, error) =>
            //    {

            //        if (!values.IsEmpty)
            //        {
            //            foreach (var dc in values.DocumentChanges)
            //            {
            //                switch (dc.Type)
            //                {
            //                    case DocumentChangeType.Added:
            //                        items.Add(dc.Document.Get<string>("Book_Id"));
            //                        //Toast.MakeText(view.Context, dc.Document.Get<string>("Book_Id"), ToastLength.Long).Show();
            //                        adapter.NotifyDataSetChanged();
            //                        break;
            //                    case DocumentChangeType.Modified:
            //                        break;
            //                    case DocumentChangeType.Removed:
            //                        items.RemoveAt(dc.OldIndex);
            //                        adapter.NotifyDataSetChanged();
            //                        break;
            //                    default:
            //                        break;
            //                }
            //            }
            //        }
            //    });
        }
        private string student_id = null;
        private int counter = 0;
        private async void BtnSearchStudentId_Click(object sender, System.EventArgs e)
        {
            ScanDlgFragment scanDlgFragment = new ScanDlgFragment();
            scanDlgFragment.Show(ChildFragmentManager.BeginTransaction(), "");
            if (!string.IsNullOrEmpty(InputStudentNo.Text))
            {
                var query = await CrossCloudFirestore.Current
                    .Instance
                    .Collection("Students")
                    .Document(InputStudentNo.Text.Trim()).GetAsync();

                if (query.Exists)
                {
                    
                    Student student = new Student();
                    student = query.ToObject<Student>();
                    student_id = student.Id;
                    var data = await CrossCloudFirestore
                        .Current
                        .Instance
                        .Collection("Reserved")
                        .Document(student.Id)
                        .Collection("Books")
                        .GetAsync();
                    counter = 0;
                    if (!data.IsEmpty)
                    {
                        
                        foreach (var item in data.Documents)
                        {
                            items.Add(item.Id);
                            counter++;
                        }
                        
                    }
                    else
                    {
                        AndHUD.Shared.ShowError(context, "Book not found in reservation", MaskType.Clear, TimeSpan.FromSeconds(3));
                    }
                    txt_total_count.Text = $"TOTAL BOOKS: {counter}";
                    ReserveAdapter adapter = new ReserveAdapter(items);
                    recycler.SetAdapter(adapter);
                    adapter.NotifyDataSetChanged();
                    adapter.BtnClick += Adapter_BtnClick;
                }
            }
        }

        private async void Adapter_BtnClick(object sender, ReserveAdapterClickEventArgs e)
        {
            Dictionary<string, object> data = new Dictionary<string, object>
            {
                { "Status", "Sold" }
            };
            await CrossCloudFirestore
                .Current
                .Instance
                .Collection("BooksListings")
                .Document(items[e.Position])
                .UpdateAsync(data);

            var stream = Resources.Assets.Open("ServiceAccount.json");
            var fcm = FirebaseHelper.FirebaseAdminSDK.GetFirebaseMessaging(stream);
            FirebaseAdmin.Messaging.Message message = new FirebaseAdmin.Messaging.Message()
            {
                Topic = student_id,
                Notification = new Notification()
                {
                    Title = "Sold",
                    Body = $"Your book has been successfully sold please come collect your parcel.",

                },

            };
            await fcm.SendAsync(message);
            AndHUD.Shared.ShowSuccess(context, "Successfully sold", MaskType.Clear, TimeSpan.FromSeconds(3));
        }
    }
}