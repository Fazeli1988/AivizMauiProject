using AivizMauiProject.Features.Exercise3.ViewModels;

namespace AivizMauiProject.Features.Exercise3.Views;

public partial class Exercise3 : ContentPage
{
    private Exercise3ViewModel Vm => (Exercise3ViewModel)BindingContext;

    public Exercise3()
    {
        InitializeComponent();

        // Connect WebView JS interaction to ViewModel
        Vm.InvokeJsSetImage = async (dataUrl) =>
        {
            try
            {
                await AnnotatorWebView.EvaluateJavaScriptAsync($"setImage('{dataUrl}')");
                await AnnotatorWebView.EvaluateJavaScriptAsync("clearRects()");
            }
            catch
            {
                Vm.LoadHtmlWithImage(dataUrl);
            }
        };

        Vm.InvokeJsClearRects = async () =>
        {
            try
            {
                await AnnotatorWebView.EvaluateJavaScriptAsync("clearRects()");
            }
            catch
            {
                // ignore
            }
        };
    }

    private void AnnotatorWebView_Navigating(object sender, WebNavigatingEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(e.Url))
            return;

        if (!e.Url.StartsWith("app://", StringComparison.OrdinalIgnoreCase))
            return;

        e.Cancel = true;

        try
        {
            var uri = new Uri(e.Url);
            var query = uri.Query.TrimStart('?');
            var map = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (var part in query.Split('&', StringSplitOptions.RemoveEmptyEntries))
            {
                var kv = part.Split('=', 2);
                if (kv.Length == 2)
                    map[kv[0]] = Uri.UnescapeDataString(kv[1]);
            }

            if (map.TryGetValue("data", out var json) && !string.IsNullOrWhiteSpace(json))
            {
                Vm.UpdateRectanglesFromJson(json);
            }
        }
        catch
        {
            // ignore
        }
    }

    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);
        if (RectanglesCollectionView?.ItemsLayout is GridItemsLayout gridLayout)
        {
            int approxItemWidth = 160;
            int span = Math.Max(1, (int)(width / approxItemWidth));
            gridLayout.Span = span;
        }
    }
}
