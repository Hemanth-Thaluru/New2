using AccountMicroservice.Model;
using Microsoft.AspNetCore.Mvc;
using RulesMicroservice.Model;
using RulesMicroservice.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;



// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860



namespace RulesMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RulesController : ControllerBase
    {
        private IRuleRepository _ruleRepository;
        private IChargeRepository _chargeRepository;
        static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(RulesController));
        public RulesController(IRuleRepository ruleRepository )
        {
            _ruleRepository = ruleRepository;
            
        }
        public RulesController(IChargeRepository chargeRepository)
        {
            _chargeRepository = chargeRepository;
        }



        [HttpGet("{AccountID}/{Balance}")]

        [Route("evaluateMinBal/{AccountID}/{Balance}")]
        public async Task<IActionResult> EvaluateMinBalAsync(int AccountID, int Balance)
        {
            RuleStatus ruleStatus = new RuleStatus();
            _log4net.Info("Evaluating Minimum Balance");
            try
            {
                double MinBalance = await _ruleRepository.GetMinBalance(AccountID);
               



                if (Balance >= MinBalance)
                {
                    ruleStatus.status = Status.Allowed;
                    return Ok(ruleStatus);
                }
                else
                {


                    ruleStatus.status = Status.Denied;
                    return BadRequest(ruleStatus);
                }
            }
            catch (NullReferenceException e)
            {
                _log4net.Error("NullReferenceException caught. Issue in calling Account API", e);
                return NotFound(e);

            }
            catch (Exception e)
            {
                _log4net.Error("Exception caught. Issue in calling Account API", e);
                ruleStatus.status = Status.NA;
                return NotFound(ruleStatus);
            }
        }



        [HttpGet]
        [Route("getServiceCharge")]
        public float GetServiceCharge(string AccountType)
        {
            if (AccountType == "Savings")
            {
                return 100;
            }
            else if (AccountType == "Current")
            {
                return 200;
            }
            else
            {
                return 0;
            }
        }




      
    }
}