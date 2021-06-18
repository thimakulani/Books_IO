using Android.Views;
using AndroidX.RecyclerView.Widget;
using Books_IO.Models;
using Google.Android.Material.TextView;
using System;
using System.Collections.Generic;

namespace Books_IO.Adapters
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
            var item = items[position];

            // Replace the contents of the view with that element
            var holder = viewHolder as ListingAdapterViewHolder;
            //holder.TextView.Text = items[position];
        }

        public override int ItemCount => items.Count;

        void OnClick(ListingAdapterClickEventArgs args) => ItemClick?.Invoke(this, args);
        void OnLongClick(ListingAdapterClickEventArgs args) => ItemLongClick?.Invoke(this, args);

    }

    public class ListingAdapterViewHolder : RecyclerView.ViewHolder
    {
        public MaterialTextView TextView { get; set; }


        public ListingAdapterViewHolder(View itemView, Action<ListingAdapterClickEventArgs> clickListener,
                            Action<ListingAdapterClickEventArgs> longClickListener) : base(itemView)
        {
            //TextView = v;
            itemView.Click += (sender, e) => clickListener(new ListingAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
            itemView.LongClick += (sender, e) => longClickListener(new ListingAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
        }
    }

    public class ListingAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}