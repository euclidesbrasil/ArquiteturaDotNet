using ArquiteturaDesafio.Core.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArquiteturaDesafio.Core.Application.UseCases.DTOs
{
    public class UserBaseDTO
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public AddressDto Address { get; set; }
        public string Phone { get; set; }
        public UserStatus Status { get; set; }
        public UserRole Role { get; set; }
        public string Password { get; set; }
    }
}
