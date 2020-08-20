using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using URLShortener.Models;

namespace URLShortener.Algorithms
{
    public  class Shortener
    {
        public string Token { get; set; }
        private static IHttpContextAccessor _httpContextAccessor;
        public Shortener(IHttpContextAccessor httpContextAccessor) {
            _httpContextAccessor = httpContextAccessor;
        }
        public static string BaseUrl()
        {
            var request = _httpContextAccessor.HttpContext.Request;
            string host = request.Host.ToUriComponent();
            return $"{request.Scheme}://{host}/";
        }
        private  Shortener TokenGenerator() {
            string urlsafe = string.Empty;
            Enumerable.Range(48, 75)
              .Where(i => i < 58 || i > 64 && i < 91 || i > 96)
              .OrderBy(o => new Random().Next())
              .ToList()
              .ForEach(i => urlsafe += Convert.ToChar(i));
            Token = urlsafe.Substring(new Random().Next(0, urlsafe.Length-6), new Random().Next(2, 6));
            return this;
        }
        public URLLink ShortenedUrl(string url, URLLinkContext context, bool skipLinkCheck) {
            if (!skipLinkCheck) { 
                foreach (URLLink u in context.URLLinks) 
                    if (u.LongUrl == url) 
                        return null;
            }
            TokenGenerator();
            foreach (URLLink u in context.URLLinks)
                if (u.Token == TokenGenerator().Token)
                    return null;

            URLLink urlLink = new URLLink { LongUrl = url,
                Token = Token,
                ShortenedUrl = BaseUrl() + Token, 
                CreationDate = DateTime.Now, 
                LinkFollowCount = 0 };
            return urlLink;
        }
    }
}