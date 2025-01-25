namespace VeritabaniOdevi;
using VeritabaniOdevi.Data;

public partial class GirisSayfasi : ContentPage
{
    public GirisSayfasi()
    {
        InitializeComponent(); // XAML dosyas�n� ba�lar
    }

    private async void OnGirisYapClicked(object sender, EventArgs e)
    {
        string ogrenciNumarasi = OgrenciNumarasiEntry.Text?.Trim();
        string sifre = SifreEntry.Text?.Trim();

        if (string.IsNullOrEmpty(ogrenciNumarasi) || string.IsNullOrEmpty(sifre))
        {
            await DisplayAlert("Hata", "L�tfen t�m alanlar� doldurun.", "Tamam");
            return;
        }

        var dbHelper = new DatabaseHelper();
        try
        {
            var sonuc = await dbHelper.GirisYapAsync(ogrenciNumarasi, sifre);

            if (sonuc.HasValue)
            {
                var (ogrenciID, ilkGiris) = sonuc.Value;

                if (ilkGiris)
                {
                    await DisplayAlert("Bilgi", "�lk giri�iniz oldu�u i�in �ifre de�i�tirmeniz gerekiyor.", "Tamam");
                    await Navigation.PushAsync(new SifreDegistirmeSayfasi(ogrenciID));
                }
                else
                {
                    await Navigation.PushAsync(new AnaMenuSayfasi());
                }
            }
            else
            {
                await DisplayAlert("Hata", "Giri� bilgileri hatal�.", "Tamam");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Hata", $"Bir hata olu�tu: {ex.Message}", "Tamam");
        }
    }


}




