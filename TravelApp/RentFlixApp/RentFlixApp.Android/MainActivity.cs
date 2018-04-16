
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using ImageCircle.Forms.Plugin.Droid;
using Xamarin.Forms;

namespace RentFlixApp.Droid
{
    [Activity(Label = "RentFlixApp", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            ImageCircleRenderer.Init();
            Window.AddFlags(WindowManagerFlags.TranslucentNavigation);
            this.Window.AddFlags(WindowManagerFlags.Fullscreen);
            //Window.SetBackgroundDrawableResource(Resource.Drawable.MyBackgroundPNG);
            SetStatusBarColor(Android.Graphics.Color.Transparent);
            LoadApplication(new App());
        }
    }
}

