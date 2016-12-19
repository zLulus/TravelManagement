using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManagerClientDemo.Entities;
using TravelManagement.Models;

namespace TravelManagement.Services
{
    public interface ITouristPoints
    {

        List<Tourist> GetTouristLocation(string filter, List<Tourist> AllTouristCollection);

        Task<List<Schedule>> GetTouristsSchedules(string filter, DateTime startTime, DateTime endTime);

        Task<List<Tourist>> GetTouristsLocations();

    }
}
