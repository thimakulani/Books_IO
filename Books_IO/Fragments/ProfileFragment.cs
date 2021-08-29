using Android.Content;
using Android.OS;
using Android.Views;
using AndroidHUD;
using AndroidX.Fragment.App;
using Books_IO.Models;
using Firebase.Auth;
using Google.Android.Material.Button;
using Google.Android.Material.TextField;
using Plugin.CloudFirestore;
using System;
using System.Collections.Generic;

namespace Books_IO.Fragments
{
    public class ProfileFragment : Fragment
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
            var view = inflater.Inflate(Resource.Layout.profile_fragment, container, false);
            ConnectViews(view);
            return view;
        }
        private TextInputEditText InputName; 
        private TextInputEditText InputLastName; 
        private TextInputEditText InputEmail; 
        private TextInputEditText InputPhone;
        private MaterialButton BtnUpdate;
        private Context context;
        private void ConnectViews(View view)
        {
            context = view.Context;
            InputName = view.FindViewById<TextInputEditText>(Resource.Id.profile_name);
            InputLastName = view.FindViewById<TextInputEditText>(Resource.Id.profile_lastname);
            InputEmail = view.FindViewById<TextInputEditText>(Resource.Id.profile_email);
            InputPhone = view.FindViewById<TextInputEditText>(Resource.Id.profile_phone);
            BtnUpdate = view.FindViewById<MaterialButton>(Resource.Id.btn_update);

            InputEmail.Enabled = false;
            BtnUpdate.Click += BtnUpdate_Click;
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
                        InputName.Text = student.Name;
                        InputLastName.Text = student.Surname;
                        InputPhone.Text = student.Phone;
                        InputEmail.Text = student.Email;
                    }
                });

        }

        private void BtnUpdate_Click(object sender, System.EventArgs e)
        {

            if (string.IsNullOrEmpty(InputName.Text) && string.IsNullOrWhiteSpace(InputName.Text))
            {
                InputName.RequestFocus();
                InputName.Error = "provide your name";
                return;
            }
            if (string.IsNullOrEmpty(InputLastName.Text) && string.IsNullOrWhiteSpace(InputLastName.Text))
            {

                InputLastName.RequestFocus();
                InputLastName.Error = "provide your last name";
                return;
            }
            if (string.IsNullOrEmpty(InputPhone.Text) && string.IsNullOrWhiteSpace(InputPhone.Text))
            {
                InputPhone.RequestFocus();
                InputPhone.Error = "provide your phone numbers";
                return;
            }

            Dictionary<string, object> data = new Dictionary<string, object>
            {
                { "Name", InputName.Text },
                { "Surname", InputLastName.Text },
                { "Phone", InputPhone.Text }
            };
            CrossCloudFirestore
                .Current
                .Instance
                .Collection("Students")
                .Document(FirebaseAuth.Instance.Uid)
                .UpdateAsync(data);
            AndHUD.Shared.ShowError(context, "Successfully Updated", MaskType.Clear, TimeSpan.FromSeconds(3));

        }
    }
}