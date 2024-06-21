using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log_in.modele
{
    public class Sale
    {
        public int Id { get; set; }
        public DateTime DateSale { get; set; }
        public int QuantitySale { get; set; }
        public float UnitPrice { get; set; }
        public float TotalPrice { get; set; }
        public int IdArticle { get; set; }

        public Sale( int id, DateTime dateSale, int quantitySale, float unitPrice, float totalPrice, int idArticle)
        {
            Id = id;
            DateSale = dateSale;
            QuantitySale = quantitySale;
            UnitPrice = unitPrice;
            TotalPrice = totalPrice;
            IdArticle = idArticle;
        }
        public override string ToString()
        {
            return Id+" "+ DateSale+" "+ UnitPrice+" "+ TotalPrice;
        }
    }

}
