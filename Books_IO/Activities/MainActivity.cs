using Android.App;
using Android.OS;
using Android.Runtime;
using AndroidX.AppCompat.App;
using Books_IO.Fragments;
using Books_IO.Models;
using Firebase.Auth;
using Firebase.Messaging;
using Google.Android.Material.AppBar;
using Google.Android.Material.BottomNavigation;
using IsmaelDiVita.ChipNavigationLib;
using Plugin.CloudFirestore;

namespace Books_IO
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = false)]
    public class MainActivity : AppCompatActivity, IsmaelDiVita.ChipNavigationLib.ChipNavigationBar.IOnItemSelectedListener
    {
        private ChipNavigationBar bottom_nav;
        private MaterialToolbar toolbar;
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
                FirebaseAuth.Instance.SignOut();
                if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
                {
                    base.FinishAndRemoveTask();
                }
                else
                {
                    base.Finish();
                }
            }
            
        }
    }
}