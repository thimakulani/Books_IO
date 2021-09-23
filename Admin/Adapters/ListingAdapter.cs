using Admin.Models;
using Android.Views;
using AndroidX.RecyclerView.Widget;
using FFImageLoading;
using Google.Android.Material.ImageView;
using Google.Android.Material.TextView;
using System;
using System.Collections.Generic;

namespace Admin.Adapters
{
    class ListingAdapter : RecyclerView.Adapter
    {
        public event EventHandler<ListingAdapterClickEventArgs> ItemClick;
        public event EventHandler<ListingAdapterClickEventArgs> ItemLongClick;
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

            var vh = new ListingAdapterViewHolder(itemView, OnClick, OnLongClick);
            return vh;
        }

        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {

            // Replace the contents of the view with that element
            var holder = viewHolder as ListingAdapterViewHolder;
            holder.Txt_book_item_title.Text = items[position].Title;
            holder.Txt_book_item_author.Text = items[position].Author;
            holder.Txt_book_item_addition.Text = items[position].Edition;
            holder.Txt_book_item_price.Text = $"R {items[position].Price}";
            holder.Txt_book_item_status.Text = items[position].Status;

            ImageService.Instance
                .LoadUrl(items[position].ImageUrl)
                .Retry(3, 200)
                .DownSampleInDip(250, 250)
                .FadeAnimation(true, true, 300)
                .IntoAsync(holder.Img);
            if(items[position].Status == "Sold")
            {
                holder.ItemView.Enabled = false;
            }
        }

        public override int ItemCount => items.Count;

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


        public ListingAdapterViewHolder(View itemView, Action<ListingAdapterClickEventArgs> clickListener,
                            Action<ListingAdapterClickEventArgs> longClickListener) : base(itemView)
        {

            Img = ItemView.FindViewById<ShapeableImageView>(Resource.Id.book_item_image);
            Txt_book_item_title = ItemView.FindViewById<MaterialTextView>(Resource.Id.book_item_title);
            Txt_book_item_author = ItemView.FindViewById<MaterialTextView>(Resource.Id.book_item_author);
            Txt_book_item_addition = ItemView.FindViewById<MaterialTextView>(Resource.Id.book_item_addition);
            Txt_book_item_price = ItemView.FindViewById<MaterialTextView>(Resource.Id.book_item_price);
            Txt_book_item_status = ItemView.FindViewById<MaterialTextView>(Resource.Id.book_item_status);


            //TextView = v;
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