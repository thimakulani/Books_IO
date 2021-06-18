using Android.App;
using Android.OS;
using Android.Runtime;
using AndroidX.AppCompat.App;
using Books_IO.Fragments;
using Google.Android.Material.BottomNavigation;

namespace Books_IO
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private BottomNavigationView bottom_nav;
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


            bottom_nav = FindViewById<BottomNavigationView>(Resource.Id.bottom_nav);
            bottom_nav.NavigationItemSelected += Bottom_nav_NavigationItemSelected;


        }

        private void Bottom_nav_NavigationItemSelected(object sender, BottomNavigationView.NavigationItemSelectedEventArgs e)
        {

        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);


        }
    }
}