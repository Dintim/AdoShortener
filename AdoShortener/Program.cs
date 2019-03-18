using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoShortener
{
    public class UrlDto
    {        
        public int Id { get; set; }
        public string FullUrl { get; set; }
        public string ShortUrl { get; set; }
        public int Limit { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ExpDate { get; set; }

        public UrlDto()
        {
            Limit = 5;
            CreateDate = DateTime.Now;
            ExpDate = DateTime.Now.AddDays(10);
        }
    }

    public class UrlRepository
    {
        string connectionString = @"Provider=SQLOLEDB.1;Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=UrlDB;Data Source=DM-ПК\SQLEXPRESS";

        public void AddUrl()
        {
            string fullUrl = string.Empty;
            string shortUrl = "https://www.shorturl.at/";
            Console.Write("Введите Url, который надо сократить: ");
            fullUrl = Console.ReadLine();

            shortUrl += RndUrl.RndString();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string executeSql = $"INSERT INTO [dbo].[Url]" +
               $"([FullUrl],[ShortUrl],[Limit],[CreateDate],[ExpDate])" +
               $"VALUES ('{fullUrl}', '{shortUrl}','{5}','{DateTime.Now}','{DateTime.Now.AddDays(10)}')";

                SqlCommand command = new SqlCommand(executeSql, connection);
                command.ExecuteNonQuery();
            }
            Console.WriteLine($"Короткая Url сформирована: {shortUrl}. Срок действия до {DateTime.Now.AddDays(10)}");            
        }



    }

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
    


    class Program
    {
        static void Main(string[] args)
        {
            
        }

        
    }
}
