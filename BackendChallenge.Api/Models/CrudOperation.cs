namespace BackendChallenge.Api.Models
{
    public static class CrudOperation
    {
        public const string CreateClient = "CreateClient";
        public const string ReadClient = "ReadClient";
        public const string UpdateClient = "UpdateClient";
        public const string DeleteClient = "DeleteClient";

        public const string CreateOrder = "CreateOrder";
        public const string ReadOrder = "ReadOrder";
        public const string UpdateOrder = "UpdateOrder";
        public const string DeleteOrder = "DeleteOrder";

        public const string CreateProduct = "CreateProduct";
        public const string ReadProduct = "ReadProduct";
        public const string UpdateProduct = "UpdateProduct";
        public const string DeleteProduct = "DeleteProduct";

        public static IEnumerable<string> GetValues()
        {
            return typeof(CrudOperation)
                .GetFields()
                .Where(field => field.IsLiteral)
                .Select(field => (string)field.GetRawConstantValue());
        }
    }
}
