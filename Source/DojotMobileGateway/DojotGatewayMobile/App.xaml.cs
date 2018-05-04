using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DojotGatewayMobile.Data;
using Xamarin.Forms;

namespace DojotGatewayMobile
{
    public partial class App : Application
    {
        static LocalDatabase database;

        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage());
        }

        public static LocalDatabase Database
        {
            get
            {
                if (database == null)
                {
                    database = new LocalDatabase();
                }
                return database;
            }
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
