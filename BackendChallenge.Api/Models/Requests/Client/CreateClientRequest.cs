using System.ComponentModel.DataAnnotations;

namespace BackendChallenge.Api.Models.Requests.Client
{
    public class CreateClientRequest
    {
        [Required]
        public string User { get; set; }
    }
}
