using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AccountManager.Controllers
{
    public class AccountsController : ApiController
    {
        // DAL
        private static readonly IList<Account> Accounts = new List<Account>
                                     {
                                         new Account { Id = 1, DisplayName = "Ryan Vice", Balance = 1000 }
                                     };

        // GET api/accounts
        public IEnumerable<Account> Get()
        {
            return Accounts.Select(account => account);
        }

        // GET api/accounts/5
        public Account Get(int id)
        {
            return Accounts.Single(account => account.Id == id);
        }

        // POST api/accounts
        public HttpResponseMessage Post(HttpRequestMessage request, PostAccountRequest postAccountRequest)
        {
            var account = new Account
            {
                Id = Accounts.Count + 1,
                DisplayName = postAccountRequest.DisplayName,
                Balance = (int) postAccountRequest.OpeningBalance
            };
            Accounts.Add(account);

            return request.CreateResponse(HttpStatusCode.Created);
        }

        // PUT api/accounts/5
        public void Put([FromBody]PutAccountRequest putAccountRequest)
        {
            var accountToUpdate = Accounts.Single(a => a.Id == putAccountRequest.AccountId);
            accountToUpdate.DisplayName = putAccountRequest.DisplayName;
        }

        // DELETE api/accounts/5
        public void Delete(int id)
        {
            Accounts.Remove(Accounts.First(account => account.Id == id));
        }

        // POST api/accounts/5/withdraw
        public void Withdraw(int id, [FromBody]WithdrawRequest request)
        {
            Accounts.Single(account => account.Id == id).Balance -= request.WithdrawAmount;
        }

        // POST api/accounts/5/deposit
        public void Deposit(int id, [FromBody]DepositRequest request)
        {
            Accounts.Single(account => account.Id == id).Balance += request.DepositAmount;
        }
    }

    public enum AccountType
    {
        Checking,
        Savings
    }

    public class Account
    {
        public int Id { get; set; }

        public string DisplayName { get; set; }

        public int Balance { get; set; }
        public AccountType AccountType { get; set; }
    }


    // DTO Models
    public class GetAccountResponse
    {
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public decimal Balance { get; set; }
    }

    //{
    //    "DisplayName" : "Scott Gu's Checking",
    //    "OpeningBalance" : 10000
    //}
    public class PostAccountRequest
    {
        public string DisplayName { get; set; }
        public decimal OpeningBalance { get; set; }
    }

    //{
    //   "AccountId" : 2,
    //   "DisplayName" : "Scott Gu's Savings"
    //}
    public class PutAccountRequest
    {
        public int AccountId { get; set; }
        public string DisplayName { get; set; }
    }
    public class DepositRequest
    {
        public int DepositAmount { get; set; }
    }

    public class WithdrawRequest
    {
        public int WithdrawAmount { get; set; }
    }


}