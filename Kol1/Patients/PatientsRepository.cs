using System.Data.SqlClient;

namespace Kol1.Medicaments;

public interface IPatientsRepository
{
    Task<Patient?> GetById(int id);
    Task<bool> Delete(int id);
}


public class PatientsRepository : IPatientsRepository
{
    private readonly IConfiguration _configuration;
    public PatientsRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public async Task<Patient?> GetById(int id)
    {
        await using var connection = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        await connection.OpenAsync();
        var query = "SELECT IdPatient, FirstName, LastName, BirthDate FROM s29143.Medicament WHERE id=@Id";
        await using var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("Id", id);
        var reader = await command.ExecuteReaderAsync();
        reader.Read();
        return new Patient
        {
            IdPatient = (int)reader["IdMedicament"],
            FirstName = reader["Name"].ToString()!,
            LastName = reader["Description"].ToString()!,
            BirthDate= (DateOnly)reader["BirthDate"]
        };
    }
    
    
    public async Task<bool> Delete(int id)
    {
        await using var connection = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        await connection.OpenAsync();
        await using var transaction = await connection.BeginTransactionAsync();
        try
        {
            await using var command = new SqlCommand("DELETE FROM s29143.Patient WHERE IdPatient=@Id", connection);
            command.Transaction = (SqlTransaction)transaction;
            command.Parameters.AddWithValue("Id", id);
            await command.ExecuteNonQueryAsync();
            command.CommandText = "DELETE FROM s29143.Prescription WHERE IdPatient=@Id";
            command.Parameters.Clear();
            command.Parameters.AddWithValue("Id", id);
            int numbers = await command.ExecuteNonQueryAsync();
            await transaction.CommitAsync();
            return numbers > 0;
        }
        catch
        {
            await transaction.RollbackAsync();
            return false;
        }
    }
}