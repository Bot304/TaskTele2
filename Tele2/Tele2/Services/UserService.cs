using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Tele2.Models;

namespace Tele2.Services
{
    public class UserWithoutAge 
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Sex { get; set; }
    }
    public class UserService
    {
        //private readonly UserContext _context;  

        //public UserService(UserContext context)
        //{
        //    _context = context;
            
        //}
        private List<User> AllUsersCache; // inmemory
        private const string GetAllUsersURL = "http://testlodtask20172.azurewebsites.net/task";
        public async Task<IEnumerable<User>> GetAllUsersAsync() //TODO IEnumerable
        {
            if (AllUsersCache == null)
            {
                AllUsersCache = new List<User>();
                var client = new HttpClient();
                var getList = await client.GetAsync(GetAllUsersURL);
                var resultList = await getList.Content.ReadAsStringAsync();
                var userList = JsonConvert.DeserializeObject<IEnumerable<UserWithoutAge>>(resultList);
                foreach (var userItem in userList)
                {
                    var get = await client.GetAsync(GetAllUsersURL + '/' + userItem.Id);
                    var result = await get.Content.ReadAsStringAsync();
                    var user = JsonConvert.DeserializeObject<User>(result);
                    user.Id = userItem.Id;
                    AllUsersCache.Add(user);
                }
                
            }
            return AllUsersCache;
        }
    }
}
