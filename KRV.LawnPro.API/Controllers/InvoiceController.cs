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
    public class InvoiceController : ControllerBase
    {
        // GET: api/<InvoiceController>
        /// <summary>
        /// Get a list of Invoices
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Invoice>>> Get()
        {
            try
            {
                return Ok(await InvoiceManager.Load());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // GET api/<InvoiceController>/5
        /// <summary>
        /// Get an invoice by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("byId/{id:Guid}")]
        public async Task<ActionResult<Invoice>> GetById(Guid id)
        {
            try
            {
                return Ok(await InvoiceManager.LoadById(id));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        // GET api/<AppointmentController>/5
        /// <summary>
        /// Get an invoice by CustomerId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("byCustomerId/{id:Guid}")]
        public async Task<ActionResult<IEnumerable<Invoice>>> GetByCustomerId(Guid id)
        {
            try
            {
                return Ok(await InvoiceManager.LoadByCustomerId(id));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        // POST api/<InvoiceController>
        /// <summary>
        /// Add an invoice
        /// </summary>
        /// <param name="invoice"></param>
        /// <param name="rollback"></param>
        /// <returns></returns>
        [HttpPost("{rollback:bool?}")]
        public async Task<IActionResult> Post([FromBody] Invoice invoice, bool rollback = false)
        {
            try
            {
                var result = await InvoiceManager.Insert(invoice, rollback);

                if (result == true)
                {
                    return Ok(invoice.Id);
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

        // PUT api/<InvoiceController>/5
        /// <summary>
        /// Change an invoice
        /// </summary>
        /// <param name="invoice"></param>
        /// <param name="rollback"></param>
        /// <returns></returns>
        [HttpPut("{rollback:bool?}")]
        public async Task<IActionResult> Put([FromBody] Invoice invoice, bool rollback = false)
        {
            try
            {
                return Ok(await InvoiceManager.Update(invoice, rollback));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // DELETE api/<InvoiceController>/5

        /// <summary>
        /// Delete an invoice
        /// </summary>
        /// <param name="id"></param>
        /// <param name="rollback"></param>
        /// <returns></returns>
        [HttpDelete("{id:Guid}/{rollback:bool?}")]
        public async Task<IActionResult> Delete(Guid id, bool rollback = false)
        {
            try
            {
                return Ok(await InvoiceManager.Delete(id, rollback));
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
