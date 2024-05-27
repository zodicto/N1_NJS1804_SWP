using OrchidBusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrchidService
{
    public interface IAccountService
    {
        public List<Account> GetAccount();


        public Account AddAccount(Account account);


        public bool DeleteAccount(string idAccount);


        public Account UpdateAccount(Account updatedAccount);

        public Account GetAccountById(string accountId);
        public bool VerifyPassword(Account account, string password);
        Account GetAccountByUsername(string username);
    }
}
