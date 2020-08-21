using System;
using System.Net;

namespace URLShortener.Algorithms
{
    public class ResponseCheck
    {
        public static bool CheckOnResponseAsync(string url){
            try
            {
                WebRequest.Create(new Uri(url)).GetResponse();
            }
            catch (Exception){
                return false;
            }
            return true;
        }
    }
}