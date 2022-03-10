using CustomerMicroService.Model;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Retail_Bank_UI.Controllers
{
    public class LoanUIController : Controller
    {
        int cd = CIDAll.cid;
        static int x;
        public async Task<IActionResult> AllLoans()
        {
            Client client = new Client();
            List<Loan> accounts = new List<Loan>();
            if(x==1)
            {
                ViewData["message"] = "Loan Request Accepted Successfully";
            }
            if (x == 2)
            {
                ViewData["message"] = "Loan Request Rejected Successfully";
            }
            try
            {
                var result = await client.APIClient().GetAsync("http://localhost:5004/api/Loan/getAllLoan");
                if (result.IsSuccessStatusCode)
                {
                    var data = result.Content.ReadAsStringAsync().Result;
                    var allLoans = JsonConvert.DeserializeObject<List<Loan>>(data);
                    return View(allLoans);
                }
                return View();
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                return View();
            }
        }


        [HttpGet]
        public async Task<IActionResult> Accept(int customerId)
        {
            Client client = new Client();
            List<Loan> accounts = new List<Loan>();
            try
            {
                var result = await client.APIClient().GetAsync("http://localhost:5004/api/Loan/Accept" + "/" + customerId);
                if (result.IsSuccessStatusCode)
                {
                    var data = result.Content.ReadAsStringAsync().Result;
                    x = 1;
                    return RedirectToAction("AllLoans");
                }

            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                return Redirect("/Loan/AllLoans");
            }

            return Redirect("/Loan/AllLoans");
        }


        [HttpGet]
        public async Task<IActionResult> Reject(int CustomerId)
        {
            Client client = new Client();
            List<Loan> accounts = new List<Loan>();
            try
            {
                var result = await client.APIClient().GetAsync("http://localhost:5004/api/Loan/Reject" + "/" + CustomerId);
                if (result.IsSuccessStatusCode)
                {
                    var data = result.Content.ReadAsStringAsync().Result;
                    x = 2;
                    return RedirectToAction("AllLoans");

                }

            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                return RedirectToAction("AllLoans");
            }

            return RedirectToAction("AllLoans");
        }


        [HttpGet]
        public async Task<IActionResult> LoanRequest()
        {
            
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> LoanRequest(Loan loanobj)
        {
            loanobj.CustomerId = cd;
            try
            {
                Client client = new Client();
                var res = await client.APIClient().PostAsJsonAsync("http://localhost:5004/api/Loan/CreateLoan", loanobj);
                if (res.IsSuccessStatusCode)
                {
                    ViewData["message"] = "Your Loan Request has placed Successfully";
                    return View();
                }
                ViewData["message"] = "Can't place your Loan Request";
                return View();
            }
            catch (Exception e)
            {
                ViewData["message"] = "Can't place your Loan Request";
               return View();
            }

        }


        [HttpGet]
        public async Task<IActionResult> CustomerLoans()
        {
            int CustomeId = cd;
            Client client = new Client();
            List<Loan> accounts = new List<Loan>();
            try
            {
                var result = await client.APIClient().GetAsync("http://localhost:5004/api/Loan/getCustomerLoan"+"/"+CustomeId);
                if (result.IsSuccessStatusCode)
                {
                    var data = result.Content.ReadAsStringAsync().Result;
                    var allLoans = JsonConvert.DeserializeObject<List<Loan>>(data);
                    return View(allLoans);
                }
                return View();
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                return View();
            }
        }

    }
}
