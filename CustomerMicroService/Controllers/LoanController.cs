using CustomerMicroService.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http.Json;
using Newtonsoft.Json;

namespace CustomerMicroService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoanController : ControllerBase
    {

        static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(LoanController));
        private CustomeContext _context;
        public LoanController(CustomeContext context)
        {
            _context = context;
        }

        [HttpGet("{customerId}")]
        [Route("getCustomerLoan/{customerId}")]
        public async Task<IActionResult> getCustomerLoan(int customerId)
        {
            if (customerId == 0)
            {
                return NotFound();
            }

            var li= _context.Loans.ToList();
            List<Loan> lo= li.FindAll(a => a.CustomerId == customerId);
            var mn = JsonConvert.SerializeObject(lo);
            _log4net.Info("Loan history returned for Account Id: " + customerId);
            return Ok(lo);
        }


        [HttpGet("{customerId}")]
        [Route("Accept/{customerId}")]
        public void Accept(int customerId)
        {
            if (customerId == 0)
            {
                _log4net.Info("Loan history returned for Account Id: " + customerId);
            }

            var li = _context.Loans.ToList();
            Loan lo = li.Find(a => a.CustomerId == customerId);
            
            _log4net.Info("Loan history returned for Account Id: " + customerId);
            lo.Pending = true;
            lo.CurrentStatus = true;
            _context.SaveChanges();
        }

        [HttpGet("{customerId}")]
        [Route("Reject/{customerId}")]
        public void Reject(int customerId)
        {
            if (customerId == 0)
            {
                _log4net.Info("Loan history returned for Account Id: " + customerId);
            }

            var li = _context.Loans.ToList();
            Loan lo = li.Find(a => a.CustomerId == customerId);

            _log4net.Info("Loan history returned for Account Id: " + customerId);
            lo.Pending = true;
            lo.CurrentStatus = false;
            _context.SaveChanges();
        }

        [HttpGet]
        [Route("getAllLoan")]
        public async Task<IActionResult> getAllLoan()
        {
            var li =  _context.Loans.ToList();
            List<Loan> lo = li.FindAll(a => a.Pending == false);
            
            _log4net.Info("Loan history returned for all accounts " );
            return Ok(lo);
        }

        [HttpPost]
        [Route("CreateLoan")]
        public async Task<IActionResult> CreateLoan([FromBody] Loan loan)
        {
            if (loan.Amount <= 0)
                return NotFound();
            try
            {
                _context.Loans.Add(loan);
                _context.SaveChanges();
                return Ok();
            }
            catch(Exception)
            {
                return NotFound();
            }
        }

        }
}
