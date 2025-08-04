using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;


namespace BloodBankManagementSystem.Core.Models
{
    public class BloodRequest
{
    public int Id { get; set; }
    public int? RequesterId { get; set; }  = 101;
    public string? RequesterType { get; set; } ="Patient";
    public string PatientName { get; set; }
    public string Gender { get; set; }
    public string BloodGroup { get; set; }
    public int UnitsRequired { get; set; }
    public string Location { get; set; }
    public string ContactNumber { get; set; }
    public string? Email { get; set; } = string.Empty;
    public string? FullAddress { get; set; } = string.Empty;
    public string Description { get; set; }
    public DateTime RequestedDate { get; set; } = DateTime.Now;
    public DateTime? ApprovedDate { get; set; } 
    public string? CreatedBy {get; set;} = "System";
    public string? UpdatedBy {get; set;} = "Admin";
    public DateTime? UpdatedAt {get; set;}
    public string Status { get; set; } = "Pending"; 

    [NotMapped]
    public string? Notes {get; set;} = string.Empty;

    [NotMapped]
    public bool IsApprovedSelected { get; set; }

    [NotMapped]
    public bool IsRejectedSelected { get; set; }

    [NotMapped]
    public bool IsDeleteSelected { get; set; }

    public bool ActiveYN {get; set;} = true;

}

public class BloodRequestHistory
     {
          public int Id { get; set; }
          public int RequestId{get; set;}
          public DateTime ActionDate { get; set; }
          public string ActionType { get; set; } = string.Empty;
          public string ActionUser { get; set; } = string.Empty;
          public string ActionNote { get; set; } = string.Empty;
     }


     public class BloodRequestStatusUpdateModel
{
    public int Id { get; set; }
    public string NewStatus { get; set; }
    public string? Notes { get; set; }
}

}