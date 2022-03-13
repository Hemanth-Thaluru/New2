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
    public class TransactionViewController : Controller
    {
        //trying-starts



        //trying-ends

        public List<Account> GetAllAccount()
        {
            Client client1 = new Client();
            List<Account> acc = new List<Account>();

            var result = client1.APIClient().GetAsync("https://localhost:44368/api/Account/getAllAccounts/").Result;
            if (result.IsSuccessStatusCode)
            {
                var s = result.Content.ReadAsStringAsync().Result;
                acc = JsonConvert.DeserializeObject<List<Account>>(s);
            }
            return acc;
        }

        public IActionResult Deposit()
        {
            
            return View();
        }
        [HttpPost]
        public IActionResult Deposit(Deposit _deposit)
        {
            TransactionStatus status = new TransactionStatus();
            List<Account> ac1 = new List<Account>();
            ac1 = GetAllAccount();
            foreach (var item in ac1)
            {
                if (item.Sav_AccountId == _deposit.AccountId)
                {
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
            }
            ViewData["message"] = "Account Id not found";
            return View();
        }
        public IActionResult Withdraw()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Withdraw(Deposit _deposit)
        {
            TransactionStatus status = new TransactionStatus();
            List<Account> ac1 = new List<Account>();
            ac1 = GetAllAccount();
            foreach (var item in ac1)
            {
                if (item.Sav_AccountId == _deposit.AccountId)
                {
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
            }
            ViewData["message"] = "Account Id not valid";
            return View();
        }
        //Transfer
        public IActionResult Transfer()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Transfer(Transfer transfer)
        {
            TransactionStatus status = new TransactionStatus();
            List<Account> ac1 = new List<Account>();
            ac1 = GetAllAccount();
            foreach (var item in ac1)
            {
                
                    if (transfer.Source_ACC_ID == transfer.Destination_ACC_ID)
                    {
                        ViewData["message"] = "Source and Destination Account can't be same";
                        return View();
                    }

                        if (transfer.Destination_ACC_ID == item.Sav_AccountId)
                        {
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
                    
                
            }
            ViewData["message"] = "Source or Destination Account not found";
            return View();
        }

        [HttpGet]
        public IActionResult GetStatement()
        {
            StatementUI sd = new StatementUI();
            return View(sd);
        }
        [HttpPost]
        public async Task<IActionResult> Statement(StatementUI sd)
        {
            List<Account> ac1 = new List<Account>();
            ac1 = GetAllAccount();
            foreach(var item in ac1)
            {
                if(item.Sav_AccountId==sd.AccountId)
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
            return RedirectToAction("GetStatement");
           
        }


    }
    }
