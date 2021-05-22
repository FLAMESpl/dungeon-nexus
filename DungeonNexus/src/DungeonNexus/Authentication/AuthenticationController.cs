using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DungeonNexus.Authentication
{
    [Route("api")]
    public class AuthenticationController : Controller
    {
        [HttpGet("login")]
        public async Task<IActionResult> Login()
        {
            return Redirect("/login");
        }
    }
}
