using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BackendChallenge.Api.Models.Entity
{
    public class ClientEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ClientId { get; set; }

        public string User { get; set; }

        public string Mail { get; set; }

        public string Password { get; set; }

        public string Role { get; set; }

        public ClientEntity()
        {
        }

        public ClientEntity(int clientId, string user, string mail, string password, string role)
        {
            ClientId = clientId;
            User = user;
            Mail = mail;
            Password = password;
            Role = role;
        }
    }
}
