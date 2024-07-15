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

    }
}
