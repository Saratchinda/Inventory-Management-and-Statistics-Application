using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dancewave.modele
{
    public class Article
    {
        public int Id {  get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float PrixAchat { get; set; }
        public float PrixVente { get; set; }
        public string Gamme { get; set; }
        public string Famille { get; set; }
        public string Fournisseur { get; set; }
        public string Emplacement { get; set; }

        public Article(int id, string name, string description, float prixAchat, float prixVente, string gamme, string famille, string fournisseur, string emplacement)
        {
            Id = id;
            Name = name;
            Description = description;
            PrixAchat = prixAchat;
            PrixVente = prixVente;
            Gamme = gamme;
            Famille = famille;
            Fournisseur = fournisseur;
            Emplacement = emplacement;
        }
        public override string ToString()
        {
            return Id.ToString() + " " + Name;
        }
    }
}
