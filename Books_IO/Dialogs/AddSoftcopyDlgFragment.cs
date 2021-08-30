using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.Fragment.App;
using Google.Android.Material.Button;
using Google.Android.Material.TextField;
using Google.Android.Material.TextView;
using System;
using System.Net;
using Newtonsoft.Json;
using Plugin.Media;
using Android.Graphics;
using System.Collections.Generic;
using Plugin.CloudFirestore;
using Firebase.Storage;
using Android.Gms.Tasks;
using Firebase.Auth;
using ID.IonBit.IonAlertLib;

namespace Books_IO.Dialogs
{
    public class AddSoftcopyDlgFragment : Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            return base.OnCreateView(inflater, container, savedInstanceState);
        }
    }
}