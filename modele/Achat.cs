using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dancewave.modele
{
    public class Achat
    {
        public int Id {  get; set; }
        public DateTime DatePurchase { get; set; }
        public int Quantity { get; set; }
        public float UnitPrice { get; set; }
        public float TotalPrice { get; set; }
        public int IdArticle {  get; set; }
           public Achat (int id,DateTime dateTime, int quantity, float unitPrice, float totalPrice, int idArticle)
        {
            Id = id;
            DatePurchase = dateTime;
            Quantity = quantity;
            UnitPrice = unitPrice;
            TotalPrice = totalPrice;
            IdArticle = idArticle;

        }
        public override string ToString()
        {
            return Id+" "+TotalPrice+" "+IdArticle;
        }

    }
}
