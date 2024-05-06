namespace Kol1.Medicaments;

public interface IMedicamentRepository
{
    Task<Medicament?> GetById(int id);
}


public class MedicamentRepository : IMedicamentRepository
{
    public async Task<Medicament?> GetById(int id)
    {
        
    }
}