using AccountMicroservice.Model;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Retail_Bank_UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TransactionMicroservice.Models;

namespace Retail_Bank_UI.Controllers
{
    public class CustomerLoginController : Controller
    {
        
        public async Task<IActionResult> Index(int customerId)
        {
            if(customerId==0)
            {
                customerId = CIDAll.cid;
            }
            Client client = new Client();
            List<Account> accounts = new List<Account>();
            try
            {
                var result = await client.APIClient().GetAsync("https://localhost:44368/api/Account/getCustomerAccount" + "/"+ customerId);
                if (result.IsSuccessStatusCode)
                {
                    var account = result.Content.ReadAsStringAsync().Result;
                    accounts = JsonConvert.DeserializeObject<List<Account>>(account);
                }
                return View(accounts);
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                return View();
            }

        }

        public IActionResult DepositCustomer()
        {
            Deposit dp = new Deposit();
            List<Account> accounts = GetAccountId();
            dp.AccountId = accounts[0].Sav_AccountId;
            return View(dp);
        }


        public List<Account> GetAccountId()
        {
            Client client1 = new Client();
            List<Account> acc = new List<Account>();

            var result = client1.APIClient().GetAsync("https://localhost:44368/api/Account/getCustomerAccount/" + CIDAll.cid).Result;
            if (result.IsSuccessStatusCode)
            {
                var s = result.Content.ReadAsStringAsync().Result;
                acc = JsonConvert.DeserializeObject<List<Account>>(s);
            }
            return acc;
        }

        [HttpPost]
        public IActionResult DepositCustomer(Deposit _deposit)
        {

            List<Account> ac = GetAccountId();

            _deposit.AccountId = ac[0].Sav_AccountId;
            TransactionStatus status = new TransactionStatus();

            using (HttpClient client = new HttpClient())
            {

                client.BaseAddress = new Uri("http://localhost:5000/api/Transaction/");
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response1 = client.PostAsJsonAsync("Deposit", _deposit).Result;
                if (response1.IsSuccessStatusCode)
                {
                    var s = response1.Content.ReadAsStringAsync().Result;
                    status = JsonConvert.DeserializeObject<TransactionStatus>(s);
                    ViewData["message"] = status.Message;


                }
                return View();
            }
        }

        public IActionResult WithdrawCustomer()
        {
            Deposit dp = new Deposit();
            List<Account> accounts = GetAccountId();
            dp.AccountId = accounts[0].Sav_AccountId;
            return View(dp);
        
        }
        [HttpPost]
        public IActionResult WithdrawCustomer(Deposit _deposit)
        {
            TransactionStatus status = new TransactionStatus();

            using (HttpClient client = new HttpClient())
            {

                client.BaseAddress = new Uri("http://localhost:5000/api/Transaction/");
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response1 = client.PostAsJsonAsync("Withdraw", _deposit).Result;
                if (response1.IsSuccessStatusCode)
                {
                    var s = response1.Content.ReadAsStringAsync().Result;
                    status = JsonConvert.DeserializeObject<TransactionStatus>(s);
                    ViewData["message"] = status.Message;


                }
                return View();
            }
        }


        public IActionResult TransferCustomer()
        {
            Transfer transfer = new Transfer();
            List<Account> accounts = GetAccountId();
            transfer.Source_ACC_ID = accounts[0].Sav_AccountId;
            return View(transfer);
        }
        [HttpPost]
        public IActionResult TransferCustomer(Transfer transfer)
        {
            TransactionStatus status = new TransactionStatus();

            using (HttpClient client = new HttpClient())
            {

                client.BaseAddress = new Uri("http://localhost:5000/api/Transaction/");
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response1 = client.PostAsJsonAsync("Transfer", transfer).Result;
                if (response1.IsSuccessStatusCode)
                {
                    var s = response1.Content.ReadAsStringAsync().Result;
                    status = JsonConvert.DeserializeObject<TransactionStatus>(s);
                    ViewData["message"] = status.Message;


                }
                return View();
            }
        }

        [HttpGet]
        public IActionResult GetStatementCustomer()
        {
            StatementUI sd = new StatementUI();
            List<Account> accounts = GetAccountId();
            sd.AccountId = accounts[0].Sav_AccountId;
            return View(sd);
        }
        [HttpPost]
        public async Task<IActionResult> StatementCustomer(StatementUI sd)
        {

            IEnumerable<Statement> status = null;

            Client client = new Client();
            var result = await client.APIClient().GetAsync("http://localhost:5000/api/Transaction/getStatement/" + sd.AccountId + "/" + sd.FromDate + "/" + sd.ToDate);
            if (result.IsSuccessStatusCode)
            {
                var data = result.Content.ReadAsStringAsync().Result;
                status = JsonConvert.DeserializeObject<IEnumerable<Statement>>(data);
            }
            return View(status);
        }

    }
}
