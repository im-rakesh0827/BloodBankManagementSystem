using System;
using System.Collections.Generic;
using System.Linq;
using static BloodBankManagementSystem.Core.Enums.BloodBankEnum; // Importing nested enums

namespace BloodBankManagementSystem.Core.Helpers
{
    public static class BloodBankHelper
    {
        public static List<string> GetAllBloodGroups()
        {
            return Enum.GetNames(typeof(BloodGroup))
                       .Select(FormatBloodGroup)
                       .ToList();
        }

        private static string FormatBloodGroup(string bloodGroup)
        {
            return bloodGroup.Replace("_Positive", "+")
                             .Replace("_Negative", "-");
        }

        public static List<string> GetAllGenders()
        {
            return Enum.GetNames(typeof(Gender)).ToList();
        }
    }
}
