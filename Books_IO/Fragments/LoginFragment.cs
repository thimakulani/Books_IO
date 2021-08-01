using Android.Content;
using Android.Gms.Tasks;
using Android.OS;
using Android.Views;
using AndroidHUD;
using AndroidX.Fragment.App;
using Books_IO.Dialogs;
using Firebase.Auth;
using Google.Android.Material.Button;
using Google.Android.Material.TextField;
using Google.Android.Material.TextView;
using ID.IonBit.IonAlertLib;
using System;

namespace Books_IO.Fragments
{
    public class LoginFragment : Fragment, IOnSuccessListener, IOnFailureListener, IOnCompleteListener
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }
        private MaterialTextView TxtForgotPassword;
        private TextInputEditText InputEmail;
        private TextInputEditText InputPassword;
        private MaterialButton BtnLogin;
        private MaterialButton BtnSignup;
        private IonAlert loadingDialog;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment

            base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.login_fragment, container, false);
            ConnectViews(view);
            return view;
        }
        private Context context;
        private void ConnectViews(View view)
        {
            context = view.Context;
            BtnLogin = view.FindViewById<MaterialButton>(Resource.Id.BtnLogin);
            BtnSignup = view.FindViewById<MaterialButton>(Resource.Id.BtnSignUp);
            // TxtSignUp = FindViewById<TextView>(Resource.Id.TxtCreateAccount);
            TxtForgotPassword = view.FindViewById<MaterialTextView>(Resource.Id.TxtForgotPassword);
            InputEmail = view.FindViewById<TextInputEditText>(Resource.Id.LoginInputEmail);
            InputPassword = view.FindViewById<TextInputEditText>(Resource.Id.LoginInputPassword);
            BtnLogin.Click += BtnLogin_Click;
            BtnSignup.Click += BtnSignup_Click;
            TxtForgotPassword.Click += TxtForgotPassword_Click;
        }

        private void TxtForgotPassword_Click(object sender, EventArgs e)
        {
            ResetPasswordDlgFragment dlg = new ResetPasswordDlgFragment();
            dlg.Show(ChildFragmentManager.BeginTransaction(), null);
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(InputEmail.Text) && string.IsNullOrWhiteSpace(InputEmail.Text))
            {
                InputEmail.Error = "Please provide your email";
                return;
            }
            if (string.IsNullOrEmpty(InputPassword.Text) && string.IsNullOrWhiteSpace(InputPassword.Text))
            {
                InputPassword.Error = "Please provide password";
                return;
            }
            BtnLogin.Enabled = false;
            loadingDialog = new IonAlert(context, IonAlert.ProgressType);
            loadingDialog.SetSpinKit("DoubleBounce")
                .SetSpinColor("#008D91")
                .ShowCancelButton(false)
                .Show();
            FirebaseAuth.Instance.SignInWithEmailAndPassword(InputEmail.Text.Trim(), InputPassword.Text.Trim())
                .AddOnSuccessListener(this)
                .AddOnCompleteListener(this)
                .AddOnFailureListener(this);
        }
        public event EventHandler SignupBtnClickHandler;
        private void BtnSignup_Click(object sender, EventArgs e)
        {
            SignupBtnClickHandler.Invoke(sender, e);
        }

        public void OnFailure(Java.Lang.Exception e)
        {
            AndHUD.Shared.ShowError(context, e.Message, MaskType.Clear, TimeSpan.FromSeconds(3));
        }
        public event EventHandler<LoginSuccessEventHandler> LoginSuccessHandler;
        public class LoginSuccessEventHandler: EventArgs
        {
            public bool Success { get; set; }
        }
        public void OnSuccess(Java.Lang.Object result)
        {
            LoginSuccessHandler.Invoke(this, new LoginSuccessEventHandler { Success = true });
        }

        public void OnComplete(Task task)
        {
            loadingDialog.Dismiss();
            BtnLogin.Enabled = true;
        }
    }
}