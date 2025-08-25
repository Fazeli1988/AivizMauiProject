using AivizMauiProject.Features.Exercise3.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Text.Json;
using System.Windows.Input;
using System.Runtime.CompilerServices;

namespace AivizMauiProject.Features.Exercise3.ViewModels
{
    public class Exercise3ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        // Collection of rectangles
        public ObservableCollection<RectangleInfo> Rectangles { get; } = new();

        // Html content for WebView binding
        private HtmlWebViewSource _webViewSource = new();
        public HtmlWebViewSource WebViewSource
        {
            get => _webViewSource;
            set => SetProperty(ref _webViewSource, value);
        }

        // Commands
        public ICommand SelectImageCommand { get; }
        public ICommand ClearRectanglesCommand { get; }

        public Exercise3ViewModel()
        {
            SelectImageCommand = new Command(async () => await OnSelectImageAsync());
            ClearRectanglesCommand = new Command(OnClearRectangles);

            // Start with a blank white image
            LoadBlankImage();
        }

        private void OnClearRectangles()
        {
            Rectangles.Clear();
            // Tell WebView to clear rectangles
            InvokeJsClearRects?.Invoke();
        }

        private async Task OnSelectImageAsync()
        {
            try
            {
                var file = await FilePicker.PickAsync(new PickOptions
                {
                    PickerTitle = "Select an image",
                    FileTypes = FilePickerFileType.Images
                });

                if (file == null)
                    return;

                await using var fs = await file.OpenReadAsync();
                using var ms = new MemoryStream();
                await fs.CopyToAsync(ms);
                var bytes = ms.ToArray();

                var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
                var mime = ext switch
                {
                    ".jpg" or ".jpeg" => "image/jpeg",
                    ".png" => "image/png",
                    ".gif" => "image/gif",
                    ".bmp" => "image/bmp",
                    _ => "image/png"
                };

                var dataUrl = $"data:{mime};base64,{Convert.ToBase64String(bytes)}";

                // Tell WebView to set the new image
                InvokeJsSetImage?.Invoke(EscapeForJs(dataUrl));

                // Clear the rectangles list simultaneously
                Rectangles.Clear();
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private void LoadBlankImage()
        {
            var blankDataUrl = CreateBlankDataUrl();
            LoadHtmlWithImage(blankDataUrl);
        }

        // Load HTML with placeholder image
        public async void LoadHtmlWithImage(string imageDataUrl)
        {
            try
            {
                using var stream = await FileSystem.OpenAppPackageFileAsync("draw.html");
                using var reader = new StreamReader(stream, Encoding.UTF8);
                var html = await reader.ReadToEndAsync();
                html = html.Replace("{{IMG_SRC}}", imageDataUrl);
                WebViewSource = new HtmlWebViewSource { Html = html };
            }
            catch
            {
                // ignore
            }
        }

        // For sending JS commands to WebView
        public Action<string>? InvokeJsSetImage { get; set; }
        public Action? InvokeJsClearRects { get; set; }
        public Action<string>? InvokeJsExecute { get; set; }

        // When receiving data from JS
        public void UpdateRectanglesFromJson(string json)
        {
            try
            {
                var rects = JsonSerializer.Deserialize<List<RectangleInfo>>(Uri.UnescapeDataString(json));
                if (rects == null) return;
                Rectangles.Clear();
                foreach (var r in rects)
                    Rectangles.Add(r);
            }
            catch
            {
                // ignore
            }
        }

        // Create blank 1x1 white PNG as data URL
        private static string CreateBlankDataUrl()
        {
            const string base64 = "iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAQAAAC1HAwCAAAAC0lEQVR4nGNgYAAAAAMAASsJTYQAAAAASUVORK5CYII=";
            return $"data:image/png;base64,{base64}";
        }

        private static string EscapeForJs(string s)
        {
            return s.Replace("\\", "\\\\").Replace("'", "\\'");
        }

        // INotifyPropertyChanged helper
        protected void SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return;

            backingStore = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
