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
    public class CustomerController : ControllerBase
    {
        // GET: api/<CustomerController>
        /// <summary>
        /// Get a list of Customers
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> Get()
        {
            try
            {
                return Ok(await CustomerManager.Load());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // GET api/<CustomerController>/5
        /// <summary>
        /// Get a customer by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("byId/{id:Guid}")]
        public async Task<ActionResult<Customer>> Get(Guid id)
        {
            try
            {
                return Ok(await CustomerManager.LoadById(id));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // GET api/<AppointmentController>/5
        /// <summary>
        /// Get a Customer by UserId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("byUserId/{id:Guid}")]
        public async Task<ActionResult<IEnumerable<Appointment>>> GetByUserId(Guid id)
        {
            try
            {
                return Ok(await CustomerManager.LoadByUserId(id));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        // POST api/<CustomerController>
        /// <summary>
        /// Add a customer
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="rollback"></param>
        /// <returns></returns>
        [HttpPost("{rollback:bool?}")]
        public async Task<IActionResult> Post([FromBody] Customer customer, bool rollback = false)
        {
            try
            {
                var result = await CustomerManager.Insert(customer, rollback);

                if (result == true)
                {
                    return Ok(customer.Id);
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

        // PUT api/<CustomerController>/5
        /// <summary>
        /// Change a customer
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="rollback"></param>
        /// <returns></returns>
        [HttpPut("{rollback:bool?}")]
        public async Task<IActionResult> Put([FromBody] Customer customer, bool rollback = false)
        {
            try
            {
                return Ok(await CustomerManager.Update(customer, rollback));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // DELETE api/<CustomerController>/5
        /// <summary>
        /// Delete a customer
        /// </summary>
        /// <param name="id"></param>
        /// <param name="rollback"></param>
        /// <returns></returns>
        [HttpDelete("{id:Guid}/{rollback:bool?}")]
        public async Task<IActionResult> Delete(Guid id, bool rollback = false)
        {
            try
            {
                return Ok(await CustomerManager.Delete(id, rollback));
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        // GET api/<AppointmentController>/5
        /// <summary>
        /// Get Customer Invoice Balance
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetBalance/{id:Guid}")]
        public async Task<ActionResult<decimal>> GetBalance(Guid id)
        {
            try
            {
                return Ok(await InvoiceManager.GetBalance(id));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


    }
}
