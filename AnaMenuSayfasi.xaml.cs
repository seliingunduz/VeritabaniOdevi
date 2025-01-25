namespace VeritabaniOdevi;

public partial class AnaMenuSayfasi : ContentPage
{
    public AnaMenuSayfasi()
    {
        InitializeComponent();
    }

    private async void OnSinavSonuclariClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new SinavSonuclariSayfasi());
    }

    private async void OnSifreDegistirClicked(object sender, EventArgs e)
    {
        int ogrenciId = Preferences.Get("ogrenciId", -1);
        if (ogrenciId != -1)
        {
            await Navigation.PushAsync(new SifreDegistirmeSayfasi(ogrenciId));
        }
        else
        {
            await DisplayAlert("Hata", "Öðrenci kimliði bulunamadý.", "Tamam");
        }
    }

    private async void OnCikisClicked(object sender, EventArgs e)
    {
        Preferences.Remove("ogrenciId");
        await Navigation.PushAsync(new GirisSayfasi());
    }
}
