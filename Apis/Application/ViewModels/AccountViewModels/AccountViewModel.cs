using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.AccountViewModels
{
    public class AccountViewModel
    {
        public int AccountId { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public int RoleId { get; set; }

        public int? AssociatedId { get; set; }

        public int? TotalPoint { get; set; }
    }
}
