using OrchidBusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OrchidDAO
{
    public class AccountDAO
    {
        private readonly DbminiCapstoneContext dbminiCapstoneContext;

        public AccountDAO()
        {
            if (dbminiCapstoneContext == null)
            {
                dbminiCapstoneContext = new DbminiCapstoneContext();
            }
        }

        public List<Account> GetAccount()
        {
            return dbminiCapstoneContext.Accounts.ToList();
        }

        // Trong phương thức AddAccount của AccountDAO
        public Account AddAccount(Account account)
        {
            try
            {
                // Mã hóa mật khẩu trước khi thêm vào cơ sở dữ liệu
                account.Password = BCrypt.Net.BCrypt.HashPassword(account.Password);
                dbminiCapstoneContext.Accounts.Add(account);
                dbminiCapstoneContext.SaveChanges();
                return account;
            }
            catch (Exception ex)
            {
                // Log exception or handle it as needed
                throw new Exception("Error adding account", ex);
            }
        }

        // Trong phương thức VerifyPassword của AccountDAO
        public bool VerifyPassword(Account account, string password)
        {
            try
            {
                // Sử dụng BCrypt để xác thực mật khẩu
                return BCrypt.Net.BCrypt.Verify(password, account.Password);
            }
            catch (Exception ex)
            {
                // Log exception or handle it as needed
                throw new Exception("Error verifying password", ex);
            }
        }




        public bool DeleteAccount(string idAccount)
        {
            try
            {
                var account = dbminiCapstoneContext.Accounts.Find(idAccount);
                if (account != null)
                {
                    dbminiCapstoneContext.Accounts.Remove(account);
                    dbminiCapstoneContext.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                // Log exception or handle it as needed
                throw new Exception("Error deleting account", ex);
            }
        }

        public Account UpdateAccount(Account updatedAccount)
        {
            try
            {
                var existingAccount = dbminiCapstoneContext.Accounts.Find(updatedAccount.IdAccount);
                if (existingAccount != null)
                {
                    existingAccount.Username = updatedAccount.Username;
                    existingAccount.Password = updatedAccount.Password;
                    existingAccount.FisrtName = updatedAccount.FisrtName;
                    existingAccount.LastName = updatedAccount.LastName;
                    existingAccount.Gmail = updatedAccount.Gmail;
                    existingAccount.Birthdate = updatedAccount.Birthdate;
                    existingAccount.Gender = updatedAccount.Gender;
                    existingAccount.Role = updatedAccount.Role;

                    dbminiCapstoneContext.SaveChanges();
                    return existingAccount;
                }
                return null;
            }
            catch (Exception ex)
            {
                // Log exception or handle it as needed
                throw new Exception("Error updating account", ex);
            }
        }
        public Account GetAccountById(string accountId)
        {
            try
            {
                return dbminiCapstoneContext.Accounts.Find(accountId);
            }
            catch (Exception ex)
            {
                // Handle the exception (e.g., log it)
                throw new Exception("Error retrieving account by ID", ex);
            }
        }
      
        public Account GetAccountByUsername(string username)
        {
            try
            {
                return dbminiCapstoneContext.Accounts.FirstOrDefault(acc => acc.Username == username);
            }
            catch (Exception ex)
            {
                // Handle the exception (e.g., log it)
                throw new Exception("Error retrieving account by username", ex);
            }
        }
    }
}
