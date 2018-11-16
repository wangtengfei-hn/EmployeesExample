using Example.MongoDB.Repository.AggregatesModel;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace Example.MongoDB.Repository
{
    public class MongoContextSeed
    {
        public static async Task SeedAsync(MongoContext context)
        {
            if (!await context.Advertisements.Indexes.List().AnyAsync())
            {
                await context.Advertisements.Indexes.CreateOneAsync(new CreateIndexModel<Advertisement>(Builders<Advertisement>.IndexKeys.Descending(_ => _.CreateTime)));
                //创建mongo二维球体索引
                //使用地理位置查询时 必须
                await context.Advertisements.Indexes.CreateOneAsync(new CreateIndexModel<Advertisement>(Builders<Advertisement>.IndexKeys.Geo2DSphere(_ => _.Geolocation)));
            }

        }
    }
}
