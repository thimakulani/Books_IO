using Android.Content;
using Android.Gms.Tasks;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidHUD;
using AndroidX.AppCompat.App;
using AndroidX.Fragment.App;
using Books_IO.Models;
using Firebase.Auth;
using Firebase.Storage;
using FirebaseAdmin.Auth;
using Google.Android.Material.AppBar;
using Google.Android.Material.Button;
using Google.Android.Material.TextField;
using Google.Android.Material.TextView;
using ID.IonBit.IonAlertLib;
using Newtonsoft.Json;
using Plugin.CloudFirestore;
using Plugin.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Task = Android.Gms.Tasks.Task;

namespace Books_IO.Dialogs
{
    public class AddBookToListing : DialogFragment, IOnSuccessListener, IOnCompleteListener, IOnFailureListener
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }
        private TextInputEditText book_isbn_no;
        private TextInputEditText book_title;
        private TextInputEditText book_price;
        private TextInputEditText book_edition;
        private TextInputEditText book_author;
        private MaterialButton btn_search_isbn;
        private MaterialButton btn_add_book;
        private MaterialButton btn_attachement;
        private MaterialButton btn_remove_attachement;
        private MaterialTextView txt_attachment;
        private Context context;

        int item_id;

        public AddBookToListing(int item_id)
        {
            this.item_id = item_id;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            View view = inflater.Inflate(Resource.Layout.dialog_add_listing, container, false);
            ConnectViews(view);
            return view;
        }

        private void ConnectViews(View view)
        {
            context = view.Context;
            btn_search_isbn = view.FindViewById<MaterialButton>(Resource.Id.btn_search_isbn);
            btn_add_book = view.FindViewById<MaterialButton>(Resource.Id.btn_add_book);
            btn_attachement = view.FindViewById<MaterialButton>(Resource.Id.btn_attachement);
            btn_remove_attachement = view.FindViewById<MaterialButton>(Resource.Id.btn_remove_attachement);

            txt_attachment = view.FindViewById<MaterialTextView>(Resource.Id.book_attachement);
            if(item_id == 0)
            {
                txt_attachment.Text = "No Image";
                btn_attachement.Text = "SELECT IMAGE";
            }
            else
            {
                txt_attachment.Text = "No File";
                btn_attachement.Text = "SELECT FILE";

            }

            book_isbn_no = view.FindViewById<TextInputEditText>(Resource.Id.book_isbn_no);
            book_title = view.FindViewById<TextInputEditText>(Resource.Id.book_title);
            book_price = view.FindViewById<TextInputEditText>(Resource.Id.book_price);
            book_edition = view.FindViewById<TextInputEditText>(Resource.Id.book_edition);
            book_author = view.FindViewById<TextInputEditText>(Resource.Id.book_author);
       //     book_isbn_no = view.FindViewById<TextInputEditText>(Resource.Id.book_isbn_no);

            btn_search_isbn.Click += Btn_search_isbn_Click;
            btn_add_book.Click += Btn_add_book_Click;
            btn_attachement.Click += Btn_attachement_Click;
            btn_remove_attachement.Click += Btn_remove_attachement_Click;

        }

        private void Btn_remove_attachement_Click(object sender, EventArgs e)
        {
            imageArray = null;
            txt_attachment.Text = "No Image";
        }
        string file_type = null;
        IDocumentReference query;
        StorageReference storage_ref;
        IonAlert loadingDialog;
        private async  void Btn_add_book_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(book_title.Text))
            {
                book_title.Error = "Provide Book Title";
                return;
            }
            if (string.IsNullOrEmpty(book_edition.Text))
            {
                book_edition.Error = "Provide edition";
                return;
            }
            if (string.IsNullOrEmpty(book_author.Text))
            {
                book_author.Error = "Provide Author(s)";
                return;
            }
            if (string.IsNullOrEmpty(book_price.Text))
            {
                book_price.Error = "Provide price";
                return;
            }
            
            loadingDialog = new IonAlert(context, IonAlert.ProgressType);
            loadingDialog.SetSpinKit("DoubleBounce")
                .SetSpinColor("#008D91")
                .ShowCancelButton(false)
                .Show();

            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("Title", book_title.Text);
            data.Add("Edition", book_edition.Text);
            data.Add("ISBN", book_isbn_no.Text);
            data.Add("Author", book_author.Text);
            data.Add("FileType", file_type);
            data.Add("ImageUrl", null);
            data.Add("Status", "Pending");
            data.Add("FacultyId", null);
            data.Add("Price", book_price.Text);
            data.Add("Student_Id", Firebase.Auth.FirebaseAuth.Instance.Uid);
            data.Add("TimeStamp", FieldValue.ServerTimestamp);

            query = await CrossCloudFirestore
                .Current
                .Instance
                .Collection("BooksListings")
                .AddAsync(data);
            //Toast.MakeText(context, query.Id, ToastLength.Long).Show();
            if (item_id == 0)
            {

                if (imageArray != null)
                {
                    storage_ref = FirebaseStorage
                        .Instance
                        .GetReference("Images").Child(query.Id);
                    storage_ref.PutBytes(imageArray)
                        .AddOnSuccessListener(this)
                        .AddOnFailureListener(this)
                        .AddOnCompleteListener(this);
                    
                }
                else
                {
                    loadingDialog.Dismiss();
                    Dismiss();
                }
            }
            else
            {
                if(PdfFile != null)
                {
                    storage_ref = FirebaseStorage
                        .Instance
                        .GetReference("Documents").Child(query.Id);
                    storage_ref.PutStream(PdfFile)
                        .AddOnSuccessListener(this)
                        .AddOnFailureListener(this)
                        .AddOnCompleteListener(this);
                }
            }

        }

        private async void Btn_attachement_Click(object sender, EventArgs e)
        {
            if(item_id == 0)
            {
                ChosePicture();
            }
            else
            {
                var options = new PickOptions
                {
                    PickerTitle = "Please select a comic file",
                    FileTypes = FilePickerFileType.Pdf
                };
                PickAndShow(options);
            }
        }
        Stream PdfFile;
       private async void PickAndShow(PickOptions options)
        {
            try
            {
                var result = await FilePicker.PickAsync(options);
                if (result != null)
                {
                    
                        PdfFile = await result.OpenReadAsync();
                        txt_attachment.Text = "File Selected";
                    file_type = "DPF";
                }

               
            }
            catch (Exception ex)
            {
                // The user canceled or something went wrong
            }

           
        }
        private void Btn_search_isbn_Click(object sender, EventArgs e)
        {
            
            if (!string.IsNullOrEmpty(book_isbn_no.Text))
            {
                var url = new Uri($"https://api.altmetric.com/v1/isbn/{book_isbn_no.Text}");
                WebClient client = new WebClient();
                client.DownloadStringAsync(url);
                
                
                client.DownloadStringCompleted += Client_DownloadStringCompleted;
            }
            else
            {
                book_isbn_no.Error = "Provide ISBN";
            }
        }

        private void Client_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                BookModel book = new BookModel();
                book = JsonConvert.DeserializeObject<BookModel>(e.Result);
                Toast.MakeText(context, book.Title, ToastLength.Long).Show();
                book_edition.Text = null;
                book_title.Text = book.Title;
                string authors = null;
                foreach(var a in book.Authors)
                {
                    authors = a;
                }
                book_author.Text = authors;

            }
            catch (Exception ex)
            {
                Toast.MakeText(context, ex.Message, ToastLength.Long).Show();
            }

        }
       // StorageReference storageRef;
        private byte[] imageArray;

        private async void ChosePicture()
        {
            await CrossMedia.Current.Initialize();
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                Toast.MakeText(context, "Upload not supported on this device", ToastLength.Short).Show();
                return;
            }
            try
            {
                var file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
                {
                    PhotoSize = Plugin.Media.Abstractions.PhotoSize.Full,
                    CompressionQuality = 50,
                    CustomPhotoSize = 100,
                    

                });
                imageArray = System.IO.File.ReadAllBytes(file.Path);

                if (imageArray != null)
                {
                    //Bitmap bmp = BitmapFactory.DecodeByteArray(imageArray, 0, imageArray.Length);
                    //ImgAwareness.SetImageBitmap(bmp);
                    txt_attachment.Text = "Image selected";
                    file_type = "IMG";

                }

            }
            catch (Exception ex)
            {
                Toast.MakeText(context, ex.Message, ToastLength.Long).Show();
            }

        }

        public override void OnStart()
        {
            base.OnStart();
            Dialog.Window.SetLayout(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
        }

        public async void OnSuccess(Java.Lang.Object result)
        {
            if(storage_ref != null)
            {
                var url = await storage_ref.GetDownloadUrlAsync();
                if(url != null)
                {
                    //Toast.MakeText(context, url.ToString(), ToastLength.Long).Show();
                    await query.UpdateAsync("ImageUrl", url.ToString());
                }
            }
        }

        public void OnComplete(Task task)
        {
            loadingDialog.Dismiss();
            Dismiss();
        }

        public void OnFailure(Java.Lang.Exception e)
        {
            AndroidHUD.AndHUD.Shared.ShowSuccess(context, e.Message, AndroidHUD.MaskType.Clear, TimeSpan.FromSeconds(2));
            query.DeleteAsync();
        }
    }
}