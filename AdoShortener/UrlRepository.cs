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

                string insertUrl = $"INSERT INTO [dbo].[Url] ([FullUrl],[ShortUrl],[Limit],[CreateDate],[ExpDate])" +
                    $"VALUES ('{fullUrl}', '{shortUrl}', 5, GETDATE(), DATEADD(day, 5, GETDATE()))";

                SqlCommand command = new SqlCommand(insertUrl, connection);
                command.ExecuteNonQuery();
            }
            message = $"Короткая ссылка сформирована: {shortUrl}. Дата действия до {DateTime.Now.ToShortDateString()}";
            return shortUrl;
        }

        public UrlDto GetFullUrl(string shortUrl, out string message)
        {
            UrlDto urlDto = new UrlDto();

            using (SqlConnection connection=new SqlConnection(connectionString))
            {
                connection.Open();

                string selectUrl = $"SELECT * FROM [dbo].[Url] WHERE [ShortUrl]='{shortUrl}'";
                SqlCommand command = new SqlCommand(selectUrl, connection);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        object id = reader.GetValue(0);
                        object fUrl = reader.GetValue(1);
                        object surl = reader.GetValue(2);
                        object limit = reader.GetValue(3);
                        object createDate = reader.GetValue(4);
                        object expDate = reader.GetValue(5);
                    }
                }
                else
                    message = "Url не найдена";


            }

            message = "";
            return urlDto;
        }
    }
}
