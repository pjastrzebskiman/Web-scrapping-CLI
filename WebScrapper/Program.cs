using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WebScrapper
{
    class Program
    {
        static void Main(string[] args)
        {
            GetHtmlAsync();
            Console.ReadLine();

        }

        private static async void GetHtmlAsync()
        {
            // POBRANIE PLIKU HTML I ZNALEZNIENIE LICZBY STRON ORAZ WYSWIETLENIE ILOSCI WYNIKÓW
            var url = "https://www.olx.pl/elektronika/gry-konsole/konsole/q-ps4/";
            var http_klient = new HttpClient();

            var html = await http_klient.GetStringAsync(url);

            var html_plik = new HtmlDocument();
            html_plik.LoadHtml(html);

            var max_page = html_plik.DocumentNode.Descendants("a")
                          .Where(node => node.GetAttributeValue("data-cy", "")
                          .Equals("page-link-last")).FirstOrDefault().InnerText.Trim();
            int licznik = int.Parse(max_page);

            Console.WriteLine(html_plik.DocumentNode.Descendants("p")
                .Where(node => node.GetAttributeValue("class","")
                .Equals("color-2")).FirstOrDefault().InnerText.Trim()+Environment.NewLine);

            // PETLA W CELU PRZESZUKANIA WSZYTSKICH STRON
            for (int i = 1; i < licznik+1; i++)
            {
                var url_all = "https://www.olx.pl/elektronika/gry-konsole/konsole/q-ps4/?page="+i;
                var html1 = await http_klient.GetStringAsync(url_all);
                var html_plik1 = new HtmlDocument();
                html_plik1.LoadHtml(html1);


                var Lista_produktow_html = html_plik1.DocumentNode.Descendants("table")
                    .Where(node => node.GetAttributeValue("id", "")
                    .Equals("offers_table")).ToList();
                var test = html_plik1.DocumentNode.Descendants("table")
                    .Where(node => node.Attributes.Contains("data-id")).ToList();
                // string[] ab=new string[100];
                //int i = 0;


                foreach (var a in test)
                {
                    Console.WriteLine(a.Descendants("h3")
                          .Where(node => node.GetAttributeValue("class", "")
                          .Equals("lheight22 margintop5")).FirstOrDefault().InnerText.Trim());

                    Console.WriteLine(a.Descendants("p")
                         .Where(node => node.GetAttributeValue("class", "")
                         .Equals("price")).FirstOrDefault().InnerText.Trim());

                    Console.WriteLine(a.Descendants("a").FirstOrDefault().GetAttributeValue("href", "") + Environment.NewLine);


                    /*  Console.WriteLine(a.Descendants("a")
                          .Where(node => node.GetAttributeValue("class","")
                          .Contains("{id")).FirstOrDefault().InnerText.Trim());*/



                    /* string b = a.Descendants("h3")
                          .Where(node => node.GetAttributeValue("class", "")
                          .Equals("lheight22 margintop5")).FirstOrDefault().InnerText.Trim();
                     ab[i] = ab+b;
                     i++;               !!!!!!!!!!!!!!!!!!!!!!!!!!!   do testu                          */
                };

            }
        }
    }
}
