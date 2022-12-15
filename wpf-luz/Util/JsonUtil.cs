using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wpf_luz.Models;

namespace wpf_luz.Util
{
    public class JsonUtil
    {
        public JsonUtil() { }

        public static ObservableCollection<Card> loadJSONCardDatabase()
        {
            using (StreamReader r = new StreamReader("../../../Database/database.json"))
            {
                ObservableCollection<Card> collection = new ObservableCollection<Card>();
                string json = r.ReadToEnd();
                collection = JsonConvert.DeserializeObject<ObservableCollection<Card>>(json);
                return collection;
            }
        }

        public static string returnJSON(Deck deck)
        {
            string json = JsonConvert.SerializeObject(deck);
            return json;
        }

        public static Deck loadCards(string cards)
        {
            string json = cards;
            Deck collection = new Deck();
            collection = JsonConvert.DeserializeObject<Deck>(json);
            return collection;
        }
    }
}
