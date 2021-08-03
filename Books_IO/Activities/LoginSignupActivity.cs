using Android.App;
using Android.Content;
using Android.OS;
using AndroidX.AppCompat.App;
using Books_IO.Fragments;

namespace Books_IO.Activities
{
    [Activity(Label = "LoginSignupActivity")]
    public class LoginSignupActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.activity_login_signup);
            if (savedInstanceState == null)
            {
                LoginFragment frag = new LoginFragment();
                frag.LoginSuccessHandler += Frag_LoginSuccessHandler;
                frag.SignupBtnClickHandler += Frag_SignupBtnClickHandler;
                SupportFragmentManager
                    .BeginTransaction()
                    .Add(Resource.Id.login_host, frag)
                    .Commit();
            }
        }

        private void Frag_SignupBtnClickHandler(object sender, System.EventArgs e)
        {
            SignUpFragment frag = new SignUpFragment();
            frag.BackClickHandler += Frag_BackClickHandler;
            SupportFragmentManager.BeginTransaction()
                .Replace(Resource.Id.login_host, frag)
                    .Commit();
        }

        private void Frag_BackClickHandler(object sender, System.EventArgs e)
        {
            LoginFragment frag = new LoginFragment();
            frag.LoginSuccessHandler += Frag_LoginSuccessHandler;
            frag.SignupBtnClickHandler += Frag_SignupBtnClickHandler;
            SupportFragmentManager
                .BeginTransaction()
                .Add(Resource.Id.login_host, frag)
                .Commit();
        }

        private void Frag_LoginSuccessHandler(object sender, LoginFragment.LoginSuccessEventHandler e)
        {
            Intent intent = new Intent(this, typeof(MainActivity));
            StartActivity(intent);
            OverridePendingTransition(Resource.Animation.Side_in_right, Resource.Animation.Side_out_left);
            Finish();
        }
    }
}