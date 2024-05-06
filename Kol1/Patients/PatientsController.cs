using Kol1.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Kol1.Medicaments;

[ApiController]
[Route("/api/patients")]
public class PatientsController : ControllerBase
{
    private readonly IPatientsService _patientsService;

    public PatientsController(IPatientsService patientsService)
    {
        _patientsService = patientsService;
    }


    [HttpDelete("/api/patients/{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        try
        {
            var patient = await _patientsService.Delete(id);
            return patient ? NoContent() : Conflict();
        }
        catch (NotFoundException e)
        {
            return NotFound();
        }
    }
}