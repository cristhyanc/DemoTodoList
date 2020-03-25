using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MvvmCross.Forms.Platforms.Android.Views;
using DemoTodoList.Core.ViewModels.Main;
using Xamarin.Forms;

namespace DemoTodoList.Droid
{
    [Activity(
        Theme = "@style/AppTheme")]
    public class MainActivity : MvxFormsAppCompatActivity<MainViewModel>
    {
        protected override void OnCreate(Bundle bundle)
        {
           
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            Acr.UserDialogs.UserDialogs.Init(this);
            MvvmCross.Mvx.IoCProvider.RegisterSingleton<Acr.UserDialogs.IUserDialogs>(Acr.UserDialogs.UserDialogs.Instance);
            
            base.OnCreate(bundle);
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
           

        }
             

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
