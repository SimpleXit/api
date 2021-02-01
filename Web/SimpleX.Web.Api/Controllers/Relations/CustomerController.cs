using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SimpleX.Domain.Models.Relations;
using SimpleX.Domain.Models.Shared;
using SimpleX.Domain.Services.Relations;
using SimpleX.Web.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleX.Web.Api.Controllers.Relations
{
    [Authorize]
    [Route("api/customers")]
    [ApiController]
    public class CustomerController : SimpleController
    {
        private readonly ICustomerService _customerService;

        public CustomerController(
            ILogger<CustomerController> logger,
            ICustomerService customerService) : base(logger)
        {
            _customerService = customerService ??
                throw new ArgumentNullException(nameof(customerService));
        }

        [HttpGet]
        public async Task<ActionResult<List<CustomerDto>>> GetCustomers()
        {
            try
            {
                return Ok(await _customerService.GetCustomers());
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerDto>> GetCustomer(long id)
        {
            try
            {
                return Ok(await _customerService.GetCustomer(id));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        [HttpGet("init")]
        public async Task<ActionResult<CustomerDto>> InitCustomer()
        {
            try
            {
                return Ok(await _customerService.InitCustomer());
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        [HttpGet("search/{value}")]
        public async Task<ActionResult<List<CustomerSearchResultDto>>> SearchCustomers(string value)
        {
            try
            {
                return Ok(await _customerService.SearchCustomer(value));
            }
            catch (Exception ex)
            {
                return Error(ex); 
            }
        }

        [HttpGet("filter")]
        public async Task<ActionResult<List<CustomerDto>>> FilterCustomers([FromBody] CustomerFilterDto filter)
        {
            try
            {
                return Ok(await _customerService.GetCustomers(filter));
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        [HttpPost]
        public async Task<ActionResult<CustomerDto>> UpdateCustomer([FromBody] CustomerDto customer)
        {
            try
            {
                return Ok(await _customerService.CreateOrUpdateCustomer(customer));
            }
            catch (ArgumentNullException ex) { return BadRequest(ex.Message); }
            catch (ArgumentException ex) { return BadRequest(ex.Message); }
            catch (InvalidOperationException ex) { return BadRequest(ex.Message); }
            catch (Exception ex) { return Error(ex); }
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteCustomer(long id)
        {
            try
            {
                await _customerService.DeleteCustomer(id);
                return Ok();
            }
            catch(InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        [HttpGet("{id}/notes")]
        public async Task<ActionResult<List<NoteDto>>> GetCustomerNotes(long id)
        {
            try
            {
                return Ok(await _customerService.GetCustomerNotes(id));
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }
    }


}
