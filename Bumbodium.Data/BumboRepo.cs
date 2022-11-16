using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bumbodium.Data
{
    public class BumboRepo
    {
        BumbodiumContext _context = new BumbodiumContext();

        public bool ValidateAccount(Account account)
        {
            foreach(Account dbAccount in _context.Accounts)
            {
                if(dbAccount.Email == account.Email)
                {
                    if (dbAccount.Password == account.Password)
                        return true;
                }
            }
            return false;
        }
    }
}
