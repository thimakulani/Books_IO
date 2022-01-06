using Admin.Activities;
using Android;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidX.Core.Content;
using AndroidX.Fragment.App;
using Com.Karumi.Dexter;
using Com.Karumi.Dexter.Listener;
using Com.Karumi.Dexter.Listener.Single;
using EDMTDev.ZXingXamarinAndroid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZXing;

namespace Admin.Dialogs
{
    public class ScanDlgFragment : DialogFragment, IPermissionListener, IResultHandler
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
            View view = inflater.Inflate(Resource.Layout.scanner_fragment, container, false);
            ConnectViews(view);
            return view;
        }
        ZXingScannerView scan_view;

        string[] PERMISSIONS = { "android.permission.CAMERA" };
        private void ConnectViews(View view)
        {
            scan_view = view.FindViewById<ZXingScannerView>(Resource.Id.scan_view);
           

            if (ContextCompat.CheckSelfPermission(view.Context, Manifest.Permission.Camera) == (int)Permission.Granted)
            {
                scan_view.SetResultHandler(this);
            }
            else
            {
                Toast.MakeText(view.Context, "Permission not granted", ToastLength.Long).Show();
                //AndroidX.Core.App.ActivityCompat.RequestPermissions((Android.App.Activity)Android.App.Application.Context, PERMISSIONS, 1);

            }

            
        }

        public void OnPermissionDenied(PermissionDeniedResponse p0)
        {
            
        }

        public void OnPermissionGranted(PermissionGrantedResponse p0)
        {

        }

        public void OnPermissionRationaleShouldBeShown(PermissionRequest p0, IPermissionToken p1)
        {
        }

        public void HandleResult(Result rawResult)
        {
            
        }
    }
}