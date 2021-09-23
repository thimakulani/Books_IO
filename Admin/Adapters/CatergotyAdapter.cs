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
    class CatergotyAdapter : RecyclerView.Adapter
    {
        public event EventHandler<CatergotyAdapterClickEventArgs> ItemClick;
        public event EventHandler<CatergotyAdapterClickEventArgs> ItemLongClick;
        private readonly List<DistinctBooks> items = new List<DistinctBooks>();

        public CatergotyAdapter(List<DistinctBooks> data)
        {
            items = data;
        }

        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {

            //Setup your layout here
            View itemView = null;
            var id = Resource.Layout.row_category;
            itemView = LayoutInflater.From(parent.Context).
                   Inflate(id, parent, false);

            var vh = new CatergotyAdapterViewHolder(itemView, OnClick, OnLongClick);
            return vh;
        }

        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {

            // Replace the contents of the view with that element
            var holder = viewHolder as CatergotyAdapterViewHolder;
            holder.Txt_book_item_title.Text = items[position].Title;
            holder.Txt_book_item_count.Text = $"{items[position].Counter} Books left";
            holder.Txt_book_item_isbn.Text = items[position].ISBN;


        }

        public override int ItemCount => items.Count;

        void OnClick(CatergotyAdapterClickEventArgs args) => ItemClick?.Invoke(this, args);
        void OnLongClick(CatergotyAdapterClickEventArgs args) => ItemLongClick?.Invoke(this, args);

    }

    public class CatergotyAdapterViewHolder : RecyclerView.ViewHolder
    {
        public MaterialTextView Txt_book_item_title { get; set; }
        public MaterialTextView Txt_book_item_isbn { get; set; }
        public MaterialTextView Txt_book_item_count { get; set; }
 


        public CatergotyAdapterViewHolder(View itemView, Action<CatergotyAdapterClickEventArgs> clickListener,
                            Action<CatergotyAdapterClickEventArgs> longClickListener) : base(itemView)
        {

            Txt_book_item_title = ItemView.FindViewById<MaterialTextView>(Resource.Id.row_reserve_book_item_title);
            Txt_book_item_isbn = ItemView.FindViewById<MaterialTextView>(Resource.Id.row_reserve_book_item_isbn);
            Txt_book_item_count = ItemView.FindViewById<MaterialTextView>(Resource.Id.row_reserve_book_item_count);


            //TextView = v;
            itemView.Click += (sender, e) => clickListener(new CatergotyAdapterClickEventArgs { View = itemView, Position = AbsoluteAdapterPosition });
            itemView.LongClick += (sender, e) => longClickListener(new CatergotyAdapterClickEventArgs { View = itemView, Position = AbsoluteAdapterPosition });
        }
    }

    public class CatergotyAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}