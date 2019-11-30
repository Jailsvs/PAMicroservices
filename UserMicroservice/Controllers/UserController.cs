using Microsoft.AspNetCore.Mvc;
using UserMicroservice.Services;
using SharedMicroservice.DTO;
using SharedMicroservice.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace UserMicroservice.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {

        private readonly IUserService _userService;
        
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Get([FromQuery] int tenantId)
        {
            /*var hostName = HttpContext.Request.Host;
            TenantDTO tenant = await _userService.GetTenant("localhost:5001");//hostName
            if (tenant == null)
                throw new Exception("Tenant not found");*/

            var users = _userService.ReturnAll(tenantId);
            return new OkObjectResult(users);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var user = _userService.ReturnById(id);
            if (user == null)
                return new NotFoundResult();
            return new OkObjectResult(user);
        }

        [Route("Bids/{id}")]
        [HttpGet()]
        public IActionResult GetBids(int id)
        {
            var user = _userService.ReturnById(id);
            return new OkObjectResult(user.AvailableBids);
        }

        [HttpPost]
        public IActionResult Post([FromBody] UserDTO userDTO, [FromQuery] int tenantId)
        {
            /*var hostName = HttpContext.Request.Host;
            TenantDTO tenant = await _userService.GetTenant("localhost:5001");//hostName
            if (tenant == null)
                throw new Exception("Tenant not found");*/

            userDTO.TenantId = tenantId;
            int dto_id = _userService.Add(userDTO);
            return CreatedAtAction(nameof(Get), new { id = dto_id });
        }

        [Route("{id}/Bid")]
        [HttpPost]
        public IActionResult PostIncreaseBid(int id, [FromBody] UserAvailableBidDTO userDTO)
        {
            userDTO.Id = id;
            _userService.IncreaseBids(userDTO);
            return new OkResult();
        }

        [HttpPut]
        public IActionResult Put([FromBody] UserUpdateDTO userDTO)
        {
            if (userDTO != null)
            {
                _userService.Alter(userDTO);
                return new OkResult();
            }
            return new NoContentResult();
        }

        [Route("{id}/Bid")]
        [HttpPut]
        public IActionResult PutDecreaseBid(int id, [FromBody] UserAvailableBidDTO userDTO)
        {
            userDTO.Id = id;
            _userService.DecreaseBids(userDTO);
            return new OkResult();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _userService.Remove(id);
            return new OkResult();
        }
    }
}
