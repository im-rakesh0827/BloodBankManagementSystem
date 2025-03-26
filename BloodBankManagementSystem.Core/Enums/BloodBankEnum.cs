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
}
