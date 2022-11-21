using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bumbodium.Data.DBModels;

namespace Bumbodium.Data.Repositories
{
    public class BumbodiumRepo
    {
        BumbodiumContext _context = new BumbodiumContext();

        public bool ValidateAccount(Account account)
        {
            foreach (Account dbAccount in _context.Accounts)
            {
                if (dbAccount.Username == account.Username)
                {
                    if (dbAccount.Password == account.Password)
                        return true;
                }
            }
            return false;
        }
    }
}
