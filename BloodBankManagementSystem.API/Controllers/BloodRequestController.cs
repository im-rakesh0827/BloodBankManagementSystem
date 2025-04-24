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
    private readonly IBloodRequestRepository _repository;

    public BloodRequestController(IBloodRequestRepository repository)
    {
        _repository = repository;
    }

    [HttpPost("create")]
    public async Task<IActionResult> RequestBlood([FromBody] BloodRequest request)
    {
        try
        {
            await _repository.AddRequestAsync(request);
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
         return Ok(await _repository.GetAllRequestsAsync());
       }
       catch (System.Exception)
       {
        throw;
       }
    }


    [HttpPut("updatestatus/{id}")]
    public async Task<IActionResult> UpdateBloodRequestStatus(int id, [FromQuery] bool status)
    {
        string StatusValue = status?"Approved":"Rejected";
        var updated = await _repository.UpdateRequestStatusAsync(id, StatusValue);
        if (!updated)
            return NotFound(new { message = "Blood request not found." });

        return Ok(new { message = "Blood request approved successfully." });
    }



    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _repository.DeleteRequestAsync(id);
        return Ok();
    }
}
}