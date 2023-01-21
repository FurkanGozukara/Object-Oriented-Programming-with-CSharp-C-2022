using Azure.Core;
using lecture_14_crawler_ex.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private static readonly SemaphoreSlim semph_returnRootDomainId = new SemaphoreSlim(1, 1);

        private static async Task<int> returnRootDomainId(this string srUrl)
        {
            //_lockRootAdd.EnterWriteLock();
            semph_returnRootDomainId.Wait();
            Debug.WriteLine("returnRootDomainId entered");
            try
            {
                using ExampleCrawlerContext _context = new ExampleCrawlerContext();
                string rootDomain = srUrl.NormalizeUrl().returnRootDomainUrl();
                var rootDomainHash = rootDomain.SHA256Hash();

                var result = await _context.RootDomains.Where(pr => pr.RootDomainUrlHash == rootDomainHash).FirstOrDefaultAsync().ConfigureAwait(false);
                if (result == null)
                {
                    RootDomains _RootDomain = new RootDomains();
                    _RootDomain.RootDomainUrlHash = rootDomainHash;

                    _context.Add(_RootDomain);
                    await _context.SaveChangesAsync().ConfigureAwait(false);

                    Task.Run(() => { addUrl(rootDomain); }); 
                }

                var result2 = await _context.RootDomains.Where(pr => pr.RootDomainUrlHash == rootDomainHash).FirstOrDefaultAsync().ConfigureAwait(false);
                _context.Dispose();
                return result2.RootDomainId;
            }
            finally
            {
                semph_returnRootDomainId.Release();
                //_lockRootAdd.ExitWriteLock();
                Debug.WriteLine("returnRootDomainId released");
            }
        }

        //use readerwritelock slim better

        private static readonly ReaderWriterLockSlim addUrlLock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);

        private static long addUrlCounter = 0;
        private static long addUrlCounter_called_To_Enter = 0;

        private static readonly SemaphoreSlim semph_addUrl = new SemaphoreSlim(1, 1);

        public static async Task<bool> addUrl(this string srUrl)
        {
            //addUrlLock.EnterWriteLock();

            Interlocked.Increment(ref addUrlCounter_called_To_Enter);
            Debug.WriteLine("called to enter " + Interlocked.Read(ref addUrlCounter_called_To_Enter));
            semph_addUrl.Wait();
            Interlocked.Increment(ref addUrlCounter);
            Debug.WriteLine("addUrl entered "+ Interlocked.Read(ref addUrlCounter));
          
            try
            {
                using ExampleCrawlerContext _context = new ExampleCrawlerContext();

                var finalUrl = srUrl.NormalizeUrl();
                Urls myUrl = new Urls();
                myUrl.UrlHash = finalUrl.SHA256Hash();

                var result = await _context.Urls.FindAsync(myUrl.UrlHash).ConfigureAwait(false);
                if (result != null)
                {
                    _context.Dispose();
                    return false;
                }

                myUrl.Url = srUrl;
                myUrl.ParentUrlHash = myUrl.UrlHash;

                myUrl.RootDomainId = await myUrl.Url.returnRootDomainId().ConfigureAwait(false);

                _context.Add(myUrl);
                await _context.SaveChangesAsync().ConfigureAwait(false);
                _context.Dispose();
                return true;
            }
            finally
            {
                //if (addUrlLock.IsWriteLockHeld)
                //    addUrlLock.ExitWriteLock();

                semph_addUrl.Release();
                Interlocked.Decrement(ref addUrlCounter);
                Debug.WriteLine("addUrl released " + Interlocked.Read(ref addUrlCounter));
                Interlocked.Decrement(ref addUrlCounter_called_To_Enter);
            }
        }
    }
}

