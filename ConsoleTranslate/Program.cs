using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace ConsoleTranslate
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            //Console.OutputEncoding = Encoding.UTF8;

            do
            {
                Console.WriteLine("Input text or key ENTER");
                Console.WriteLine("");
                Console.Write("> ");

                var text = Console.ReadLine();

                if (text.Length == 0) 
                {
                    text = (string)Clipboard.GetText();
                }
                    
                string langFrom = Regex.Match(text, "[a-zA-Z]+").Success ? "en" : "ru";
                string langTo = langFrom == "en" ? "ru" : "en";

                using (WebClient web = new WebClient())
                {
                    web.Credentials = CredentialCache.DefaultCredentials;
                    web.Proxy.Credentials = CredentialCache.DefaultCredentials;

                    string request = string.Format(@"http://mymemory.translated.net/api/get?q={0}&langpair={1}|{2}", HttpUtility.UrlEncode(text), langFrom, langTo);

                    string result = web.DownloadString(request);

                    result = Regex.Match(result, "translatedText\":\".*?\"", RegexOptions.IgnoreCase).Groups[0].Value.Replace("translatedText:", "");
                    result = result.TrimStart("translatedText:\"".ToCharArray()).TrimEnd("\"".ToCharArray());

                    Console.WriteLine("");
                    Console.WriteLine("> " + result);
                    Console.WriteLine("");
                }
            }
            while (true);
        }
    }
}
