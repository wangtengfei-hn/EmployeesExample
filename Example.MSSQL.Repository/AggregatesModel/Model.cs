using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Example.MSSQL.Repository.AggregatesModel
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
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// 
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 时间戳
        /// 指定并发检查字段
        /// </summary>
        [Timestamp]
        public byte[] Timestamp { get; set; }
    }
}
