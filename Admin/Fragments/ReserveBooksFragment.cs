using Admin.Adapters;
using Admin.Dialogs;
using Admin.Models;
using Android.Content;
using Android.Hardware;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidHUD;
using AndroidX.AppCompat.Widget;
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
using ZXing.Mobile;
using ZXing.Mobile.CameraAccess;

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
        private AppCompatImageView ImgSearchStudentId, ImgScan;
        private TextInputEditText InputStudentNo;
        private RecyclerView recycler;
        private MaterialTextView txt_total_count;


        private readonly Context _context;
        private readonly ISurfaceHolder _holder;
        private readonly SurfaceView _surfaceView;
        private readonly CameraEventsListener _cameraEventListener;
        private int _cameraId;
        IScannerSessionHost _scannerHost;

        private string student_id = null;
        private int counter = 0;
        private void ConnectViews(View view)
        {
            context = view.Context;
            txt_total_count = view.FindViewById<MaterialTextView>(Resource.Id.txt_total_count);
            recycler = view.FindViewById<RecyclerView>(Resource.Id.recycler_reserve);
            ImgSearchStudentId = view.FindViewById<AppCompatImageView>(Resource.Id.btn_search_student_no);
            ImgScan = view.FindViewById<AppCompatImageView>(Resource.Id.img_scanner);
            InputStudentNo = view.FindViewById<TextInputEditText>(Resource.Id.InputStudentNo);
            recycler.SetLayoutManager(new LinearLayoutManager(view.Context));
            txt_total_count.Text = $"TOTAL BOOKS: {0}";
           

            ImgSearchStudentId.Click += ImgSearchStudentId_Click;
            ImgScan.Click += ImgScan_Click;

        }

        private async void ImgScan_Click(object sender, EventArgs e)
        {
            var MScanner = new MobileBarcodeScanner();
            var Result = await MScanner.Scan();
            if (Result == null)
            {
                return;
            }
            //get the bar code text here 
            string BarcodeText = Result.Text;

            Toast.MakeText(context, Result.Text, ToastLength.Long).Show();
        }

        private async void ImgSearchStudentId_Click(object sender, System.EventArgs e)
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