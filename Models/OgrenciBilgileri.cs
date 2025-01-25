namespace VeritabaniOdevi.Models
{
    public class OgrenciBilgileri
    {
        public int OgrenciID { get; set; } // Veritabanındaki birincil anahtar (Primary Key)
        public string OgrenciNumarasi { get; set; } = string.Empty; // Öğrenci numarası (benzersiz kimlik)
        public string Sifre { get; set; } = string.Empty; // Şifre (hashlenmiş)
        public string Salt { get; set; } = string.Empty; // Salt (şifreleme için ek değer)
        public bool IlkGiris { get; set; } // İlk giriş kontrolü
    }
}
