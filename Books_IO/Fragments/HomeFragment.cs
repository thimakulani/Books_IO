﻿using Android.OS;
using Android.Views;
using AndroidX.Fragment.App;
using Books_IO.Dialogs;
using Google.Android.Material.FloatingActionButton;
using System;

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
        private void ConnectViews(View view)
        {
            fab_add_book = view.FindViewById<ExtendedFloatingActionButton>(Resource.Id.fab_add_book);
            fab_add_book.Click += Fab_add_book_Click;
        }

        private void Fab_add_book_Click(object sender, EventArgs e)
        {
            AddBookToListing dlg = new AddBookToListing();
            dlg.Show(ChildFragmentManager.BeginTransaction(), "");
        }
    }
}