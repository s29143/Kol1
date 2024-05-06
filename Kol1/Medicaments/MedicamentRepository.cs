using System.Data.SqlClient;

namespace Kol1.Medicaments;

public interface IMedicamentRepository
{
    Task<Medicament?> GetById(int id);
}


public class MedicamentRepository : IMedicamentRepository
{
    private readonly IConfiguration _configuration;
    public MedicamentRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<Medicament?> GetById(int id)
    {
        await using var connection = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        await connection.OpenAsync();
        var query = "SELECT IdMedicament, Name, Description, TYPE FROM [s29143].Medicament WHERE IdMedicament=@Id";
        await using var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("Id", id);
        var reader = await command.ExecuteReaderAsync();
        if (reader.Read())
        {
            Medicament medicament = new Medicament
            {
                IdMedicament = (int)reader["IdMedicament"],
                Name = reader["Name"].ToString()!,
                Description = reader["Description"].ToString()!,
                Type = reader["Type"].ToString()!
            };

            command.CommandText =
                "SELECT IdMedicament, IdPrescription, Dose, Details FROM [s29143].Prescription_Medicament WHERE IdMedicament=@Id";
            command.Parameters.Clear();
            command.Parameters.AddWithValue("Id", id);
            List<Prescription.Prescription> prescriptions = new List<Prescription.Prescription>();
            reader = await command.ExecuteReaderAsync();
            while (reader.Read())
            {
                prescriptions.Add(
                        new Prescription.Prescription
                        {
                            IdPatient = (int)reader["IdPatient"],
                            IdPrescription = (int)reader["IdPrescription"],
                            IdDoctor = (int)reader["IdDoctor"],
                            Date = (DateOnly)reader["Date"],
                            DueDate = (DateOnly)reader["Date"]
                        }
                    );
            }

            medicament.Prescriptions = prescriptions;


            return medicament;
        }

        return null;
    }
}