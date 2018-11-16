using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.MySQL.Repository.AggregatesModel
{
    /// <summary>
    /// 
    /// </summary>
    public class Model
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        public long Id { get; set; } = GuidToLongId();

        /// <summary>
        /// 
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 时间戳
        /// 指定并发检查字段
        /// </summary>
        [ConcurrencyCheck]
        public DateTime _timestamp { get; set; }

        /// <summary>  
        /// 根据GUID获取19位的唯一数字序列
        /// </summary>  
        /// <returns></returns>  
        static long GuidToLongId()
        {
            byte[] buffer = Guid.NewGuid().ToByteArray();
            return BitConverter.ToInt64(buffer, 0);
        }
    }
}
