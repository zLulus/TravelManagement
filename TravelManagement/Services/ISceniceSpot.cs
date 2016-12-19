using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelManagement.Models;

namespace TravelManagement.Services
{
    public interface ISceniceSpot
    {
        /// <summary>
        /// 修改景点信息
        /// </summary>
        /// <param name="scenic"></param>
        /// <returns></returns>
        bool AlterScenicSpot(Scenic scenic);
         
        /// <summary>
        /// 增加景点信息
        /// </summary>
        /// <param name="scenic"></param>
        /// <returns></returns>
        Task<bool> AddScenicSpot(Scenic scenic);

        /// <summary>
        /// 删除景点信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeleteScenicSpot(string id);


        /// <summary>
        /// 获取景点信息列表
        /// </summary>
        /// <returns></returns>
        Task<List<Scenic>> GetScenicSpotInfoList();


    }
}
