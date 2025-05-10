using System;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
namespace BloodBankManagementSystem.UI.Helpers
{
    public static class ServerConstants
    {
        // public const string BaseApiUrl = "https://localhost:5001/api/";
        public enum APICallNames
        {
            #region Donor APICallNames
            [StringValue("api/donors/allDonors")] GetAllDonors,
            [StringValue("api/donors/history/")] GetDonorHistoryById,
            [StringValue("api/donors/register")] RegisterDonor,
            [StringValue("api/donors/update/")] UpdateDonor,
            [StringValue("api/donors/delete/")] DeleteDonor,
            #endregion

            #region Patient APICallNames
            [StringValue("api/patients/register")] RegisterPatient,
            [StringValue("api/patients/update/")] UpdatePatient,
            [StringValue("api/patients/")] DeletePatient,
            [StringValue("api/patients/allPatients")] GetAllPatients,
            [StringValue("api/patients/history/")] GetPatientHistoryById,
            #endregion

            #region User APICallNames
            [StringValue("api/Users/register")] RegisterUser,
            [StringValue("/api/users/update/")] UpdateUser,
            [StringValue("/api/users/")] DeleteUser,
            [StringValue("api/Users/allUsers")] GetAllUsers,
            [StringValue("api/users/history/")] GetUserHistoryById,
            #endregion

            #region Request APICallNames
            [StringValue("api/bloodrequest/create")] CreateBloodRequest,
            [StringValue("api/bloodrequest/update/")] UpdateBloodRequest,
            [StringValue("api/bloodrequest/")] DeleteRequest,
            [StringValue("api/bloodrequest/allBloodRequests")] GetAllBloodRequests,
            [StringValue("api/bloodrequest/updatestatus/")] UpdateBloodRequestStatus,
            #endregion
        }

        public static class Auth
        {
            public const string Login = "auth/login";
            public const string Register = "auth/register";
        }
    }

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class StringValueAttribute : Attribute
    {
        public string Value { get; private set; }
        public StringValueAttribute(string value)
        {
            Value = value;
        }
    }

    public static class EnumExtensions
    {
        public static string GetStringValue(this Enum value)
        {
            Type type = value.GetType();
            string name = Enum.GetName(type, value);
            if (name != null)
            {
                FieldInfo field = type.GetField(name);
                if (field != null)
                {
                    StringValueAttribute attr =
                           Attribute.GetCustomAttribute(field, typeof(StringValueAttribute)) as StringValueAttribute;
                    if (attr != null)
                    {
                        return attr.Value;
                    }
                }
            }
            return null; 
        }
    }
}
