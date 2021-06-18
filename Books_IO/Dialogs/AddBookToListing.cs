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
namespace Books_IO.Dialogs
{
    public class AddBookToListing : DialogFragment
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
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            View view = inflater.Inflate(Resource.Layout.dislog_add_listing, container, false);
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

            book_isbn_no = view.FindViewById<TextInputEditText>(Resource.Id.book_isbn_no);
            book_title = view.FindViewById<TextInputEditText>(Resource.Id.book_title);
            book_price = view.FindViewById<TextInputEditText>(Resource.Id.book_price);
            book_edition = view.FindViewById<TextInputEditText>(Resource.Id.book_edition);
            book_author = view.FindViewById<TextInputEditText>(Resource.Id.book_author);
       //     book_isbn_no = view.FindViewById<TextInputEditText>(Resource.Id.book_isbn_no);

            btn_search_isbn.Click += Btn_search_isbn_Click;

            btn_attachement.Click += Btn_attachement_Click;

        }

        private void Btn_attachement_Click(object sender, EventArgs e)
        {
            
        }

        private void Btn_search_isbn_Click(object sender, EventArgs e)
        {
            book_isbn_no.Text = "9783319255576";
            if (!string.IsNullOrEmpty(book_isbn_no.Text))
            {
                var url = new Uri($"https://api.altmetric.com/v1/isbn/{book_isbn_no.Text}");
                WebClient client = new WebClient();
                client.DownloadStringAsync(url);
                client.DownloadStringCompleted += Client_DownloadStringCompleted;
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

        public override void OnStart()
        {
            base.OnStart();
            Dialog.Window.SetLayout(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
        }
    }
}