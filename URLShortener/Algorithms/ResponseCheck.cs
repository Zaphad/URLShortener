using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace URLShortener.Algorithms
{
    public class ResponseCheck
    {
        [Obsolete("CheckOnResponseAsync is deprecated, please use CheckOnResponse instead.")]
        public static async Task<bool> CheckOnResponseAsync(string url){
            if (url == null || !LinkResponseCheck(url)) {
                return false;
            }
            HttpClient client = new HttpClient();
            var checkingResponse = await client.GetAsync(url);
            if (!checkingResponse.IsSuccessStatusCode)
            {
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