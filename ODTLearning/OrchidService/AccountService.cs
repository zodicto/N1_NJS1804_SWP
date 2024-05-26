using OrchidBusinessObject;
using OrchidRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrchidService
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepositories accountRepositories = null;
        public AccountService()
        {
            if (accountRepositories == null)
            {
                accountRepositories = new AccountRepositories();
            }
        }
        public Account AddAccount(Account account)
        {
          return  accountRepositories.AddAccount(account);
        }

        public bool DeleteAccount(string idAccount)
        {
            return accountRepositories.DeleteAccount(idAccount);
        }

        public List<Account> GetAccount()
        {
            return accountRepositories.GetAccount();
        }

        public Account GetAccountById(string accountId)
        {
            return accountRepositories.GetAccountById(accountId);
        }

        public Account UpdateAccount(Account updatedAccount)
        {
            return accountRepositories.UpdateAccount(updatedAccount);
        }
    }
}
