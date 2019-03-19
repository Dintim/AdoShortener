using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AdoShortener
{
    public class UserService
    {
        UrlRepository repository = new UrlRepository();

        public void Start()
        {
            string message = string.Empty;

            while (true)
            {
                Console.Clear();
                Console.WriteLine("====================================");
                Console.WriteLine("Выберите действие:");
                Console.WriteLine("1. Получить короткий url");
                Console.WriteLine("2. Получить длинный url");
                Console.WriteLine("3. Выход");
                Console.Write(": ");
                int choice = Int32.Parse(Console.ReadLine());
                if (choice == 3)
                    break;
                else if (choice==1)
                {
                    Console.Clear();
                    Console.WriteLine("==================================");
                    Console.Write("Введите длинную url: ");
                    string fUrl = Console.ReadLine();
                    string sUrl = repository.GetShortUrl(fUrl, out message);
                    Console.WriteLine("==================================\n");
                    Console.WriteLine(message);
                    Thread.Sleep(2000);
                }
                else if (choice==2)
                {
                    Console.Clear();
                    Console.WriteLine("==================================");
                    Console.Write("Введите короткую url: ");
                    string sUrl = Console.ReadLine();
                    string fUrl = repository.GetFullUrl(sUrl, out message);
                    Console.WriteLine("==================================\n");
                    Console.WriteLine(message);
                    Thread.Sleep(2000);
                }                    
            }
        }
    }
}
