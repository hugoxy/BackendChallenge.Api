using System.ComponentModel.DataAnnotations;

namespace BackendChallenge.Api.Models.Requests.Client
{
    internal class DeleteClientRequest
    {
        [Required]
        public int ClientId { get; set; }
    }
}