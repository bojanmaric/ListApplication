
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Popis
{
    public class DbWriter
    {
        private readonly IMongoCollection<Article> _articleCollection;

        public DbWriter()
        {
            var client =new MongoClient(Properties.Settings.Default.DbString);
            var database = client.GetDatabase(Properties.Settings.Default.DB);
            _articleCollection = database.GetCollection<Article>("Articles");
        }

        public void InsertArticle(Article article)
        {
            _articleCollection.InsertOne(article);
        }

        public void InsertArticlesFromJson(string jsonFilePath)
        {
            // Read the JSON file
            var jsonData = File.ReadAllText(jsonFilePath);

            // Deserialize JSON to a list of articles
            var articles = JsonConvert.DeserializeObject<List<Article>>(jsonData);

            // Insert articles into the database
            _articleCollection.InsertMany(articles);

            MessageBox.Show("Articles successfully inserted into the database.");
        }

        public (bool,string) IcreaseArticle(string barkod, double amountToAdd)
        {
            var filter = Builders<Article>.Filter.Eq(a => a.barkod, barkod);
            var update = Builders<Article>.Update.Inc(a => a.kolicina, amountToAdd); // Increment existing kolicina

            var result = _articleCollection.UpdateOne(filter, update);

            if (result.ModifiedCount > 0)
            {
                return (true, $"Dodato {amountToAdd} za Barkod: {barkod}.");
            }
            else
            {
                return (false,$"Nije pronadjen Barkod: {barkod}.Izmena neuspesna");
            }
        }
        public (bool,string) DecreaseArticle(string barkod, double amountToSubtract)
        {
            var filter = Builders<Article>.Filter.Eq(a => a.barkod, barkod);
            var update = Builders<Article>.Update.Inc(a => a.kolicina, -amountToSubtract); // Decrement existing kolicina

            var result = _articleCollection.UpdateOne(filter, update);

            if (result.ModifiedCount > 0)
            {
                return (true,$"Oduzeto {amountToSubtract} za Barkod: {barkod}.");
            }
            else
            {
                return (false, $"Nije pronadjen Barkod: {barkod}.Izmena neuspesna");
            }
        }
    }
}
