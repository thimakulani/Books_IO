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
using Com.Karumi.Dexter.Listener.Multi;
using Com.Karumi.Dexter.Listener.Single;
using EDMTDev.ZXingXamarinAndroid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using ZXing;
using Context = Android.Content.Context;

namespace Admin.Dialogs
{
    public class ScanDlgFragment : DialogFragment, IPermissionListener , IMultiplePermissionsListener, IResultHandler
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
            // Create your fragment here
        }

        Context mContext;
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // inflater.Inflate(Resource.Layout.YourFragment, container, false);
            View view = inflater.Inflate(Resource.Layout.scanner_fragment, container, false);
            SetStyle(ScanDlgFragment.StyleNoTitle,
             Android.Resource.Style.ThemeBlackNoTitleBarFullScreen);
            ConnectViews(view);
            return view;
        }
       // ZXingScannerView scan_view;
        Java.IO.File _file;
        ZXingScannerView scan_view;
        TextView responce;

        string[] PERMISSIONS = { "android.permission.CAMERA" };
        public event EventHandler<ResultEvenHandler> ResultEven;
        public class ResultEvenHandler: EventArgs
        {
            public string Uid { get; set; }
        }
        private void ConnectViews(View view)
        {
            scan_view = view.FindViewById<ZXingScannerView>(Resource.Id.scan_view);
            responce = view.FindViewById<TextView>(Resource.Id.results);
            mContext = view.Context;

            //Dexter.WithActivity(this.Activity)
            //    .WithPermissions(Manifest.Permission.Camera)
            //    .WithListener(this)
            //    .Check();

            scan_view.SetResultHandler(new MyResultsHandler(this));
            scan_view.StartCamera();
        }


        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            scan_view.StopCamera();
        }

        public void OnPermissionDenied(PermissionDeniedResponse p0)
        {

        }

        public void OnPermissionGranted(PermissionGrantedResponse p0)
        {
            scan_view.SetResultHandler(this);
            scan_view.StartCamera();
        }   

        public void OnPermissionRationaleShouldBeShown(PermissionRequest p0, IPermissionToken p1)
        {
        }

        public void OnPermissionRationaleShouldBeShown(IList<PermissionRequest> p0, IPermissionToken p1)
        {
            Toast.MakeText(this.Context, p0.ToString(), ToastLength.Long).Show();
        }

        public void OnPermissionsChecked(MultiplePermissionsReport p0)
        {
            Toast.MakeText(this.Context,p0.ToString(), ToastLength.Long).Show();
        }

        public void HandleResult(Result rawResult)
        {
            responce.Text = rawResult.Text;
            ResultEven.Invoke(this, new ResultEvenHandler { Uid = rawResult.Text });
            Dismiss();
        }

        private class MyResultsHandler : IResultHandler
        {

            private ScanDlgFragment ScanDlgFragment;

            public MyResultsHandler(ScanDlgFragment scanDlgFragment)
            {
                this.ScanDlgFragment = scanDlgFragment;
            }

            public void HandleResult(Result rawResult)
            {
                ScanDlgFragment.responce.Text = rawResult.Text;
                Console.WriteLine(":::::::::::::::::::::::::::::::::::"+rawResult.Text);
                ScanDlgFragment.ResultEven.Invoke(this, new ResultEvenHandler { Uid = rawResult.Text });
                ScanDlgFragment.Dismiss();
            }
        }
    }
}