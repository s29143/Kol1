using Kol1.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Kol1.Medicaments;

[ApiController]
[Route("/api/medicament")]
public class MedicamentsController : ControllerBase
{
    private readonly IMedicamentService _medicamentService;

    public MedicamentsController(IMedicamentService medicamentService)
    {
        _medicamentService = medicamentService;
    }


    [HttpGet("/api/medicament/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        try
        {
            var medicament = await _medicamentService.GetById(id);
            return Ok(medicament);
        }
        catch (NotFoundException e)
        {
            return NotFound();
        }
    }
}