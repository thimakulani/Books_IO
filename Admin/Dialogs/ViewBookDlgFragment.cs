using Android.Content;
using Android.OS;
using Android.Views;
using AndroidX.Fragment.App;
using Google.Android.Material.Button;
using Google.Android.Material.TextField;
using System;
using Plugin.CloudFirestore;
using Admin.Models;
using AndroidHUD;

namespace Admin.Dialogs
{
    public class ViewBookDlgFragment : DialogFragment
    {
        private readonly string book_id = null;

        public ViewBookDlgFragment(string book_id)
        {
            this.book_id = book_id;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetStyle(StyleNoFrame, Resource.Style.FullScreenDialogStyle);
            // Create your fragment here
        }
        private TextInputEditText book_isbn_no;
        private TextInputEditText book_title;
        private TextInputEditText book_price;
        private TextInputEditText book_edition;
        private TextInputEditText book_author;
        private TextInputEditText book_status;
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
        string student_id = null;
        private void ConnectViews(View view)
        {

            context = view.Context;

            book_isbn_no = view.FindViewById<TextInputEditText>(Resource.Id.view_book_isbn);
            book_title = view.FindViewById<TextInputEditText>(Resource.Id.view_book_title);
            book_price = view.FindViewById<TextInputEditText>(Resource.Id.view_book_price);
            book_edition = view.FindViewById<TextInputEditText>(Resource.Id.view_book_edition);
            book_author = view.FindViewById<TextInputEditText>(Resource.Id.view_book_author);
            book_status = view.FindViewById<TextInputEditText>(Resource.Id.view_book_status);
            btn_add_reserve = view.FindViewById<MaterialButton>(Resource.Id.btn_reserve);
            btn_add_reserve.Click += Btn_add_reserve_Click;
            btn_add_reserve.Text = "APPROVE";

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
                        book_price.Text = books.Price;
                        book_title.Text = books.Title;
                        student_id = books.Student_Id;
                        book_status.Text = books.Status;
                    }
                });
        }

        private void Btn_add_reserve_Click(object sender, EventArgs e)
        {
            CrossCloudFirestore.Current
                .Instance
                .Collection("BooksListings")
                .Document(book_id)
                .UpdateAsync("Status", "Approved");


            AndHUD.Shared.ShowSuccess(context, "Successfully updated", MaskType.Clear, TimeSpan.FromSeconds(3));
        }
    }
}