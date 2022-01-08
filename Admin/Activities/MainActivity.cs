using Admin.Fragments;
using Admin.Models;
using Android.App;
using Android.OS;
using Android.Runtime;
using AndroidX.AppCompat.App;
using Firebase.Auth;
using IsmaelDiVita.ChipNavigationLib;
using Plugin.CloudFirestore;
using AlertDialog = Android.App.AlertDialog;

namespace Admin.Activities
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = false)]
    public class MainActivity : AppCompatActivity, IsmaelDiVita.ChipNavigationLib.ChipNavigationBar.IOnItemSelectedListener
    {
        private ChipNavigationBar bottom_nav;
        string[] PERMISSIONS = { "android.permission.CAMERA" };
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
            AndroidX.Core.App.ActivityCompat.RequestPermissions(this, PERMISSIONS, 1);

            bottom_nav = FindViewById<ChipNavigationBar>(Resource.Id.bottom_nav);
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
                 }
             });

        }

       

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
            if (id == Resource.Id.nav_reserved)
            {
                SupportFragmentManager
                    .BeginTransaction()
                    .Replace(Resource.Id.fragment_host, new ReserveBooksFragment())
                    .Commit();
            }
            if(id == Resource.Id.nav_categories)
            {
                SupportFragmentManager
                    .BeginTransaction()
                    .Replace(Resource.Id.fragment_host, new CategoryFragment())
                    .Commit();
            }
            if (id == Resource.Id.nav_logout)
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