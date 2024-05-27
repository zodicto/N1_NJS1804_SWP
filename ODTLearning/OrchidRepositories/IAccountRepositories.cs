using OrchidBusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrchidRepositories
{
    public interface IAccountRepositories
    {
        public List<Account> GetAccount();


        public Account AddAccount(Account account);


        public bool DeleteAccount(string idAccount);


        public Account UpdateAccount(Account updatedAccount);

        public Account GetAccountById(string accountId);
        public bool VerifyPassword(Account account, string password);
        public Account GetAccountByUsername(string username);


    }
}
