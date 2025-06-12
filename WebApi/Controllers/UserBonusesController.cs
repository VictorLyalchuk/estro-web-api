using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserBonusesController : Controller
    {
        private readonly IUserBonusesService _userBonuses;
        public UserBonusesController(IUserBonusesService userBonuses)
        {
            _userBonuses = userBonuses;
        }
        
        [HttpGet("getBonuses")]
        public async Task<IActionResult> GetUserBonuses(string userId)
        {
            var bonuses = await _userBonuses.GetUserBonuses(userId);
            if (bonuses == null)
            {
                return Ok();
            }
            return Ok(bonuses);
        }
    }
}
