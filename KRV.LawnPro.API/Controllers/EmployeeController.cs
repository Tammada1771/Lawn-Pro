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
    public class EmployeeController : ControllerBase
    {
        // GET: api/<EmployeeController>
        /// <summary>
        /// Get a list of employees
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> Get()
        {
            try
            {
                return Ok(await EmployeeManager.Load());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // GET api/<AppointmentController>/5
        /// <summary>
        /// Get an employee by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("byId/{id:Guid}")]
        public async Task<ActionResult<IEnumerable<Appointment>>> GetById(Guid id)
        {
            try
            {
                 return Ok(await EmployeeManager.LoadById(id));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // GET api/<AppointmentController>/5
        /// <summary>
        /// Get an employee by UserId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("byUserId/{id:Guid}")]
        public async Task<ActionResult<IEnumerable<Appointment>>> GetByUserId(Guid id)
        {
            try
            {
                return Ok(await EmployeeManager.LoadByUserId(id));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        // POST api/<EmployeeController>
        /// <summary>
        /// Add an employee
        /// </summary>
        /// <param name="employee"></param>
        /// <param name="rollback"></param>
        /// <returns></returns>
        [HttpPost("{rollback:bool?}")]
        public async Task<IActionResult> Post([FromBody] Employee employee, bool rollback = false)
        {
            try
            {
                var result = await EmployeeManager.Insert(employee, rollback);

                if (result == true)
                {
                    return Ok(employee.Id);
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

        // PUT api/<EmployeeController>/5
        /// <summary>
        /// Change an employee
        /// </summary>
        /// <param name="employee"></param>
        /// <param name="rollback"></param>
        /// <returns></returns>
        [HttpPut("{rollback:bool?}")]
        public async Task<IActionResult> Put([FromBody] Employee employee, bool rollback = false)
        {
            try
            {
                return Ok(await EmployeeManager.Update(employee, rollback));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // DELETE api/<EmployeeController>/5
        /// <summary>
        /// Delete an employee
        /// </summary>
        /// <param name="id"></param>
        /// <param name="rollback"></param>
        /// <returns></returns>
        [HttpDelete("{id:Guid}/{rollback:bool?}")]
        public async Task<IActionResult> Delete(Guid id, bool rollback = false)
        {
            try
            {
                return Ok(await EmployeeManager.Delete(id, rollback));
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
