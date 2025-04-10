namespace BloodBankManagementSystem.UI.Helpers
{
    public static class ServerConstants
    {



        // [StringValue("api/donors/history")]

        public const string BaseApiUrl = "https://localhost:5001/api/";

        public static class Donor
        {
            public const string GetDonorHistoryById = "api/donors/history/"; 
            public const string RegisterDonor = "api/donors/register";
            public const string GetAllDonors = "api/donors";
            public const string Update = "api/donors/update";
            public const string Delete = "api/donors/delete/";
        }

        public static class Auth
        {
            public const string Login = "auth/login";
            public const string Register = "auth/register";
        }

    }
}
