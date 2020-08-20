using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using URLShortener.Algorithms;
using URLShortener.Models;

namespace URLShortener.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private URLLinkContext _context;
        private Shortener shortener;
        public HomeController(ILogger<HomeController> logger, URLLinkContext context, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _context = context;
            shortener = new Shortener(httpContextAccessor);
        }
        public IActionResult Index()
        {
            return View(_context.URLLinks.ToList());
        }
        public IActionResult Edit(int? id,string longUrl,string submitButton) {
            if (id != null)
            {
                if (_context.URLLinks.Find(id) is null)
                {
                    return StatusCode(404);
                }
            }
            if (submitButton!=null) { 
                if (submitButton.Equals("changeCoreUrl")) {
                        if (longUrl != null)
                        {
                            if (!ResponseCheck.CheckOnResponseAsync(longUrl))
                            {
                                return RedirectToAction("Input", "Error");
                            }
                            foreach (URLLink u in _context.URLLinks) {
                                if (u.LongUrl == longUrl) {
                                    return View("Edit", _context.URLLinks.Find(id));
                                }
                            }
                            _context.URLLinks.Find(id).LongUrl = longUrl;
                            _context.SaveChanges();
                        }
                }
                if (submitButton.Equals("generateNewLink")) { 
                    URLLink urlLink;
                    if ((urlLink = shortener.ShortenedUrl(_context.URLLinks.Find(id).LongUrl, _context, true)) != null)
                    {
                        _context.URLLinks.Find(id).ShortenedUrl = urlLink.ShortenedUrl;
                        _context.URLLinks.Find(id).Token = urlLink.Token;
                        _context.URLLinks.Find(id).LinkFollowCount = urlLink.LinkFollowCount;
                        _context.SaveChanges();
                    }
                }
            }
            return View("Edit",_context.URLLinks.Find(id));
        }
        public IActionResult Create(string longUrl, string submitButton, int? id) {
            URLLink urlLink;
            if (longUrl != null)
            {
                if (!ResponseCheck.CheckOnResponseAsync(longUrl))
                {
                    return RedirectToAction("Input", "Error");
                }
                if ((urlLink = shortener.ShortenedUrl(longUrl, _context, false)) != null)
                {
                    _context.URLLinks.Add(urlLink);
                    _context.SaveChanges();
                }
                return View(urlLink);
            }
            if (id != null) { 
                if ((urlLink = shortener.ShortenedUrl(_context.URLLinks.Find(id).LongUrl, _context, true)) != null)
                {
                    _context.URLLinks.Find(id).ShortenedUrl = urlLink.ShortenedUrl;
                    _context.URLLinks.Find(id).Token = urlLink.Token;
                    _context.URLLinks.Find(id).LinkFollowCount = urlLink.LinkFollowCount;
                    _context.SaveChanges();
                }
                return View(_context.URLLinks.Find(id));
            }
            return View();
        }
        [HttpGet, Route("/{token}")]
        public IActionResult RedirectByShortUrl([FromRoute] string token)
        {
            string urlForRedirect = $"{Shortener.BaseUrl()}";
            foreach (URLLink u in _context.URLLinks) {
                if (u.Token == token) {
                    urlForRedirect = u.LongUrl;
                    u.LinkFollowCount++;
                }
            }
            _context.SaveChanges();
            return Redirect(urlForRedirect);
        }
        public IActionResult Delete(int? id) {
            if (id != null) {
                if (_context.URLLinks.Find(id) != null) { 
                    _context.URLLinks.Remove(_context.URLLinks.Find(id));
                    _context.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }
    }
}