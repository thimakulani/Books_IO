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

namespace Books_IO.Fragments
{
    public class HomeFragment : Fragment
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
            View view = inflater.Inflate(Resource.Layout.home_fragment, container, false);
            ConnectViews(view);
            return view;
        }
        private ExtendedFloatingActionButton fab_add_book;
        private readonly List<Books> Items = new List<Books>();
        private Context context;
        private void ConnectViews(View view)
        {
            context = view.Context;
            fab_add_book = view.FindViewById<ExtendedFloatingActionButton>(Resource.Id.fab_add_book);
            RecyclerView recycler_books_list = view.FindViewById<RecyclerView>(Resource.Id.recycler_books_list);
            LinearLayoutManager linearLayoutManager = new LinearLayoutManager(view.Context);
            ListingAdapter adapter = new ListingAdapter(Items);
            recycler_books_list.SetLayoutManager(linearLayoutManager);
            recycler_books_list.SetAdapter(adapter);
            adapter.ItemClick += Adapter_ItemClick;
            adapter.DownloadClick += Adapter_DownloadClick;
            CrossCloudFirestore
                .Current
                .Instance
                .Collection("BooksListings")
                .WhereEqualsTo("Status", "Approved")
                .AddSnapshotListener((values, error) =>
                {
                    if (!values.IsEmpty)
                    {
                        foreach (var data in values.DocumentChanges)
                        {
                            switch (data.Type)
                            {
                                case DocumentChangeType.Added:
                                    Items.Add(data.Document.ToObject<Books>());
                                    adapter.NotifyDataSetChanged();
                                    break;
                                case DocumentChangeType.Modified:
                                    Items[data.OldIndex] = data.Document.ToObject<Books>();
                                    adapter.NotifyDataSetChanged();
                                    break;
                                case DocumentChangeType.Removed:
                                    Items.RemoveAt(data.OldIndex);
                                    adapter.NotifyDataSetChanged();
                                    break;
                                default:
                                    break;
                            }
                        }
                    }

                });

            fab_add_book.Click += Fab_add_book_Click;
        }

        private async void Adapter_DownloadClick(object sender, ListingAdapterClickEventArgs e)
        {
            var data = await CrossCloudFirestore.Current
                .Instance
                .Collection("Students")
                .Document(FirebaseAuth.Instance.Uid)
                .GetAsync();
            Student student = data.ToObject<Student>();

            //if (data.Exists)
            //{
            //    var url = new Uri("https://thimakulani.000webhostapp.com/action_page.php");
            //    WebClient webClient = new WebClient();
            //    NameValueCollection valueName = new NameValueCollection();

            //    valueName.Add("body", $"Book title: {Items[e.Position].Title}\n Download Url: {Items[e.Position].ImageUrl}");
            //    valueName.Add("recepent_name", $"{student.Name} {student.Surname}");
            //    valueName.Add("recepent_email", $"{student.Email.Trim()}");
            //    webClient.UploadValuesAsync(url,"POST", valueName);
            //    //Android.Widget.Toast.MakeText(context, "XX", Android.Widget.ToastLength.Long).Show();
            //    webClient.UploadValuesCompleted += WebClient_UploadValuesCompleted;
            //}
            
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("BOOK.IO", "thimakulani@gmail.com"));
            message.To.Add(new MailboxAddress($"{student.Name} {student.Surname}", $"{student.Email.Trim()}"));
            message.Subject = "REQUESTED PDF";
            message.Body = new TextPart("plain")
            {
                Text = $"Book title: {Items[e.Position].Title}" +
                $" Download Url: {Items[e.Position].ImageUrl}",
            };
            
            using(var client = new MailKit.Net.Smtp.SmtpClient())
            {
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                client.Connect("smtp.gmail.com", 587);
                client.Authenticate("sigauquetk@gmail.com", "22147674");
                await client.SendAsync(message);
            };
            AndHUD.Shared.ShowSuccess(context, "Soft copy has been sent to your email", MaskType.Clear, TimeSpan.FromSeconds(3));

        }

        private void WebClient_UploadValuesCompleted(object sender, UploadValuesCompletedEventArgs e)
        {
            var results = Encoding.UTF8.GetString(e.Result);
            Android.Widget.Toast.MakeText(context, results, Android.Widget.ToastLength.Long).Show();
        }

        private void Adapter_ItemClick(object sender, ListingAdapterClickEventArgs e)
        {
            if (Items[e.Position].FileType == "IMG")
            {
                ViewBookDlgFragment dlg = new ViewBookDlgFragment(Items[e.Position].Id);
                dlg.Show(ChildFragmentManager.BeginTransaction(), null);
            }
           
        }

        private void Fab_add_book_Click(object sender, EventArgs e)
        {
            PopupMenu popupMenu = new PopupMenu(context, fab_add_book);
            popupMenu.Menu.Add(IMenu.None, 0, 1, "SELL BOOK");
            popupMenu.Menu.Add(IMenu.None, 1, 1, "UPLOAD SOFTCOPY");
            popupMenu.Show();
            popupMenu.MenuItemClick += (e, x) =>
            {
                if (x.Item.ItemId == 0)
                {
                    AddBookToListing dlg = new AddBookToListing(x.Item.ItemId);
                    dlg.Show(ChildFragmentManager.BeginTransaction(), "");
                }
                if (x.Item.ItemId == 1)
                {
                    AddBookToListing dlg = new AddBookToListing(x.Item.ItemId);
                    dlg.Show(ChildFragmentManager.BeginTransaction(), "");
                }
            };

            
        }
    }
 
}