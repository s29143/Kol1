using Kol1.Exceptions;

namespace Kol1.Medicaments;

public interface IMedicamentService
{
    Task<Medicament> GetById(int id);
}


public class MedicamentService : IMedicamentService
{
    private readonly IMedicamentRepository _medicamentRepository;

    public MedicamentService(IMedicamentRepository medicamentRepository)
    {
        _medicamentRepository = medicamentRepository;
    }

    public async Task<Medicament> GetById(int id)
    {
        var medicament = await _medicamentRepository.GetById(id);
        if (medicament is null)
            throw new NotFoundException("Medication not found");
        return medicament;
    }
}