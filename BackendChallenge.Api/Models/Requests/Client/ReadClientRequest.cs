using System.ComponentModel.DataAnnotations;

namespace BackendChallenge.Api.Models.Requests.Client
{
    public class ReadClientRequest
    {
        [Required]
        public int ClientId { get; set; }
    }
}