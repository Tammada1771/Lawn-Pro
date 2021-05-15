using KRV.LawnPro.BL;
using KRV.LawnPro.BL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace KRV.LawnPro.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ServiceTypeController : ControllerBase
    {
        // GET: api/<ServiceTypeController>
        /// <summary>
        /// Get a list of service types
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServiceType>>> Get()
        {
            try
            {
                return Ok(await ServiceTypeManager.Load());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // GET api/<ServiceTypeController>/5
        /// <summary>
        /// Get a service type by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<ServiceType>> Get(Guid id)
        {
            try
            {
                return Ok(await ServiceTypeManager.LoadById(id));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // POST api/<ServiceTypeController>
        /// <summary>
        /// Add a service Type
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="rollback"></param>
        /// <returns></returns>
        [HttpPost("{rollback:bool?}")]
        public async Task<IActionResult> Post([FromBody] ServiceType serviceType, bool rollback = false)
        {
            try
            {
                var result = await ServiceTypeManager.Insert(serviceType, rollback);

                if (result == true)
                {
                    return Ok(serviceType.Id);
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

        // PUT api/<ServiceTypeController>/5
        /// <summary>
        /// Change a service type
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="rollback"></param>
        /// <returns></returns>
        [HttpPut("{rollback:bool?}")]
        public async Task<IActionResult> Put([FromBody] ServiceType serviceType, bool rollback = false)
        {
            try
            {
                return Ok(await ServiceTypeManager.Update(serviceType, rollback));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // DELETE api/<ServiceTypeController>/5
        /// <summary>
        /// Delete a service type
        /// </summary>
        /// <param name="id"></param>
        /// <param name="rollback"></param>
        /// <returns></returns>
        [HttpDelete("{id:Guid}/{rollback:bool?}")]
        public async Task<IActionResult> Delete(Guid id, bool rollback = false)
        {
            try
            {
                return Ok(await ServiceTypeManager.Delete(id, rollback));
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
