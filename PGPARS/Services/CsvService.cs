using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using CsvHelper;
using System.Globalization;
using PGPARS.Models;

public class CsvService
{
    public IEnumerable<T> ReadCsvFile<T>(Stream fileStream, ClassMap classMap) where T : class
    {
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            PrepareHeaderForMatch = args => args.Header.ToLower().Trim(),
            MissingFieldFound = null
        };

        try
        {
            using (var reader = new StreamReader(fileStream))
            using (var csv = new CsvReader(reader, config))
            {
                csv.Context.RegisterClassMap(classMap);

                // Read all records and filter out any where key fields are missing
                var records = csv.GetRecords<T>()
                                 .Where(r => !IsEmptyRecord(r)) // Ensure only valid rows are returned
                                 .ToList();
                return records;
            }
        }
        catch (HeaderValidationException ex)
        {
            throw new ApplicationException("CSV file header is invalid.", ex);
        }
        catch (TypeConverterException ex)
        {
            throw new ApplicationException("CSV file contains invalid data format.", ex);
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Error reading CSV file", ex);
        }
    }

    // Function to check if a record is empty
    private bool IsEmptyRecord<T>(T record)
    {
        if (record == null) return true;

        if(record is Applicant applicant)
        {
            return string.IsNullOrWhiteSpace(applicant.Nnumber);
        }

        return false;
    }
}
