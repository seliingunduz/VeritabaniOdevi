using VeritabaniOdevi.Data;
namespace VeritabaniOdevi;

public partial class SinavDetaySayfasi : ContentPage
{
    public SinavDetaySayfasi(int sinavId)
    {
        InitializeComponent();
        LoadSinavDetaylari(sinavId);
    }

    private async void LoadSinavDetaylari(int sinavId)
    {
        try
        {
            var dbHelper = new DatabaseHelper();
            var detaylar = await dbHelper.GetSinavDetaylariAsync(sinavId);

            foreach (var detay in detaylar)
            {
                var grid = new Grid
                {
                    ColumnDefinitions =
                    {
                        new ColumnDefinition { Width = GridLength.Star },
                        new ColumnDefinition { Width = GridLength.Auto },
                        new ColumnDefinition { Width = GridLength.Auto }
                    },
                    RowDefinitions =
                    {
                        new RowDefinition { Height = GridLength.Auto }
                    },
                    RowSpacing = 10,
                    ColumnSpacing = 10,
                    Margin = new Thickness(0, 5)
                };

                grid.Add(new Label
                {
                    Text = detay.DersAdi,
                    FontAttributes = FontAttributes.Bold
                }, 0, 0);

                grid.Add(new Label
                {
                    Text = detay.DogruSayisi.ToString(),
                    HorizontalOptions = LayoutOptions.Center
                }, 1, 0);

                grid.Add(new Label
                {
                    Text = detay.YanlisSayisi.ToString(),
                    HorizontalOptions = LayoutOptions.Center
                }, 2, 0);

                SinavDetayContainer.Add(grid);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Hata", $"Sýnav detaylarý yüklenirken bir hata oluþtu: {ex.Message}", "Tamam");
        }
    }
}
