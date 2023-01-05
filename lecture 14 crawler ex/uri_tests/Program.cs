using System.Globalization;

public static class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("https://wwW.toros.edu.TR".NormalizeUrl());
        Console.WriteLine("https://wwW.toros.edu.TR/".NormalizeUrl());
        Console.WriteLine("https://wwW.toros.edu.TR/test asd".NormalizeUrl());
        Console.WriteLine("https://wwW.toros.edu.TR/test%20asd".NormalizeUrl());
        Console.WriteLine("https://wwW.toros.edu.TR/test%20dgg#top".NormalizeUrl());

        Uri _uri = new Uri("https://www.monstermmorpg.com/Jolteta-Monster-Dex-2");
        Console.WriteLine(_uri.Host);
        Console.WriteLine(_uri.Port);
        Console.WriteLine(_uri.Scheme);
        Console.WriteLine(_uri.PathAndQuery);
        Console.WriteLine(_uri.Query);
        Console.WriteLine(_uri.Fragment);
        Console.WriteLine(_uri.Authority);
        Console.WriteLine(_uri.LocalPath);
    }

    public static string NormalizeUrl(this string url)
    {
        url = DecodeUrlString(url);

        Uri myUri = new Uri(url);

        url = myUri.AbsoluteUri.ToString().ToLowerCaseEN();
        return url.Split('#').First();
    }

    private static string DecodeUrlString(this string url)
    {
        string newUrl;
        while ((newUrl = Uri.UnescapeDataString(url)) != url)
            url = newUrl;
        return newUrl;
    }

}

public static class ext
{
    private readonly static CultureInfo enCulture = new CultureInfo("en-US");
    public static string ToLowerCaseEN(this string str)
    {
        return str.ToLower(enCulture);
    }
}