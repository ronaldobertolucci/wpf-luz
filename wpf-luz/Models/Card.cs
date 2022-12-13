using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpf_luz.Models
{
    public class Card
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Set { get; set; }
        public string Set_Type { get; set; }
        public string Mana_Cost { get; set; }
        public string Oracle_Text { get; set; }
        public string Type_Line { get; set; }
        public string Power { get; set; }
        public string Toughness { get; set; }
        public string Loyalty { get; set; }

        public Card() { }
        public Card(Card card) // copy
        {
            Id = card.Id;
            Name = card.Name;
            Set = card.Set;
            Set_Type = card.Set_Type;
            Mana_Cost = card.Mana_Cost;
            Oracle_Text = card.Oracle_Text;
            Type_Line = card.Type_Line;
            Power = card.Power;
            Toughness = card.Toughness;
            Loyalty = card.Loyalty;
        }
        public Card(string id, string name, string set, string set_type,
                    string manaCost, string oracleText, string typeLine,
                    string power, string toughness, string loyalty)
        {
            Id = id;
            Name = name;
            Set = set;
            Set_Type = set_type;
            Mana_Cost = manaCost;
            Oracle_Text = oracleText;
            Type_Line = typeLine;
            Power = power;
            Toughness = toughness;
            Loyalty = loyalty;
        }
    }
}
