using System;
using System.Collections.Generic;
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
        
    }
}
