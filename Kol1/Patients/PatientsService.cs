using Kol1.Exceptions;

namespace Kol1.Medicaments;

public interface IPatientsService
{
    Task<Patient> GetById(int id);
    Task<bool> Delete(int id);
}


public class PatientsService : IPatientsService
{
    private readonly IPatientsRepository _patientsRepository;

    public PatientsService(IPatientsRepository patientsRepository)
    {
        _patientsRepository = patientsRepository;
    }

    public async Task<Patient> GetById(int id)
    {
        var patient = await _patientsRepository.GetById(id);
        if (patient is null)
            throw new NotFoundException("Patient not found given id=" + id);
        return patient;
    }

    public async Task<bool> Delete(int id)
    {
        var patient = this.GetById(id);
        return await _patientsRepository.Delete(id);
    }
}