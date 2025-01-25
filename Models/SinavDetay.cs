namespace VeritabaniOdevi.Models;

public class SinavDetay
{
    public string DersAdi { get; set; } = string.Empty;
    public string AltDersAdi { get; set; } = string.Empty;
    public int DogruSayisi { get; set; }
    public int YanlisSayisi { get; set; }
    public float Net { get; set; }
}
