using BloodBankManagementSystem.Core.Interfaces;
using BloodBankManagementSystem.Core.Models;
using Microsoft.AspNetCore.Mvc;
using BloodBankManagementSystem.API.Services;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
namespace BloodBankManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BloodRequestController : ControllerBase
    {
        private readonly IBloodRequestRepository _requestRepository;
        public BloodRequestController(IBloodRequestRepository repository)
        {
            _requestRepository = repository;
        }
        [HttpPost("create")]
        public async Task<IActionResult> RequestBlood([FromBody] BloodRequest request)
        {
            try
            {
                await _requestRepository.AddRequestAsync(request);
                // await AddHistory(request.RequesterId, "Sysstem User", "Create", "Request created");
                return Ok(new { message = "Request submitted successfully" });
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        [HttpGet("allBloodRequests")]
        public async Task<ActionResult<List<BloodRequest>>> GetAllRequests()
        {
            try
            {
                return Ok(await _requestRepository.GetAllRequestsAsync());
            }
            catch (System.Exception)
            {
                throw;
            }
        }


        [HttpPut("updatestatus/{id}")]
        public async Task<IActionResult> UpdateBloodRequestStatus(int id, [FromQuery] bool status)
        {
            try
            {
                string StatusValue = status?"Approved":"Rejected";
                var updated = await _requestRepository.UpdateRequestStatusAsync(id, StatusValue);
                if (!updated)
                    return NotFound(new { message = "Blood request not found." });
                await AddHistory(id, "Sysstem User", StatusValue, "Updated request status");
                return Ok(new { message = "Blood request approved successfully." });
            }
            catch (System.Exception)
            {
                
                throw;
            }
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetRequestById(int id)
        {
            try
            {
                var request = await _requestRepository.GetRequestByIdAsync(id);
                if (request == null)
                    return NotFound(new { message = "Donor not found" });
                return Ok(request);
            }
            catch (System.Exception)
            {
                
                throw;
            }
        }


        [HttpPut("update")]
        public async Task<IActionResult> UpdateRequestInfo([FromBody] BloodRequest updateRequest){
            try
            {
                var existingRequest = await _requestRepository.GetRequestByIdAsync(updateRequest.Id);
                if (existingRequest == null)
                {
                    return NotFound(new { message = "Request not found." });
                }
                existingRequest.Id = updateRequest.Id;
                existingRequest.Description = updateRequest.Description;
                existingRequest.PatientName = updateRequest.PatientName;
                existingRequest.UnitsRequired = updateRequest.UnitsRequired;
                existingRequest.BloodGroup = updateRequest.BloodGroup;
                existingRequest.Location = updateRequest.Location;
                existingRequest.Gender = updateRequest.Gender;
                existingRequest.ContactNumber = updateRequest.ContactNumber;
                existingRequest.UpdatedAt = DateTime.Now;
                existingRequest.UpdatedBy = "Admin";
                await _requestRepository.UpdateRequestDetailsAsync(existingRequest);
                return Ok(new { message = "Request updated successfully" });
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _requestRepository.DeleteRequestAsync(id);
                return Ok();
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        private async Task AddHistory(int requestId, string actionUser, string actionType, string actionNote)
        {
            try{
                var requestHistory = new BloodRequestHistory
                {
                    RequestId = requestId,
                    ActionDate = DateTime.Now,
                    ActionType = actionType,
                    ActionUser = actionUser,
                    ActionNote = actionNote
                };
                await _requestRepository.AddBloodRequestHistoryAsync(requestHistory);
            }
            catch(Exception){
                throw;
            }
        }


       [HttpPost("updateStatus")]
        public async Task<IActionResult> BulkUpdateStatus([FromBody] List<BloodRequestStatusUpdateModel> updates)
        {
            if (updates == null || !updates.Any())
                return BadRequest("No requests provided.");

            var affectedRows = await _requestRepository.BulkUpdateStatusAsync(updates);
            if (affectedRows > 0)
                return Ok(new { message = "Requests updated successfully." });

            return StatusCode(500, "Something went wrong.");
        }



        // [HttpPost("delete")]
        // public async Task<IActionResult> DeleteBloodRequest([FromBody] List<BloodRequest> reqeusts)
        // {
        //     if (reqeusts == null || !reqeusts.Any())
        //         return BadRequest("No requests provided.");

        //     var affectedRows = await _requestRepository.DeleteBloodRequestAsync(reqeusts);
        //     if (affectedRows > 0)
        //         return Ok(new { message = "Requests updated successfully." });

        //     return StatusCode(500, "Something went wrong.");
        // }


        [HttpPost("delete")]
        public async Task<IActionResult> DeleteBloodRequest([FromBody] List<BloodRequest> requests)
        {
            if (requests == null || !requests.Any())
                return BadRequest("No requests provided.");

            var ids = requests.Select(r => r.Id).ToList();
            var affectedRows = await _requestRepository.SoftDeleteBloodRequestsAsync(ids);
            if (affectedRows > 0)
                return Ok(new { message = "Requests updated successfully." });

            return StatusCode(500, "Something went wrong while updating requests.");
        }


    }
}