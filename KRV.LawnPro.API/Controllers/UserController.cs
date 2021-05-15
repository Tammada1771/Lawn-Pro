using KRV.LawnPro.BL;
using KRV.LawnPro.BL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace KRV.LawnPro.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {


        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
        }


        // GET: api/<UserController>
        /// <summary>
        /// Get a list of Users
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> Get()
        {
            try
            {
                return Ok(await UserManager.Load());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // GET api/<UserController>/5
        /// <summary>
        /// Get a User based on Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<User>> Get(Guid id)
        {
            try
            {
                return Ok(await UserManager.LoadById(id));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // GET api/<UserController>/5
        /// <summary>
        /// Login with username and password
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public async Task<ActionResult<User>> Login([FromBody] User user)
        {
            try
            {
                user = await UserManager.Login(user);

                if (user.Id != Guid.Empty)
                {
                    _logger.LogWarning("Login Successful. Username: " + user.UserName);
                } 
                else
                {
                    _logger.LogWarning("Login Attempt Failed. Username: " + user.UserName);

                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Login Error. Username: " + user.UserName + " Error: " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // POST api/<UserController>
        /// <summary>
        /// Insert a new User
        /// </summary>
        /// <param name="user"></param>
        /// <param name="rollback"></param>
        /// <returns></returns>
        [HttpPost("{rollback:bool?}")]
        public async Task<IActionResult> Post([FromBody] User user, bool rollback = false)
        {
            try
            {
                var result = await UserManager.Insert(user, rollback);

                if (result == true)
                {
                    return Ok(user.Id);
                }
                else
                {
                    return Ok(string.Empty);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // PUT api/<UserController>/5
        /// <summary>
        /// Update a User
        /// </summary>
        /// <param name="user"></param>
        /// <param name="nameonly"></param>
        /// <param name="rollback"></param>
        /// <returns></returns>
        [HttpPut("{nameonly:bool?}/{rollback:bool?}")]
        public async Task<IActionResult> Put([FromBody] User user, bool nameonly = false, bool rollback = false)
        {
            try
            {
                return Ok(await UserManager.Update(user, nameonly, rollback));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // DELETE api/<UserController>/5
        /// <summary>
        /// Delete a user
        /// </summary>
        /// <param name="id"></param>
        /// <param name="rollback"></param>
        /// <returns></returns>
        [HttpDelete("{id:Guid}/{rollback:bool?}")]
        public async Task<IActionResult> Delete(Guid id, bool rollback = false)
        {
            try
            {
                return Ok(await UserManager.Delete(id, rollback));
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        // GET: api/<UserController>
        /// <summary>
        /// Seeds the Users
        /// </summary>
        /// <returns></returns>
        [HttpGet("seed/")]
        public async Task<ActionResult<IEnumerable<User>>> SeedPasswords()
        {
            try
            {
                return Ok(await UserManager.Seed());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

    }
}
