using Example.MongoDB.Repository.AggregatesModel;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.MongoDB.Repository
{
    public class MongoContext
    {
        readonly IMongoDatabase database;


        public MongoContext(string connectionString,string databaseName)
        {
            var client = new MongoClient(connectionString);
            this.database = client.GetDatabase(databaseName);
        }

        /// <summary>
        /// 
        /// </summary>
        public IMongoCollection<Advertisement> Advertisements => database.GetCollection<Advertisement>(nameof(Advertisements));

        /// <summary>
        /// 
        /// </summary>
        public static void SetMappings()
        {
            BsonClassMap.RegisterClassMap<Advertisement>(cm =>
            {
                cm.AutoMap();
                cm.SetIdMember(cm.GetMemberMap(_ => _.Id));
            });

        }
    }
}
