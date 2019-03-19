using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoShortener
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();                

                string fullUrl = "https://stackoverflow.com/questions/23000524/ado-net-datetime-in-sqlcommand";
                string shortUrl = "http://bit.do/eMkPj";

                string insertUrl = $"INSERT INTO [dbo].[Url] ([FullUrl],[ShortUrl],[Limit],[CreateDate],[ExpDate])"+
                    $"VALUES ('{fullUrl}', '{shortUrl}', 5, GETDATE(), DATEADD(day, 5, GETDATE()))";

                SqlCommand command = new SqlCommand(insertUrl, connection);
                command.ExecuteNonQuery();
            }
        }
        
    }
}
