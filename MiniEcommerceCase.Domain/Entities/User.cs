using MiniEcommerceCase.Domain.Common;

namespace MiniEcommerceCase.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Username { get; set; }
        public string PasswordHash { get; set; }
    }
}
