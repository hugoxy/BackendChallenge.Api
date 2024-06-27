using System.ComponentModel.DataAnnotations;

namespace BackendChallenge.Api.Models.Requests.Client
{
    public class DeleteClientRequest
    {
        [Required]
        public int ClientId { get; set; }
    }
}