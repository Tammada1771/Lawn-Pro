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
    public class AppointmentController : ControllerBase
    {
        // GET: api/<AppointmentController>
        /// <summary>
        /// Get a list of Appointments
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Appointment>>> Get()
        {
            try
            {
                return Ok(await AppointmentManager.Load());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // GET api/<AppointmentController>/5
        /// <summary>
        /// Get an appointment by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("byId/{id:Guid}")]
        public async Task<ActionResult<Appointment>> Get(Guid id)
        {
            try
            {
                return Ok(await AppointmentManager.LoadById(id));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        // GET api/<AppointmentController>/5
        /// <summary>
        /// Get a list of appointments by EmployeeId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("byEmployeeId/{id:Guid}")]
        public async Task<ActionResult<IEnumerable<Appointment>>> GetByEmployeeId(Guid id)
        {
            try
            {
                 return Ok(await AppointmentManager.LoadByEmployeeId(id));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        // GET api/<AppointmentController>/5
        /// <summary>
        /// Get a list of appointments by customerId
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("byCustomerId/{id:Guid}")]
        public async Task<ActionResult<IEnumerable<Appointment>>> GetByCustomerId(string mode, Guid id)
        {
            try
            {
                return Ok(await AppointmentManager.LoadByCustomerId(id));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }



        // POST api/<AppointmentController>
        /// <summary>
        /// Add and appointment
        /// </summary>
        /// <param name="appointment"></param>
        /// <param name="rollback"></param>
        /// <returns></returns>
        [HttpPost("{rollback:bool?}")]
        public async Task<IActionResult> Post([FromBody] Appointment appointment, bool rollback = false)
        {
            try
            {
                var result = await AppointmentManager.Insert(appointment, rollback);

                if (result == true)
                {
                    return Ok(appointment.Id);
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

        // PUT api/<AppointmentController>/5
        /// <summary>
        /// Change an appointment
        /// </summary>
        /// <param name="appointment"></param>
        /// <param name="rollback"></param>
        /// <returns></returns>
        [HttpPut("{rollback:bool?}")]
        public async Task<IActionResult> Put([FromBody] Appointment appointment, bool rollback = false)
        {
            try
            {
                return Ok(await AppointmentManager.Update(appointment, rollback));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // DELETE api/<AppointmentController>/5
        /// <summary>
        /// Delete an appointment
        /// </summary>
        /// <param name="id"></param>
        /// <param name="rollback"></param>
        /// <returns></returns>
        [HttpDelete("{id:Guid}/{rollback:bool?}")]
        public async Task<IActionResult> Delete(Guid id, bool rollback = false)
        {
            try
            {
                return Ok(await AppointmentManager.Delete(id, rollback));
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
