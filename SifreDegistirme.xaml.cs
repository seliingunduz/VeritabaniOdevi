using VeritabaniOdevi.Data;

namespace VeritabaniOdevi
{
    public partial class SifreDegistirmeSayfasi : ContentPage
    {
        private int ogrenciId;

        public SifreDegistirmeSayfasi(int ogrenciId)
        {
            InitializeComponent(); 
            this.ogrenciId = ogrenciId;
        }

        private async void OnSifreDegistirClicked(object sender, EventArgs e)
        {
            string yeniSifre = YeniSifreEntry.Text?.Trim();
            string yeniSifreTekrar = YeniSifreTekrarEntry.Text?.Trim();

            if (string.IsNullOrEmpty(yeniSifre) || string.IsNullOrEmpty(yeniSifreTekrar))
            {
                await DisplayAlert("Hata", "Lütfen tüm alanlarý doldurun.", "Tamam");
                return;
            }

            if (yeniSifre != yeniSifreTekrar)
            {
                await DisplayAlert("Hata", "Þifreler uyuþmuyor.", "Tamam");
                return;
            }

            var dbHelper = new DatabaseHelper();
            bool result = await dbHelper.SifreDegistirAsync(ogrenciId, yeniSifre);

            if (result)
            {
                await DisplayAlert("Baþarýlý", "Þifreniz baþarýyla deðiþtirildi.", "Tamam");
                await Navigation.PushAsync(new AnaMenuSayfasi());
            }
            else
            {
                await DisplayAlert("Hata", "Þifre deðiþtirilemedi. Lütfen tekrar deneyin.", "Tamam");
            }
        }


    }
}
