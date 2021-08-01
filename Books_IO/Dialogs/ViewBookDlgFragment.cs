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
using Books_IO.Models;
using Firebase.Auth;

namespace Books_IO.Dialogs
{
    public class ViewBookDlgFragment : DialogFragment
    {
        private string book_id = null;

        public ViewBookDlgFragment(string book_id)
        {
            this.book_id = book_id;
        }

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
        private MaterialButton btn_add_reserve;
        private Context context;
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.view_book_fragment, container, false);
            ConnectViews(view);
            return view;
        }

        private void ConnectViews(View view)
        {

            context = view.Context;

            book_isbn_no = view.FindViewById<TextInputEditText>(Resource.Id.view_book_isbn);
            book_title = view.FindViewById<TextInputEditText>(Resource.Id.view_book_title);
            book_price = view.FindViewById<TextInputEditText>(Resource.Id.view_book_price);
            book_edition = view.FindViewById<TextInputEditText>(Resource.Id.view_book_edition);
            book_author = view.FindViewById<TextInputEditText>(Resource.Id.view_book_author);
            btn_add_reserve = view.FindViewById<MaterialButton>(Resource.Id.btn_reserve);
            btn_add_reserve.Click += Btn_add_reserve_Click;


            CrossCloudFirestore.Current
                .Instance
                .Collection("BooksListings")
                .Document(book_id)
                .AddSnapshotListener((value, errors) =>
                {
                    if (value.Exists)
                    {
                        Books books = value.ToObject<Books>();
                        book_author.Text = books.Author;
                        book_edition.Text = books.Edition;
                        book_isbn_no.Text = books.ISBN;
                        book_title.Text = books.Title;
                        book_price.Text = books.Price;
                    }
                });
        }

        private void Btn_add_reserve_Click(object sender, EventArgs e)
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("Book_Id", book_id);
            data.Add("TimeStamp", FieldValue.ServerTimestamp);
            CrossCloudFirestore.Current
                .Instance
                .Collection("Reserved")
                .Document(FirebaseAuth.Instance.Uid)
                .SetAsync(data);
        }
    }
}