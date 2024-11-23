using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Popis
{

    public class DbReader
    {
        private readonly IMongoCollection<Article> _articleCollection;

        public DbReader()
        {
            var client = new MongoClient(Properties.Settings.Default.DbString);
            var database = client.GetDatabase(Properties.Settings.Default.DB);
            _articleCollection = database.GetCollection<Article>("Articles");
        }

        public List<Article> GetAllArticles()
        {
            return _articleCollection.Find(_ => true).ToList();
        }

        public List<Article> FindArticles(string searchTerm)
        {
            var filter = Builders<Article>.Filter.Or(
                Builders<Article>.Filter.Regex(a => a.naziv, new MongoDB.Bson.BsonRegularExpression(searchTerm, "i")), // Case-insensitive partial match
                Builders<Article>.Filter.Regex(a => a.barkod, new MongoDB.Bson.BsonRegularExpression(searchTerm, "i"))
            );

            var results = _articleCollection.Find(filter).ToList();

            if (results.Count == 0)
            {
                MessageBox.Show($"No articles found matching: {searchTerm}");
            }

            return results;
        }

        public Article GetArticleByBarkod(string barkod)
        {
            return _articleCollection.Find(a => a.barkod == barkod).FirstOrDefault();
        }
    }
}
