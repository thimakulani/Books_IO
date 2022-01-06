using Android.Content;
using Android.OS;
using Android.Views;
using AndroidX.AppCompat.Widget;
using AndroidX.Fragment.App;
using AndroidX.RecyclerView.Widget;
using Books_IO.Adapters;
using Books_IO.Dialogs;
using Books_IO.Models;
using Firebase.Auth;
using Google.Android.Material.FloatingActionButton;
using Plugin.CloudFirestore;
using System;
using System.Net;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Net.Mail;
using MimeKit;
using MailKit.Net.Smtp;
using AndroidHUD;
using Android.Graphics;
using System.IO;
using Android.Content.PM;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using ZXing.Common;
using ZXing;

namespace Books_IO.Fragments
{
    public class QRCodeFragment : DialogFragment
    {


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.qrcode_fragment, container, false);
            ConnectViews(view);

            return view;
        }

        private AppCompatImageView qr_code, close;
        private Context context;


        private string message;
        private static int size = 660;
        private static int small_size = 264;

        private void ConnectViews(View view)
        {
            context = view.Context;

            close = view.FindViewById<AppCompatImageView>(Resource.Id.img_close);
            qr_code = view.FindViewById<AppCompatImageView>(Resource.Id.img_unique_qr_code);

            GeneerateQRCode();

            close.Click += Close_Click;
        }

        private void Close_Click(object sender, EventArgs e)
        {
            Dismiss();
        }

        private void GeneerateQRCode()
        {
            string[] PERMISSIONS =
               {
                    "android.permission.READ_EXTERNAL_STORAGE",
                    "android.permission.WRITE_EXTERNAL_STORAGE"
                };

            var permission = ContextCompat.CheckSelfPermission(context, "android.permission.WRITE_EXTERNAL_STORAGE");
            var permissionread = ContextCompat.CheckSelfPermission(context, "android.permission.READ_EXTERNAL_STORAGE");

            if (permission != Permission.Granted && permissionread != Permission.Granted)
               // ActivityCompat.RequestPermissions(Activity.C, PERMISSIONS, 1);

            try
            {
                if (permission == Permission.Granted && permissionread == Permission.Granted)
                {
                    BitMatrix bitmapMatrix = null;
                    message = FirebaseAuth.Instance.Uid;

                    bitmapMatrix = new MultiFormatWriter().encode(message, BarcodeFormat.QR_CODE, size, size);


                    var width = bitmapMatrix.Width;
                    var height = bitmapMatrix.Height;
                    int[] pixelsImage = new int[width * height];

                    for (int i = 0; i < height; i++)
                    {
                        for (int j = 0; j < width; j++)
                        {
                            if (bitmapMatrix[j, i])
                                pixelsImage[i * width + j] = (int)Convert.ToInt64(0xff000000);
                            else
                                pixelsImage[i * width + j] = (int)Convert.ToInt64(0xffffffff);

                        }
                    }

                    Bitmap bitmap = Bitmap.CreateBitmap(width, height, Bitmap.Config.Argb8888);
                    bitmap.SetPixels(pixelsImage, 0, width, 0, 0, width, height);

                    var sdpath = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
                    var path = System.IO.Path.Combine(sdpath, "logeshbarcode.jpg");
                    var stream = new FileStream(path, FileMode.Create);
                    bitmap.Compress(Bitmap.CompressFormat.Jpeg, 100, stream);
                    stream.Close();

                    qr_code.SetImageBitmap(bitmap);
                }
                else
                {
                    Console.WriteLine("No Permission");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception {ex} ");
            }
        }
    }
 
}