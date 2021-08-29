using Admin.Fragments;
using Admin.Models;
using Android.App;
using Android.OS;
using Android.Runtime;
using AndroidX.AppCompat.App;
using Firebase.Auth;
using IsmaelDiVita.ChipNavigationLib;
using Plugin.CloudFirestore;

namespace Admin.Activities
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = false)]
    public class MainActivity : AppCompatActivity, IsmaelDiVita.ChipNavigationLib.ChipNavigationBar.IOnItemSelectedListener
    {
        private ChipNavigationBar bottom_nav;
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
            
        }
    }
}