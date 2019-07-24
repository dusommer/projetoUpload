using Upload.Domain.Entities.Base;
using prmToolkit.NotificationPattern;
using System.Collections.Generic;

namespace Upload.Domain.Entities
{
    public class User : EntityBase
    {
        public User()
        {
        }

        public User(string email, string password)
        {
            Email = email;
            Password = password;
        }

        public string Email { get; private set; }
        public string Password { get; private set; }

        public virtual ICollection<File> Files { get; set; }

        public override void Validate()
        {
            new AddNotifications<User>(this)
                .IfNullOrEmpty(x => x.Email, "Necessário informar o e-mail.")
                .IfNullOrEmpty(x => x.Password, "Necessário informar a senha.");
        }
    }
}
