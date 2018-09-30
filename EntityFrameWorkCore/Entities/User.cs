using EntityFrameWorkCore.Enums;

namespace EntityFrameWorkCore.Entities
{
    public class User : EntityBase
    {
        public string Email { get; set; }

        public string Company { get; set; }

        public IdentityType IdentityType { get; set; }

        public string Password { get; set; }
    }
}