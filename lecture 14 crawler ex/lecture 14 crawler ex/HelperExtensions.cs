using Azure.Core;
using lecture_14_crawler_ex.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace lecture_14_crawler_ex
{
    public static class HelperExtensions
    {
        public static string SHA256Hash(this string text)
        {
            string source = text;
            using (SHA256 sha1Hash = SHA256.Create())
            {
                byte[] sourceBytes = Encoding.UTF8.GetBytes(source);
                byte[] hashBytes = sha1Hash.ComputeHash(sourceBytes);
                string hash = BitConverter.ToString(hashBytes).Replace("-", String.Empty);
                return hash;
            }
        }

        public static bool IsValidUrl(this string uriName)
        {
            Uri uriResult;
            bool result = Uri.TryCreate(uriName, UriKind.Absolute, out uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

            return result;
        }

        public static string NormalizeUrl(this string url)
        {
            url = DecodeUrlString(url);

            Uri myUri = new Uri(url);

            url = myUri.AbsoluteUri.ToString().ToLowerCaseEN();
            return url.Split('#').First();
        }

        public static string returnRootDomainUrl(this string url)
        {
            Uri uri = new Uri(url);
            return $"{uri.Scheme}://{uri.Host}";
        }

        private static string DecodeUrlString(this string url)
        {
            string newUrl;
            while ((newUrl = Uri.UnescapeDataString(url)) != url)
                url = newUrl;
            return newUrl;
        }

        private readonly static CultureInfo enCulture = new CultureInfo("en-US");

        public static string ToLowerCaseEN(this string str)
        {
            return str.ToLower(enCulture);
        }


        private static readonly ReaderWriterLockSlim _lockRootAdd = new ReaderWriterLockSlim();

        private static async Task<int> returnRootDomainId(this string srUrl)
        {

            _lockRootAdd.EnterWriteLock();
            try
            {

                using ExampleCrawlerContext _context = new ExampleCrawlerContext();
                string rootDomain = srUrl.NormalizeUrl().returnRootDomainUrl();
                var rootDomainHash = rootDomain.SHA256Hash();

                var result = await _context.RootDomains.Where(pr => pr.RootDomainUrlHash == rootDomainHash).FirstOrDefaultAsync();
                if (result == null)
                {
                    RootDomains _RootDomain = new RootDomains();
                    _RootDomain.RootDomainUrlHash = rootDomainHash;

                    _context.Add(_RootDomain);
                    await _context.SaveChangesAsync();

                    await addUrl(rootDomain);
                }

                var result2 = await _context.RootDomains.Where(pr => pr.RootDomainUrlHash == rootDomainHash).FirstOrDefaultAsync();
                return result2.RootDomainId;
            }
            finally
            {
                _lockRootAdd.ExitWriteLock();
            }
        }

        //use readerwritelock slim better

        private static readonly ReaderWriterLockSlim addUrlLock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);

        public static async Task<bool> addUrl(this string srUrl)
        {
            addUrlLock.EnterWriteLock();

            try
            {
                using ExampleCrawlerContext _context = new ExampleCrawlerContext();

                var finalUrl = srUrl.NormalizeUrl();
                Urls myUrl = new Urls();
                myUrl.UrlHash = finalUrl.SHA256Hash();

                var result = await _context.Urls.FindAsync(myUrl.UrlHash);
                if (result != null)
                {
                    return false;
                }

                myUrl.Url = srUrl;
                myUrl.ParentUrlHash = myUrl.UrlHash;

                myUrl.RootDomainId = await myUrl.Url.returnRootDomainId();

                _context.Add(myUrl);
             await   _context.SaveChangesAsync();
                return true;
            }
            finally
            {
                addUrlLock.ExitWriteLock();


            }
        }
    }
}

