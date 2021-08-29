using Admin.Models;
using Android.Views;
using AndroidX.RecyclerView.Widget;
using FFImageLoading;
using Google.Android.Material.Button;
using Google.Android.Material.ImageView;
using Google.Android.Material.TextView;
using Plugin.CloudFirestore;
using System;
using System.Collections.Generic;

namespace Admin.Adapters
{
    class ReserveAdapter : RecyclerView.Adapter
    {
        public event EventHandler<ReserveAdapterClickEventArgs> ItemClick;
        public event EventHandler<ReserveAdapterClickEventArgs> BtnClick;
        public event EventHandler<ReserveAdapterClickEventArgs> ItemLongClick;
        private readonly List<string> items = new List<string>();

        public ReserveAdapter(List<string> data)
        {
            items = data;
        }

        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {

            //Setup your layout here
            View itemView = null;
            var id = Resource.Layout.book_reserve_item;
            itemView = LayoutInflater.From(parent.Context).
                   Inflate(id, parent, false);

            var vh = new ReserveAdapterViewHolder(itemView, OnClick, OnLongClick, OnBtnClick);
            return vh;
        }

        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {

            var holder = viewHolder as ReserveAdapterViewHolder;
            CrossCloudFirestore
                .Current
                .Instance
                .Collection("BooksListings")
                .Document(items[position])
                .AddSnapshotListener((snapshot, error) =>
                {
                    if (snapshot.Exists)
                    {
                        var book = snapshot.ToObject<Books>();
                        holder.Txt_book_item_title.Text = book.Title;
                        holder.Txt_book_item_author.Text = book.Author;
                        holder.Txt_book_item_addition.Text = book.Edition;
                        holder.Txt_book_item_price.Text = book.Price;

                        ImageService.Instance
                            .LoadUrl(book.ImageUrl)
                            .Retry(3, 200)
                            .DownSampleInDip(250, 250)
                            .FadeAnimation(true, true, 300)
                            .IntoAsync(holder.Img);
                    }
                });
        }

        public override int ItemCount => items.Count;

        void OnBtnClick(ReserveAdapterClickEventArgs args) => BtnClick?.Invoke(this, args);
        void OnClick(ReserveAdapterClickEventArgs args) => ItemClick?.Invoke(this, args);
        void OnLongClick(ReserveAdapterClickEventArgs args) => ItemLongClick?.Invoke(this, args);

    }

    public class ReserveAdapterViewHolder : RecyclerView.ViewHolder
    {
        public ShapeableImageView Img { get; set; }
        public MaterialTextView Txt_book_item_title { get; set; }
        public MaterialTextView Txt_book_item_author { get; set; }
        public MaterialTextView Txt_book_item_addition { get; set; }
        public MaterialTextView Txt_book_item_price { get; set; }
        public MaterialButton BtnRemoveBook { get; set; }


        public ReserveAdapterViewHolder(View itemView, Action<ReserveAdapterClickEventArgs> clickListener,
                            Action<ReserveAdapterClickEventArgs> longClickListener, Action<ReserveAdapterClickEventArgs> btnClickListener) : base(itemView)
        {
            //TextView = v;
            Img = ItemView.FindViewById<ShapeableImageView>(Resource.Id.reserve_book_item_image); 
            Txt_book_item_title = ItemView.FindViewById<MaterialTextView>(Resource.Id.reserve_book_item_title);
            Txt_book_item_author = ItemView.FindViewById<MaterialTextView>(Resource.Id.reserve_book_item_author);
            Txt_book_item_addition = ItemView.FindViewById<MaterialTextView>(Resource.Id.reserve_book_item_addition);
            Txt_book_item_price = ItemView.FindViewById<MaterialTextView>(Resource.Id.reserve_book_item_price);
            BtnRemoveBook = ItemView.FindViewById<MaterialButton>(Resource.Id.btn_remove_book);

            BtnRemoveBook.Click += (sender, e) => btnClickListener(new ReserveAdapterClickEventArgs { View = itemView, Position = AbsoluteAdapterPosition });
            itemView.Click += (sender, e) => clickListener(new ReserveAdapterClickEventArgs { View = itemView, Position = AbsoluteAdapterPosition });
            itemView.LongClick += (sender, e) => longClickListener(new ReserveAdapterClickEventArgs { View = itemView, Position = AbsoluteAdapterPosition });
        }
    }

    public class ReserveAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}