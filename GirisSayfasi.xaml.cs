namespace VeritabaniOdevi;
using VeritabaniOdevi.Data;

public partial class GirisSayfasi : ContentPage
{
    public GirisSayfasi()
    {
        InitializeComponent(); // XAML dosyasýný baðlar
    }

    private async void OnGirisYapClicked(object sender, EventArgs e)
    {
        string ogrenciNumarasi = OgrenciNumarasiEntry.Text?.Trim();
        string sifre = SifreEntry.Text?.Trim();

        if (string.IsNullOrEmpty(ogrenciNumarasi) || string.IsNullOrEmpty(sifre))
        {
            await DisplayAlert("Hata", "Lütfen tüm alanlarý doldurun.", "Tamam");
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
                    await DisplayAlert("Bilgi", "Ýlk giriþiniz olduðu için þifre deðiþtirmeniz gerekiyor.", "Tamam");
                    await Navigation.PushAsync(new SifreDegistirmeSayfasi(ogrenciID));
                }
                else
                {
                    await Navigation.PushAsync(new AnaMenuSayfasi());
                }
            }
            else
            {
                await DisplayAlert("Hata", "Giriþ bilgileri hatalý.", "Tamam");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Hata", $"Bir hata oluþtu: {ex.Message}", "Tamam");
        }
    }


}




