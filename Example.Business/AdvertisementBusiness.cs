using Common;
using Common.Extensions;
using Example.IBusiness;
using Example.IBusiness.Model;
using Example.MongoDB.Repository;
using Example.MongoDB.Repository.AggregatesModel;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.Business
{
    public class AdvertisementBusiness : IAdvertisementBusiness
    {

        internal log4net.ILog log = log4net.LogManager.GetLogger(typeof(AdvertisementBusiness));

        public MongoContext dbContext { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public AdvertisementModel Get(Guid id)
        {
            var result = dbContext.Advertisements.AsQueryable().FirstOrDefault(_ => _.Id == id);

            return result.Translate<Advertisement, AdvertisementModel>((input, output) =>
            {
                output.CreateTime = new DateTime(input.CreateTime);
            });
        }
        /// <summary>
        /// 查GPS坐标范围内的
        /// </summary>
        /// <param name="geolocation"></param>
        /// <returns></returns>
        public List<AdvertisementModel> Get(Geolocation geolocation)
        {
            //使用mongo的地理位置查询
            var list = dbContext.Advertisements
                        .Find(Builders<Advertisement>.Filter.And(

                            Builders<Advertisement>.Filter.And(
                                Builders<Advertisement>.Filter.Eq(_ => _.Useful, true),
                                Builders<Advertisement>.Filter.Eq(_ => _.Show, true)),

                                //Builders<Advertisement>.Filter.Or(
                                //    Builders<Advertisement>.Filter.Eq(_ => _.Useful, true),
                                //    Builders<Advertisement>.Filter.Eq(_ => _.Show, true)),

                                Builders<Advertisement>.Filter.GeoWithinCenterSphere(_ => _.Geolocation, geolocation.Longitude, geolocation.Latitude, 1D / 6371)
                        )).ToList();
            var result = list.Select(_ => _.Translate<Advertisement, AdvertisementModel>((input, output) =>
            {
                output.CreateTime = new DateTime(input.CreateTime);
            })).ToList();

            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="images"></param>
        /// <param name="content"></param>
        /// <param name="geolocation"></param>
        /// <returns></returns>
        public AdvertisementModel Add(string[] images, string content, Geolocation geolocation)
        {
            var result = new Advertisement
            {
                Content = content,
                Images = images,
                Geolocation = geolocation,
                LikeCount = 0,
                ReplyCount = 0,
                Show = true
            };

            dbContext.Advertisements.InsertOne(result);

            return result.Translate<Advertisement, AdvertisementModel>((input, output) =>
            {
                output.CreateTime = new DateTime(input.CreateTime);
            });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="images"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public ResultModel Modify(Guid id, string[] images, string content)
        {
            var result = new ResultModel();

            var update = dbContext.Advertisements.UpdateOne(_ => _.Id == id,
              Builders<Advertisement>.Update
              .Set(_ => _.Images, images)
              .Set(_ => _.Content, content));

            if (update.IsAcknowledged)
                result.Success = true;
            else
                result.Message = "更新失败";

            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ResultModel Delete(Guid id)
        {
            var result = new ResultModel();

            var update = dbContext.Advertisements.UpdateOne(_ => _.Id == id,
              Builders<Advertisement>.Update
              .Set(_ => _.Useful, false));

            if (update.IsAcknowledged)
                result.Success = true;
            else
                result.Message = "删除失败";

            //var delete = dbContext.Advertisements.DeleteOne(Builders<Advertisement>.Filter.Eq(_ => _.Id, id));
            //if (delete.IsAcknowledged)
            //    result.Success = true;
            //else
            //    result.Message = "删除失败";

            return result;
        }

    }
}
