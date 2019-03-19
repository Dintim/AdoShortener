using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoShortener
{
    public static class RndUrl
    {
        public static string RndString()
        {
            Random rnd = new Random();
            string s = string.Empty;
            for (int i = 0; i < 2; i++)
            {
                s += (char)rnd.Next(97, 122);
            }
            for (int i = 0; i < 3; i++)
            {
                s += (char)rnd.Next(65, 90);
            }
            return s;
        }
    }

    public class UrlRepository
    {
        string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public string GetShortUrl(string fullUrl, out string message)
        {            
            string shortUrl = string.Empty;
            string site = "https://www.shorturl.at/";            

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                while (true)
                {
                    shortUrl = site + RndUrl.RndString();
                    string selectUrl = $"SELECT * FROM [dbo].[Url] WHERE [ShortUrl]='{shortUrl}'";
                    SqlCommand selectCommand = new SqlCommand(selectUrl, connection);
                    SqlDataReader reader = selectCommand.ExecuteReader();
                    if (!reader.HasRows)
                        break;
                }                
            }

            using (SqlConnection connection=new SqlConnection(connectionString))
            {
                connection.Open();
                string insertUrl = $"INSERT INTO [dbo].[Url] ([FullUrl],[ShortUrl],[Limit],[CreateDate],[ExpDate])" +
                    $"VALUES ('{fullUrl}', '{shortUrl}', 5, GETDATE(), DATEADD(day, 5, GETDATE()))";

                SqlCommand command = new SqlCommand(insertUrl, connection);
                command.ExecuteNonQuery();
            }

            message = $"Короткая ссылка сформирована: {shortUrl}.\nВы можете воспользоваться ссылкой {5} раз\nДата действия до {DateTime.Now.AddDays(5).ToShortDateString()}";
            return shortUrl;
        }

        public string GetFullUrl(string shortUrl, out string message)
        {
            UrlDto urlDto = null;

            using (SqlConnection connection=new SqlConnection(connectionString))
            {
                connection.Open();

                string selectUrl = $"SELECT * FROM [dbo].[Url] WHERE [ShortUrl]='{shortUrl}'";
                SqlCommand command = new SqlCommand(selectUrl, connection);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    object id = reader.GetValue(0);
                    object fUrl = reader.GetValue(1);
                    object surl = reader.GetValue(2);
                    object limit = reader.GetValue(3);
                    object createDate = reader.GetValue(4);
                    object expDate = reader.GetValue(5);

                    urlDto = new UrlDto {
                        Id = Int32.Parse(id.ToString()),
                        FullUrl = fUrl.ToString(),
                        ShortUrl = surl.ToString(),
                        Limit = Int32.Parse(limit.ToString()),
                        CreateDate = DateTime.Parse(createDate.ToString()),
                        ExpDate = DateTime.Parse(expDate.ToString())
                        };                 
                }                             
            }

            if (urlDto != null)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    urlDto.Limit--;
                    string updateSql = $"UPDATE [dbo].[Url] SET [Limit] = {urlDto.Limit} WHERE [ShortUrl] = '{shortUrl}'";
                    SqlCommand updateCommand = new SqlCommand(updateSql, connection);
                    updateCommand.ExecuteNonQuery();

                    message = $"Получите длинную ссылку: {urlDto.FullUrl}.\nВы можете воспользоваться короткой url еще {urlDto.Limit} раза.\n" +
                    $"Срок действия короткой ссылки до {urlDto.ExpDate.ToShortDateString()}";
                }
            }
            else                
                message = "Url не найдена";

            return urlDto.FullUrl;
        }
    }
}
