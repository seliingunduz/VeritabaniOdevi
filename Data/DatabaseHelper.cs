using MySql.Data.MySqlClient;
using VeritabaniOdevi.Models;

namespace VeritabaniOdevi.Data
{
    public class DatabaseHelper
    {
        private string connectionString = "Server=127.0.0.1;Port=3306;Database=But;Uid=root;Pwd=1234;";

        public async Task<(int OgrenciID, bool IlkGiris)?> GirisYapAsync(string ogrenciNumarasi, string password)
        {
            try
            {
                using var connection = new MySqlConnection(connectionString);
                await connection.OpenAsync();

                string query = "SELECT OgrenciID, Sifre, Salt, IlkGiris FROM Ogrenciler WHERE OgrenciNumarasi = @OgrenciNumarasi";
                using var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@OgrenciNumarasi", ogrenciNumarasi);

                using var reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    string storedHash = reader["Sifre"]?.ToString() ?? string.Empty;
                    string storedSalt = reader["Salt"]?.ToString() ?? string.Empty;
                    bool ilkGiris = reader["IlkGiris"] != DBNull.Value && Convert.ToBoolean(reader["IlkGiris"]);

                    if (HashHelper.ValidateSha256(password, storedHash, storedSalt))
                    {
                        return (Convert.ToInt32(reader["OgrenciID"]), ilkGiris);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Veritabanı Hatası: {ex.Message}");
            }

            return null;
        }

        public async Task<bool> UpdateIlkGirisAsync(int ogrenciID)
        {
            try
            {
                using var connection = new MySqlConnection(connectionString);
                await connection.OpenAsync();

                string query = "UPDATE Ogrenciler SET IlkGiris = 0 WHERE OgrenciID = @OgrenciID";
                using var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@OgrenciID", ogrenciID);

                return await cmd.ExecuteNonQueryAsync() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"IlkGiris Güncelleme Hatası: {ex.Message}");
            }

            return false;
        }



        public async Task<bool> SifreDegistirAsync(int ogrenciId, string password)
        {
            try
            {
                using var connection = new MySqlConnection(connectionString);
                await connection.OpenAsync();

                // Salt ve hashed password oluştur
                string salt = HashHelper.GenerateSalt();
                string hashedPassword = HashHelper.HashSha256(password, salt);

                Console.WriteLine($"Salt: {salt}");
                Console.WriteLine($"Hashed Password: {hashedPassword}");

                string query = "UPDATE Ogrenciler SET Sifre = @Sifre, Salt = @Salt, IlkGiris = 0 WHERE OgrenciID = @OgrenciID";
                using var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Sifre", hashedPassword);
                cmd.Parameters.AddWithValue("@Salt", salt);
                cmd.Parameters.AddWithValue("@OgrenciID", ogrenciId);

                // Etkilenen satır sayısını kontrol et
                int affectedRows = await cmd.ExecuteNonQueryAsync();
                Console.WriteLine($"Güncellenen Satır Sayısı: {affectedRows}");
                return affectedRows > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata: {ex.Message}");
            }

            return false;
        }
        public async Task<List<SinavSonucu>> GetSinavSonuclariAsync(int ogrenciId)
        {
            var sonuclar = new List<SinavSonucu>();
            try
            {
                using var connection = new MySqlConnection(connectionString);
                await connection.OpenAsync();

                string query = @"
                SELECT s.SinavID, s.SinavTarihi
                FROM SinavSonuclari ss
                JOIN Sinavlar s ON ss.SinavID = s.SinavID
                WHERE ss.OgrenciID = @OgrenciID
                GROUP BY s.SinavID, s.SinavTarihi";

                using var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@OgrenciID", ogrenciId);

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    sonuclar.Add(new SinavSonucu
                    {
                        SinavID = Convert.ToInt32(reader["SinavID"]),
                        SinavTarihi = Convert.ToDateTime(reader["SinavTarihi"])
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata: {ex.Message}");
            }

            return sonuclar;
        }

        public async Task<List<SinavDetay>> GetSinavDetaylariAsync(int sinavId)
        {
            var detaylar = new List<SinavDetay>();
            try
            {
                using var connection = new MySqlConnection(connectionString);
                await connection.OpenAsync();

                string query = @"
                SELECT d.DersAdi, ad.AltDersAdi, ss.DogruSayisi, ss.YanlisSayisi, ss.Net
                FROM SinavSonuclari ss
                JOIN Dersler d ON ss.DersID = d.DersID
                LEFT JOIN AltDersler ad ON ss.AltDersID = ad.AltDersID
                WHERE ss.SinavID = @SinavID";

                using var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@SinavID", sinavId);

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    detaylar.Add(new SinavDetay
                    {
                        DersAdi = reader["DersAdi"].ToString(),
                        AltDersAdi = reader["AltDersAdi"]?.ToString(),
                        DogruSayisi = Convert.ToInt32(reader["DogruSayisi"]),
                        YanlisSayisi = Convert.ToInt32(reader["YanlisSayisi"]),
                        Net = Convert.ToSingle(reader["Net"])
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata: {ex.Message}");
            }

            return detaylar;
        }
    }

}

    