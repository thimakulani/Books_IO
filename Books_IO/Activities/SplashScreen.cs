using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using Firebase.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Books_IO.Activities
{
    [Activity(Label = "@string/app_name", Theme = "@style/MyTheme.Splash", MainLauncher = true, NoHistory = true)]
    public class SplashScreen : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            Task startWork = new Task(() =>
            {
                Task.Delay(3000);
            });
            startWork.ContinueWith(t =>
            {
                var user = FirebaseAuth.Instance.CurrentUser;
                if (user != null)
                {
                    Intent intent = new Intent(Application.Context, typeof(MainActivity));
                    StartActivity(intent);
                    OverridePendingTransition(Resource.Animation.Side_in_right, Resource.Animation.Side_out_left);
                    Finish();
                }
                else
                {
                    Intent intent = new Intent(Application.Context, typeof(LoginSignupActivity));
                    StartActivity(intent);
                     OverridePendingTransition(Resource.Animation.Side_in_right, Resource.Animation.Side_out_left);
                    Finish();
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
            startWork.Start();
        }
    }
    
}