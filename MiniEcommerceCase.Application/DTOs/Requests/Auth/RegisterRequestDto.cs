using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniEcommerceCase.Application.DTOs.Requests.Auth
{
    public class RegisterRequestDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
