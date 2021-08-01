using Android.OS;
using Android.Views;
using AndroidX.Fragment.App;

namespace Admin.Fragments
{
    public class SignUpFragment : Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment

            base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.signup_fragment, container, false);
            ConnectViews(view);
            return view;
        }

        private void ConnectViews(View view)
        {

        }
    }
}