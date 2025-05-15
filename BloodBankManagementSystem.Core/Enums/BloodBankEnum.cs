namespace BloodBankManagementSystem.Core.Enums
{
    public static class BloodBankEnum
    {
        public enum BloodGroup
        {
            A_Positive,   // A+
            A_Negative,   // A-
            B_Positive,   // B+
            B_Negative,   // B-
            O_Positive,   // O+
            O_Negative,   // O-
            AB_Positive,  // AB+
            AB_Negative   // AB-
        }
        public enum Gender
        {
            Male,
            Female,
            Other
        }

        public enum Roles{
            Admin, User, Doctor, Hospital
        }
       
        // [Flags]
        public enum HealthIssue
        {
            None = 0,
            ChronicDisease = 1,           // Diabetes, Kidney Disease, etc.
            BloodborneInfection = 2,       // HIV, Hepatitis B/C, etc.
            PregnantOrBreastfeeding = 4,   // For females
            RecentSurgery = 8,
            RestrictedMedications = 16,
            TraveledToMalariaRiskArea = 32,
            CancerHistory = 64,
            DrugUseOrHighRiskBehavior = 128
        }
    }

    public static class FilterOptionsHelper
    {
        public static readonly Dictionary<string, string> AllFilterOption = new()
        {
            { "Active", "Show Active" },
            { "Last7Days", "Last 7 Days" },
            { "Last30Days", "Last 30 Days" },
            { "Last1Year", "Last 1 Year" },
            { "All", "Show All" }
        };
    }
}
