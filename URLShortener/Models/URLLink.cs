using System;

namespace URLShortener.Models
{
    public class URLLink
    {
        public int ID { get; set; }
        public string Token { get; set; }
        public string LongUrl { get; set; }
        public string ShortenedUrl { get; set; }
        public DateTime CreationDate { get; set; }
        public int LinkFollowCount {get; set;}
    }
}
