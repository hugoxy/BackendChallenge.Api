using System.ComponentModel.DataAnnotations;

namespace BackendChallenge.Api.Models.Requests.Client
{
    internal class ReadClientRequest
    {
        [Required]
        public int ClientId { get; set; }
    }
}