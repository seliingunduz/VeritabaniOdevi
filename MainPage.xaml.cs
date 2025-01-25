namespace VeritabaniOdevi
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnGirisYapClicked(object sender, EventArgs e)
        {
            // Giriş sayfasına yönlendirme
            await Navigation.PushAsync(new GirisSayfasi());
        }


    }
}
