using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.WalletViewModels
{
    public class WalletViewModel
    {
        public int WalletId { get; set; }
        public int? UserId { get; set; }
        public decimal? TotalPoint { get; set; }
    }
}
