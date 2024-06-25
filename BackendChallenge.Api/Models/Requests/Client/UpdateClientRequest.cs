using System.ComponentModel.DataAnnotations;

namespace BackendChallenge.Api.Models.Requests.Client
{
    public class UpdateClientRequest
    {
        [Required]
        public int ClientId { get; set; }
        [Required]
        public string User { get; set; }
    }
}