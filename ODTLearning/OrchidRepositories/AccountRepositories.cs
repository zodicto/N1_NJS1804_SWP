using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using OrchidBusinessObject;
using OrchidDAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrchidRepositories
{
    public class AccountRepositories : IAccountRepositories
    {
        private readonly AccountDAO accountDAO = null;
        public AccountRepositories()
        {
            if (accountDAO == null)
            {
                accountDAO = new AccountDAO();
            }
        }
        public Account AddAccount(Account account)
        {
            return accountDAO.AddAccount(account);
        }

        public bool DeleteAccount(string idAccount)
        {
            return accountDAO.DeleteAccount(idAccount);
        }

        public List<Account> GetAccount()
        {
            return accountDAO.GetAccount();
        }

        public Account GetAccountById(string accountId)
        {
            return accountDAO.GetAccountById(accountId); ;
        }

        public Account UpdateAccount(Account updatedAccount)
        {
            return accountDAO.UpdateAccount(updatedAccount) ;
        }
    }
}
