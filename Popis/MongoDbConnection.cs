using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Popis
{
   
    public class MongoDBConnection
    {
        private readonly IMongoDatabase _mongoDb;

        public MongoDBConnection()
        {

            var client = new MongoClient("mongodb+srv://ledmastervaljevo:Unapredjenje.popisa2024@cluster0.oh89o.mongodb.net/");
            _mongoDb = client.GetDatabase("PopisGodisnji");
        }

        public IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            return _mongoDb.GetCollection<T>(collectionName);
        }


    }
    
}
