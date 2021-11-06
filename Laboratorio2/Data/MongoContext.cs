using MongoDB.Driver;

namespace Laboratorio2.Data
{
    public class MongoContext
    {
        public MongoClient client;

        
        public IMongoDatabase iniBDMongo(string url, string db)
        {
            client = new MongoClient(url);
            return client.GetDatabase(db);
        }


    }
}
