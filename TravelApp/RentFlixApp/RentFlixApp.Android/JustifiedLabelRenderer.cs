using RentFlixApp.CustomRenderers;
using RentFlixApp.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(JustifiedLabel), typeof(JustifiedLabelRenderer))]
namespace RentFlixApp.Droid
{
    //We don't extend from LabelRenderer on purpose as we want to set 
    // our own native control (which is not TextView)
#pragma warning disable CS0618 // Type or member is obsolete
    public class JustifiedLabelRenderer : ViewRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            base.OnElementChanged(e);

            //if we have a new forms element, we want to update text with font style (as specified in forms-pcl) on native control
            if (e.NewElement != null)
            {
                if (Control == null)
                {
                    //register webview as native control
                    var webView = new Android.Webkit.WebView(Context);
                    webView.VerticalScrollBarEnabled = false;
                    webView.HorizontalScrollBarEnabled = false;

                    webView.LoadData("<html><body>&nbsp;</body></html>", "text/html; charset=utf-8", "utf-8");
                    SetNativeControl(webView);
                }

                //if we have a new forms element, we want to update text with font style (as specified in forms-pcl) on native control
                UpdateTextOnControl();
            }
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            //if there is change in text or font-style, trigger update to redraw control
            if (e.PropertyName == nameof(Label.Text)
               || e.PropertyName == nameof(Label.FontFamily)
               || e.PropertyName == nameof(Label.FontSize)
               || e.PropertyName == nameof(Label.TextColor)
               || e.PropertyName == nameof(Label.FontAttributes))
            {
                UpdateTextOnControl();
            }
        }

        void UpdateTextOnControl()
        {
            var webView = Control as Android.Webkit.WebView;

            // create css style from font-style as specified
            if (Element is Label formsLabel)
            {
                var cssStyle = $"margin: 0px; padding: 0px; text-align: justify; color: {ToHexColor(formsLabel.TextColor)}; background-color: {ToHexColor(formsLabel.BackgroundColor)}; font-family: {formsLabel.FontFamily}; font-size: {formsLabel.FontSize}; font-weight: {formsLabel.FontAttributes}";

                // apply that to text 
                var strData =
                    $"<html><body style=\"{cssStyle}\">{formsLabel.Text}</body></html>";

                // and, refresh webview
                webView?.LoadData(strData, "text/html; charset=utf-8", "utf-8");
            }

            webView?.Reload();
        }

        // helper method to convert forms-color to css-color
        string ToHexColor(Color color)
        {
            var red = (int)(color.R * 255);
            var green = (int)(color.G * 255);
            var blue = (int)(color.B * 255);
            var hex = $"#{red:X2}{green:X2}{blue:X2}";

            return hex;
        }
    }
#pragma warning restore CS0618 // Type or member is obsolete
}