using Android.Content;
using Android.Gms.Tasks;
using Android.OS;
using Android.Views;
using AndroidHUD;
using AndroidX.AppCompat.App;
using AndroidX.Fragment.App;
using Books_IO.Models;
using Firebase.Auth;
using FirebaseAdmin.Auth;
using Google.Android.Material.AppBar;
using Google.Android.Material.Button;
using Google.Android.Material.TextField;
using ID.IonBit.IonAlertLib;
using Plugin.CloudFirestore;
using System;
using System.Collections.Generic;

namespace Books_IO.Fragments
{
    public class SignUpFragment : Fragment, IOnCompleteListener, IOnSuccessListener, IOnFailureListener
    {
        private MaterialButton BtnSubmitReg;
        //fire base 

        private TextInputEditText InputStudentNumber;
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
            InputStudentNumber = view.FindViewById<TextInputEditText>(Resource.Id.RegisterInputStudentNumber);
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
        private IonAlert loadingDialog;
        private async void BtnSubmitReg_Click(object sender, EventArgs e)
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
            if (string.IsNullOrEmpty(InputStudentNumber.Text) && string.IsNullOrWhiteSpace(InputStudentNumber.Text))
            {
                InputStudentNumber.RequestFocus();
                InputPassword.Error = "provide your id";
                return;
            }
            BtnSubmitReg.Enabled = false;
            loadingDialog = new IonAlert(context, IonAlert.ProgressType);
            loadingDialog.SetSpinKit("DoubleBounce")
                .SetSpinColor("#008D91")
                .ShowCancelButton(false)
                .Show();

            var stream = Resources.Assets.Open("ServiceAccount.json");
            var auth = FirebaseHelper.FirebaseAdminSDK.GetFirebaseAdminAuth(stream);

            UserRecordArgs user = new UserRecordArgs()
            {
                Email = InputEmail.Text.Trim(),
                Password = InputPassword.Text.Trim(),
                Uid = InputStudentNumber.Text.Trim()
            };
            try
            {
                var results = await auth.CreateUserAsync(user);
                Dictionary<string, object> data = new Dictionary<string, object>();
                data.Add("Name", InputName.Text);
                data.Add("Surname", InputSurname.Text);
                data.Add("Email", InputEmail.Text);
                data.Add("StudentId", InputStudentNumber.Text);
                data.Add("Phone", InputPhone.Text);

                await CrossCloudFirestore
                    .Current
                    .Instance
                    .Collection("Students")
                    .Document(results.Uid)
                    .SetAsync(data);
                AndHUD.Shared.ShowSuccess(context, "Successfully Registered", MaskType.Clear, TimeSpan.FromSeconds(3));
                BackClickHandler(sender, e);
            }
            catch (Exception ex)
            {
                AndHUD.Shared.ShowError(context, ex.Message, MaskType.Clear, TimeSpan.FromSeconds(3));
            }
            finally
            {
                loadingDialog.Dismiss();
            }
            //FirebaseAuth.Instance.CreateUserWithEmailAndPassword(InputEmail.Text.Trim(), InputPassword.Text.Trim())
            //         .AddOnSuccessListener(this)
            //         .AddOnFailureListener(this)
            //         .AddOnCompleteListener(this);
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
            
            Intent intent = new Intent(context, typeof(MainActivity));
            StartActivity(intent);
        }

        public void OnComplete(Task task)
        {
            BtnSubmitReg.Enabled = true;
            loadingDialog.Dismiss();
        }
    }
}