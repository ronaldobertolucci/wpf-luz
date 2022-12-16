using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using wpf_luz.Models;

namespace wpf_luz.Util
{
    public class ScryfallAPI
    {
        private static readonly HttpClient client = new HttpClient();

        public static ObservableCollection<Card> loadAllCards()
        {
            ObservableCollection<Card> allCards = new ObservableCollection<Card>();
            string data = getData("https://api.scryfall.com/sets/");
            List<Set> sets = JsonUtil.loadCards<List<Set>>(data);


            for (int i = 0; i < 20; i++) //provisório
            {
                if (sets[i].Set_Type == "core" || sets[i].Set_Type == "expansion")
                {
                    if (sets[i].Card_Count != 0)
                    {
                        List<Card> cards = new List<Card>();
                        cards = loadSet(sets[i].Search_Uri);
                        cards.ForEach(item => allCards.Add(item));
                    }
                }
            }
            return allCards;
        }
        public static List<Card> loadSet(string data)
        {
            List<string> links = new List<string>();
            List<Card> cards = new List<Card>();
            links = getAllLinksBySet(links, data);
            for (int j = 0; j < links.Count; j++)
            {
                string json = getData(links[j]);
                List<Card> x = JsonConvert.DeserializeObject<List<Card>>(json).ToList();
                x.ForEach(item => cards.Add(item));
            }
            return cards;
        }
        public static string getData(string uri)
        {
            // retorna 'data' do json object
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");

            var response = client.GetAsync(uri).Result;
            if (response.IsSuccessStatusCode)
            {
                using (Stream stringTask = response.Content.ReadAsStream())
                {
                    using (StreamReader reader = new StreamReader(stringTask))
                    {
                        string json = reader.ReadToEnd();
                        string data = JObject.Parse(json)["data"].ToString();
                        Thread.Sleep(100);
                        return data;
                    }
                }
            }
            else
            {
                throw new Exception("Error establishing API connection.");
            }
        }
        public static List<string> getAllLinksBySet(List<string> uris, string uri)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");

            var response = client.GetAsync(uri).Result;
            try
            {
                using (Stream stringTask = response.Content.ReadAsStream())
                {
                    using (StreamReader reader = new StreamReader(stringTask))
                    {
                        string json = reader.ReadToEnd();
                        bool hasMore = false;
                        if (JObject.Parse(json)["has_more"] != null)
                            hasMore = (bool)JObject.Parse(json)["has_more"];

                        uris.Add(uri);
                        if (hasMore)
                        {
                            string next_page = JObject.Parse(json)["next_page"].ToString();
                            Thread.Sleep(75);
                            return getAllLinksBySet(uris, next_page);
                        }
                        Thread.Sleep(75);
                        return uris;
                    }
                }
            }
            catch
            {
                throw new Exception("Error establishing API connection.");
            }
        }
    }
}
