using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelManagement.Models
{
    public class Point
    {
        /// <summary>
        /// 纬度
        /// </summary>
        public decimal? latitude { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public decimal? longitude { get; set; }

        public DateTime? time { get; set; }
    }
}
