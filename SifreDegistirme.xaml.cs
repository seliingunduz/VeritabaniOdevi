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
                await DisplayAlert("Hata", "L�tfen t�m alanlar� doldurun.", "Tamam");
                return;
            }

            if (yeniSifre != yeniSifreTekrar)
            {
                await DisplayAlert("Hata", "�ifreler uyu�muyor.", "Tamam");
                return;
            }

            var dbHelper = new DatabaseHelper();
            bool result = await dbHelper.SifreDegistirAsync(ogrenciId, yeniSifre);

            if (result)
            {
                await DisplayAlert("Ba�ar�l�", "�ifreniz ba�ar�yla de�i�tirildi.", "Tamam");
                await Navigation.PushAsync(new AnaMenuSayfasi());
            }
            else
            {
                await DisplayAlert("Hata", "�ifre de�i�tirilemedi. L�tfen tekrar deneyin.", "Tamam");
            }
        }


    }
}
