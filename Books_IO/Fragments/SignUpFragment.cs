using Android.Content;
using Android.Gms.Tasks;
using Android.OS;
using Android.Views;
using AndroidX.AppCompat.App;
using AndroidX.Fragment.App;
using Books_IO.Models;
using Firebase.Auth;
using Google.Android.Material.AppBar;
using Google.Android.Material.Button;
using Google.Android.Material.TextField;
using Plugin.CloudFirestore;
using System;
using System.Collections.Generic;

namespace Books_IO.Fragments
{
    public class SignUpFragment : Fragment, IOnCompleteListener, IOnSuccessListener, IOnFailureListener
    {
        private MaterialButton BtnSubmitReg;
        //fire base 

        private TextInputEditText InputName;
        private TextInputEditText InputEmail;
        private TextInputEditText InputSurname;
        private TextInputEditText InputPhone;
        private TextInputEditText InputPassword;
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
        private Context context;
        private void ConnectViews(View view)
        {
            context = view.Context;
            InputName = view.FindViewById<TextInputEditText>(Resource.Id.RegisterInputFirstName);
            InputSurname = view.FindViewById<TextInputEditText>(Resource.Id.RegisterInputLastName);
            InputPhone = view.FindViewById<TextInputEditText>(Resource.Id.RegisterInputPhoneNumber);
            InputEmail = view.FindViewById<TextInputEditText>(Resource.Id.RegisterInputEmail);
            InputPassword = view.FindViewById<TextInputEditText>(Resource.Id.RegisterInputPassword);
            BtnSubmitReg = view.FindViewById<MaterialButton>(Resource.Id.BtnRegister);
            MaterialToolbar toolbar = view.FindViewById<MaterialToolbar>(Resource.Id.toolbar1);
            toolbar.SetNavigationIcon(Resource.Drawable.ic_arrow_back_white);
            toolbar.NavigationClick += Toolbar_NavigationClick;
            BtnSubmitReg.Click += BtnSubmitReg_Click;
        }

        private void BtnSubmitReg_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(InputName.Text) && string.IsNullOrWhiteSpace(InputName.Text))
            {
                InputName.RequestFocus();
                InputName.Error = "provide your name";
                return;
            }
            if (string.IsNullOrEmpty(InputSurname.Text) && string.IsNullOrWhiteSpace(InputSurname.Text))
            {

                InputSurname.RequestFocus();
                InputSurname.Error = "provide your last name";
                return;
            }
            if (string.IsNullOrEmpty(InputPhone.Text) && string.IsNullOrWhiteSpace(InputPhone.Text))
            {
                InputPhone.RequestFocus();
                InputPhone.Error = "provide your phone numbers";
                return;
            }
            if (string.IsNullOrEmpty(InputEmail.Text) && string.IsNullOrWhiteSpace(InputEmail.Text))
            {
                InputEmail.RequestFocus();
                InputEmail.Error = "provide your email address";
                return;
            }
            if (string.IsNullOrEmpty(InputPassword.Text) && string.IsNullOrWhiteSpace(InputPassword.Text))
            {
                InputPassword.RequestFocus();
                InputPassword.Error = "provide your password";
                return;
            }
            FirebaseAuth.Instance.CreateUserWithEmailAndPassword(InputEmail.Text.Trim(), InputPassword.Text.Trim())
                     .AddOnSuccessListener(this)
                     .AddOnFailureListener(this)
                     .AddOnCompleteListener(this);
        }

        public event EventHandler BackClickHandler;
        private void Toolbar_NavigationClick(object sender, AndroidX.AppCompat.Widget.Toolbar.NavigationClickEventArgs e)
        {
            BackClickHandler(sender, e);
        }
        public void OnFailure(Java.Lang.Exception e)
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(context);
            builder.SetTitle("Error");
            builder.SetMessage(e.Message);
            builder.SetNeutralButton("OK", delegate
            {
                builder.Dispose();
            });
            builder.Show();
        }

        public void OnSuccess(Java.Lang.Object result)
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("Name", InputName.Text);
            data.Add("Surname", InputSurname.Text);
            data.Add("Email", InputEmail.Text);
            data.Add("Phone", InputPhone.Text);

            CrossCloudFirestore
                .Current
                .Instance
                .Collection("Students")
                .Document(FirebaseAuth.Instance.Uid)
                .SetAsync(data);
        }

        public void OnComplete(Task task)
        {
            
        }
    }
}