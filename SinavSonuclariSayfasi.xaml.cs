using VeritabaniOdevi.Data;
using VeritabaniOdevi.Models;
namespace VeritabaniOdevi;

public partial class SinavSonuclariSayfasi : ContentPage
{
    public SinavSonuclariSayfasi()
    {
        InitializeComponent();
        LoadSinavSonuclari();
    }

    private async void LoadSinavSonuclari()
    {
        try
        {
            int ogrenciId = Preferences.Get("ogrenciId", -1);
            if (ogrenciId == -1)
            {
                await DisplayAlert("Hata", "��renci kimli�i bulunamad�.", "Tamam");
                return;
            }

            var dbHelper = new DatabaseHelper();
            var sinavSonuclari = await dbHelper.GetSinavSonuclariAsync(ogrenciId);
            SinavSonuclariCollectionView.ItemsSource = sinavSonuclari;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Hata", $"S�nav sonu�lar� y�klenirken bir hata olu�tu: {ex.Message}", "Tamam");
        }
    }

    private async void OnSinavSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is SinavSonucu selectedSinav)
        {
            await Navigation.PushAsync(new SinavDetaySayfasi(selectedSinav.SinavID));
        }
    }
}
