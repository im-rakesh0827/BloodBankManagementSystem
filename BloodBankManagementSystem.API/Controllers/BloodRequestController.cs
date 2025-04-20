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

    [HttpPost]
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

    [HttpGet]
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

    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] string status)
    {
        try
        {
            await _repository.UpdateRequestStatusAsync(id, status);
            return Ok(new { message = "Status updated" });
        }
        catch (System.Exception)
        {
            
            throw;
        }
    }
}
}