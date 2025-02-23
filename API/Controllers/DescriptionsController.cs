using API.DTOs;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class DescriptionsController(IDescriptionRepository descriptionRepository) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DescriptionDto>>> GetDescriptions()
    {
        var descriptions = await descriptionRepository.GetDescriptionsAsync();
        return Ok(descriptions);
    }
}
