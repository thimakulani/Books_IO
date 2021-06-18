using Android.OS;
using Android.Views;
using AndroidX.Fragment.App;
using Books_IO.Models;
using Firebase.Auth;
using Plugin.CloudFirestore;
using System;

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

        private void ConnectViews(View view)
        {



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

    }
}