using Android.Content.Res;
using RentFlixApp.CustomRenderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(DesignEntry), typeof(RentFlixApp.Droid.DesignEntryRenderer))]
namespace RentFlixApp.Droid
{
#pragma warning disable CS0618 // Type or member is obsolete
    public class DesignEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                Control.SetHintTextColor(ColorStateList.ValueOf(Android.Graphics.Color.White));
             }
        }
    }
#pragma warning restore CS0618 // Type or member is obsolete
}