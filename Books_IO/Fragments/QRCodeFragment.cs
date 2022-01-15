using Android.Content;
using Android.OS;
using Android.Views;
using AndroidX.AppCompat.Widget;
using AndroidX.Fragment.App;
using Firebase.Auth;
using System;
using ZXing;
using ZXing.Mobile;

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
            qr_code = view.FindViewById<AppCompatImageView>(Resource.Id.img_qr);

            GeneerateQRCode();

            close.Click += Close_Click;
        }

        private void Close_Click(object sender, EventArgs e)
        {
            Dismiss();
        }
        public static BarcodeFormat CurrentFormat = BarcodeFormat.QR_CODE;
        Android.Graphics.Bitmap bitmap = null;
        private void GeneerateQRCode()
        {


            try
            {
                var value = FirebaseAuth.Instance.CurrentUser.Uid;
                var writer = new BarcodeWriter
                {
                    Format = CurrentFormat,
                    Options = new ZXing.Common.EncodingOptions
                    {
                        Height = 250,
                        Width = 250
                    }
                };
                bitmap = writer.Write(value);


                //Console.WriteLine($"Exception {bitmap.ByteCount} " );


                qr_code.SetImageBitmap(bitmap);


                //this.buttonSaveToGallery.Enabled = true;




                // BitMatrix bitmapMatrix = null;
                //message = FirebaseAuth.Instance.Uid;

                //var bitmapMatrix = new MultiFormatWriter().encode(message, BarcodeFormat.QR_CODE, size, size);


                //var width = bitmapMatrix.Width;
                //var height = bitmapMatrix.Height;
                //int[] pixelsImage = new int[width * height];

                //for (int i = 0; i < height; i++)
                //{
                //    for (int j = 0; j < width; j++)
                //    {
                //        if (bitmapMatrix[j, i])
                //            pixelsImage[i * width + j] = (int)Convert.ToInt64(0xff000000);
                //        else
                //            pixelsImage[i * width + j] = (int)Convert.ToInt64(0xffffffff);

                //    }
                //}

                //Bitmap bitmap = Bitmap.CreateBitmap(width, height, Bitmap.Config.Argb8888);
                //bitmap.SetPixels(pixelsImage, 0, width, 0, 0, width, height);

                //var sdpath = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
                //var path = System.IO.Path.Combine(sdpath, "logeshbarcode.jpg");
                //var stream = new FileStream(path, FileMode.Create);
                //bitmap.Compress(Bitmap.CompressFormat.Jpeg, 100, stream);
                //stream.Close();

                //qr_code.SetImageBitmap(bitmap);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception {ex} ");
            }
        }
        
    }
 
}