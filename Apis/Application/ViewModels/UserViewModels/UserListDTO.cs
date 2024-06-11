using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.UserViewModels
{
    public class UserListDTO
    {
        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string? Phone { get; set; }

        public string? Address { get; set; }

        public DateTime? Dob { get; set; }

        public string? Gender { get; set; }

        public string Username { get; set; } = null!;

        public string? Status { get; set; }

        public int? Roleid { get; set; }
    }
}
