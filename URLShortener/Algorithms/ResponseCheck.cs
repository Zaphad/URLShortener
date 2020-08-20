using System;
using System.Net;

namespace URLShortener.Algorithms
{
    public class ResponseCheck
    {
        public static bool CheckOnResponseAsync(string url){
            if (url == null || !LinkResponseCheck(url)) {
                return false;
            }
            try
            {
                WebRequest.Create(new Uri(url)).GetResponse();
            }
            catch (Exception){
                return false;
            }
            return true;
        }
        public static bool LinkResponseCheck(string url)
        {
            Uri uriResult;
            return Uri.TryCreate(url, UriKind.Absolute, out uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }
    }
}