using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Tele2.Models;
using RestSharp;
using RestSharp.Authenticators;
using System.Text.Json;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Rendering;
using Tele2.Services;

namespace Tele2.Controllers
{
    [Produces("application/json")]
    [Route("Users")]
    public class UsersController : Controller
    {
        
        private readonly UserService _userService; // TODO ReadOnly

        public UsersController(UserService userService)
        {
            
            _userService = userService;

        }

        //https://localhost:44322/Users/GetUsers?sex=male&fromage=20&toage=30&page=1&pagesize=5
        [HttpGet("GetUsers")]
        public async Task<IActionResult> GetUsersAsync([FromQuery]int fromAge, [FromQuery] int toAge, [FromQuery] string sex, [FromQuery] int page = 0, [FromQuery] int pageSize = 0)
        {
            var users = await _userService.GetAllUsersAsync();

            if (fromAge > 0)
            {
                users = users.Where(u => u.Age >= fromAge); 
            }

            if (toAge > 0)
            {
                users = users.Where(u => u.Age <= toAge); 
            }

            if (!string.IsNullOrWhiteSpace(sex))
            {
                users = users.Where(u => u.Sex == sex); 
            }
            users = users.Skip(page * pageSize);
            if (pageSize > 0)
            {
                users = users.Take(pageSize);
            }
            return Ok(users);
        }

        //https://localhost:44322/Users/GetUserWithoutAge?id=qyfgqiyhwfoq1
        [HttpGet("GetUserWithoutAge")]
        public async Task<IActionResult> GetUserWithoutAgeAsync([FromQuery] string id)
        {
            var users = await _userService.GetAllUsersAsync();


            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest();
            }

            users = users.Where(u => u.Id == id); 

            return Ok(users.Select(u => new { u.Id, u.Sex, u.Name }).FirstOrDefault());
        }

    }
}
