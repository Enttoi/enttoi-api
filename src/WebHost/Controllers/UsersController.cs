using Core.Services;
using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using WebHost.Models;

namespace WebHost.Controllers
{
    /// <summary>
    /// Users API
    /// </summary>
    public class UsersController : ApiController
    {
        private readonly IDistributedCounter _counterService;

        public UsersController(IDistributedCounter counterService)
        {
            if (counterService == null) throw new ArgumentNullException(nameof(counterService));
            _counterService = counterService;
        }

        /// <summary>
        /// Gets the count of user that currently connected to HUB.
        /// </summary>
        /// <returns>The number of online users</returns>
        [Route("users/online")]
        [ResponseType(typeof(OnlineUsers))]
        public async Task<IHttpActionResult> GetSensors()
        {
            return Ok(new OnlineUsers { UsersCount = await _counterService.GetCurrent() });
        }
    }
}
