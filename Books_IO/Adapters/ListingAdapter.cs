using Android.Views;
using AndroidX.RecyclerView.Widget;
using Books_IO.Models;
using FFImageLoading;
using Firebase.Auth;
using Google.Android.Material.Button;
using Google.Android.Material.ImageView;
using Google.Android.Material.TextView;
using System;
using System.Collections.Generic;

namespace Books_IO.Adapters
{
    class ListingAdapter : RecyclerView.Adapter
    {
        public event EventHandler<ListingAdapterClickEventArgs> ItemClick;
        public event EventHandler<ListingAdapterClickEventArgs> ItemLongClick;
        public event EventHandler<ListingAdapterClickEventArgs> DownloadClick;
        private readonly List<Books> items = new List<Books>();

        public ListingAdapter(List<Books> data)
        {
            items = data;
        }

        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {

            //Setup your layout here
            View itemView = null;
            var id = Resource.Layout.row_books_items;
            itemView = LayoutInflater.From(parent.Context).
                   Inflate(id, parent, false);

            var vh = new ListingAdapterViewHolder(itemView, OnClick, OnLongClick, OnDownloadClick);
            return vh;
        }

        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {

            var holder = viewHolder as ListingAdapterViewHolder;
            holder.Txt_book_item_title.Text = items[position].Title;
            holder.Txt_book_item_author.Text = items[position].Author;
            holder.Txt_book_item_addition.Text = items[position].Edition;
            holder.Txt_book_item_price.Text = $"R{items[position].Price}";
            holder.Txt_book_item_status.Text = items[position].Status;


            if(items[position].Student_Id == FirebaseAuth.Instance.Uid)
            {
                holder.ItemView.Enabled = false;
            }
            if(items[position].FileType == "IMG")
            {
                holder.Btn_download_pdf.Visibility = ViewStates.Gone;
                if (!string.IsNullOrEmpty(items[position].ImageUrl))
                {
                    ImageService.Instance
                        .LoadUrl(items[position].ImageUrl)
                        .Retry(3, 200)
                        .DownSampleInDip(250, 250)
                        .FadeAnimation(true, true, 300)
                        .IntoAsync(holder.Img);
                }
            }
            else if(items[position].FileType == "PDF")
            {
                holder.Btn_download_pdf.Visibility = ViewStates.Visible;
                holder.Img.Visibility = ViewStates.Gone;
                holder.Txt_book_item_status.Visibility = ViewStates.Gone;
                holder.Txt_book_item_price.Visibility = ViewStates.Gone;

            }
            else
            {
                holder.Btn_download_pdf.Visibility = ViewStates.Gone;
                holder.Img.Visibility = ViewStates.Gone;
            }

            
        }

        public override int ItemCount => items.Count;

        void OnDownloadClick(ListingAdapterClickEventArgs args) => DownloadClick?.Invoke(this, args);
        void OnClick(ListingAdapterClickEventArgs args) => ItemClick?.Invoke(this, args);
        void OnLongClick(ListingAdapterClickEventArgs args) => ItemLongClick?.Invoke(this, args);

    }

    public class ListingAdapterViewHolder : RecyclerView.ViewHolder
    {
        public ShapeableImageView Img { get; set; }
        public MaterialTextView Txt_book_item_title { get; set; }
        public MaterialTextView Txt_book_item_author { get; set; }
        public MaterialTextView Txt_book_item_addition { get; set; }
        public MaterialTextView Txt_book_item_price { get; set; }
        public MaterialTextView Txt_book_item_status { get; set; }
        public MaterialButton Btn_download_pdf { get; set; }


        public ListingAdapterViewHolder(View itemView, Action<ListingAdapterClickEventArgs> clickListener,
                            Action<ListingAdapterClickEventArgs> longClickListener, Action<ListingAdapterClickEventArgs> longDownloadListener) : base(itemView)
        {
            //TextView = v;
            Btn_download_pdf = ItemView.FindViewById<MaterialButton>(Resource.Id.btn_download_pdf);
            Img = ItemView.FindViewById<ShapeableImageView>(Resource.Id.book_item_image);
            Txt_book_item_title = ItemView.FindViewById<MaterialTextView>(Resource.Id.book_item_title);
            Txt_book_item_author = ItemView.FindViewById<MaterialTextView>(Resource.Id.book_item_author);
            Txt_book_item_addition = ItemView.FindViewById<MaterialTextView>(Resource.Id.book_item_addition);
            Txt_book_item_price = ItemView.FindViewById<MaterialTextView>(Resource.Id.book_item_price);
            Txt_book_item_status = ItemView.FindViewById<MaterialTextView>(Resource.Id.book_item_status);
            Btn_download_pdf.Click += (sender, e) => longDownloadListener(new ListingAdapterClickEventArgs { View = itemView, Position = AbsoluteAdapterPosition });
            itemView.Click += (sender, e) => clickListener(new ListingAdapterClickEventArgs { View = itemView, Position = AbsoluteAdapterPosition });
            itemView.LongClick += (sender, e) => longClickListener(new ListingAdapterClickEventArgs { View = itemView, Position = AbsoluteAdapterPosition });
        }
    }

    public class ListingAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}