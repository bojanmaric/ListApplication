using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Popis
{
    public class Article
    {
        [BsonId] 
        public ObjectId Id { get; set; }
        public string barkod { get; set; }
        public double porez { get; set; }
        public string jedinica_mere { get; set; }
        public double cena { get; set; }
        public string naziv { get; set; }
        public double kolicina { get; set; } = 0;
        public string sifra { get; set; }
        public int vrsta_artikla { get; set; }
    }
}
