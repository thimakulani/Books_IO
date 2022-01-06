using Android.App;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using AndroidX.AppCompat.App;
using AndroidX.AppCompat.Widget;
using Books_IO.Fragments;
using Books_IO.Models;
using Firebase.Auth;
using Firebase.Messaging;
using Google.Android.Material.AppBar;
using Google.Android.Material.BottomNavigation;
using IsmaelDiVita.ChipNavigationLib;
using Plugin.CloudFirestore;
using System;
using System.IO;
using ZXing;
using ZXing.Common;
using static Android.App.ActionBar;
using AlertDialog = Android.App.AlertDialog;

namespace Books_IO
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = false)]
    public class MainActivity : AppCompatActivity, ChipNavigationBar.IOnItemSelectedListener
    {
        private ChipNavigationBar bottom_nav;
        private MaterialToolbar toolbar;
        private AppCompatImageView qr_code;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            if (savedInstanceState == null)
            {
                SupportFragmentManager
                    .BeginTransaction()
                    .Add(Resource.Id.fragment_host, new HomeFragment())
                    .Commit();
            }

            toolbar = FindViewById<MaterialToolbar>(Resource.Id.toolbar);
            bottom_nav = FindViewById<ChipNavigationBar>(Resource.Id.bottom_nav);
            qr_code = FindViewById<AppCompatImageView>(Resource.Id.img_unique_qr_code);

            bottom_nav.SetMenuResource(Resource.Menu.nav_menu);
            bottom_nav.SetItemSelected(Resource.Id.nav_home);
            bottom_nav.SetOnItemSelectedListener(this);
            
            CrossCloudFirestore
               .Current
               .Instance
               .Collection("Students")
               .Document(FirebaseAuth.Instance.Uid)
               .AddSnapshotListener((values, errors) =>
               {
                   if (values.Exists)
                   {
                       Student student = values.ToObject<Student>();
                       toolbar.Title = $"Welcome {student.Name} {student.Surname}".ToUpper();
                       FirebaseMessaging.Instance.SubscribeToTopic(FirebaseAuth.Instance.Uid);
                   }
               });

            qr_code.Click += Qr_code_Click;

        }

        private void Qr_code_Click(object sender, System.EventArgs e)
        {
            
            QRCodeFragment fragment = new QRCodeFragment();
            fragment.Show(SupportFragmentManager.BeginTransaction(), "");
            //popupDialog = new Dialog(this);
            //popupDialog.SetContentView(Resource.Layout.qrcode_fragment);
            //popupDialog.Show();

            //popupDialog.Window.SetLayout(LayoutParams.MatchParent, LayoutParams.MatchParent);
            //popupDialog.Window.SetBackgroundDrawableResource(Android.Resource.Color.Transparent);

            //close = popupDialog.FindViewById<AppCompatImageView>(Resource.Id.img_close);
            //qr_code_image = popupDialog.FindViewById<AppCompatImageView>(Resource.Id.img_qr);


            
            //close.Click += Close_Click;
            //GeneerateQRCode();

        }

      

        //private void GeneerateQRCode()
        //{
        //    string[] PERMISSIONS =
        //       {
        //            "android.permission.READ_EXTERNAL_STORAGE",
        //            "android.permission.WRITE_EXTERNAL_STORAGE"
        //        };

        //    var permission = ContextCompat.CheckSelfPermission(this, "android.permission.WRITE_EXTERNAL_STORAGE");
        //    var permissionread = ContextCompat.CheckSelfPermission(this, "android.permission.READ_EXTERNAL_STORAGE");

        //    if (permission != Permission.Granted && permissionread != Permission.Granted)
        //        ActivityCompat.RequestPermissions(this, PERMISSIONS, 1);

        //    try
        //    {
        //        if (permission == Permission.Granted && permissionread == Permission.Granted)
        //        {
        //            BitMatrix bitmapMatrix = null;
        //            message = FirebaseAuth.Instance.Uid; ;


        //            bitmapMatrix = new MultiFormatWriter().encode(message, BarcodeFormat.QR_CODE, size, size);

        //            var width = bitmapMatrix.Width;
        //            var height = bitmapMatrix.Height;
        //            int[] pixelsImage = new int[width * height];

        //            for (int i = 0; i < height; i++)
        //            {
        //                for (int j = 0; j < width; j++)
        //                {
        //                    if (bitmapMatrix[j, i])
        //                        pixelsImage[i * width + j] = (int)Convert.ToInt64(0xff000000);
        //                    else
        //                        pixelsImage[i * width + j] = (int)Convert.ToInt64(0xffffffff);

        //                }
        //            }

        //            Bitmap bitmap = Bitmap.CreateBitmap(width, height, Bitmap.Config.Argb8888);
        //            bitmap.SetPixels(pixelsImage, 0, width, 0, 0, width, height);

        //            var sdpath = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
        //            var path = System.IO.Path.Combine(sdpath, "logeshbarcode.jpg");
        //            var stream = new FileStream(path, FileMode.Create);
        //            bitmap.Compress(Bitmap.CompressFormat.Jpeg, 100, stream);
        //            stream.Close();

        //            qr_code_image.SetImageBitmap(bitmap);
        //        }
        //        else
        //        {
        //            Console.WriteLine("No Permission");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Exception {ex} ");
        //    }


        //}

     
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);


        }

        public void OnItemSelected(int id)
        {
            if(id == Resource.Id.nav_home)
            {
                SupportFragmentManager
                    .BeginTransaction()
                    .Replace(Resource.Id.fragment_host, new HomeFragment())
                    .Commit();
            }
            else if (id == Resource.Id.nav_reserved)
            {
                SupportFragmentManager
                    .BeginTransaction()
                    .Replace(Resource.Id.fragment_host, new ReservedFragment())
                    .Commit();

            }
            else if (id == Resource.Id.nav_sell)
            {
                SupportFragmentManager
                    .BeginTransaction()
                    .Replace(Resource.Id.fragment_host, new SellingFragment())
                    .Commit();
            }
            else if (id == Resource.Id.nav_profile)
            {
                SupportFragmentManager
                    .BeginTransaction()
                    .Replace(Resource.Id.fragment_host, new ProfileFragment())
                    .Commit();
            }
            else if (id == Resource.Id.nav_logout)
            {
                AlertDialog.Builder dialog = new AlertDialog.Builder(this);
                AlertDialog alert = dialog.Create();
                alert.SetTitle("Exiting app");
                alert.SetMessage("You are attempting to exit the app! \n Would you like to proceed?");
                alert.SetIcon(Resource.Drawable.ic_round_info_24);
                alert.SetButton("OK", (c, ev) =>
                {
                    FirebaseAuth.Instance.SignOut();
                    if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
                    {
                        base.FinishAndRemoveTask();
                    }
                    else
                    {
                        base.Finish();
                    }
                });
                alert.SetButton2("CANCEL", (c, ev) => { });
                alert.Show();
               
            }
            
        }
    }
}