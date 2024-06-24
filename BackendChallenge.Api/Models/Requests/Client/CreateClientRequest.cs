namespace BackendChallenge.Api.Models.Requests.Client
{
    public class CreateClientRequest
    {
        public string User { get; set; }

        public CreateClientRequest(string user)
        {
            User = user;
        }
    }
}
