using EntityFrameWorkCore.Enums;

namespace CookieAuthSample.Services.Dtos
{
    public class CreateUserInput
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public IdentityType IdentityType { get; set; }
    }
}