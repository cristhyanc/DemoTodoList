using Android.App;
using MvvmCross.Forms.Platforms.Android.Core;
using Xamarin.Forms;

#if DEBUG
[assembly: Application(Debuggable = true)]
#else
[assembly: Application(Debuggable = false)]
#endif

namespace DemoTodoList.Droid
{
    public class Setup : MvxFormsAndroidSetup<Core.App, UI.App>
    {
        public override Xamarin.Forms.Application FormsApplication
        {
            get
            {
                Xamarin.Forms.Forms.SetFlags("SwipeView_Experimental");
                return base.FormsApplication;
            }
        }
    }
}
