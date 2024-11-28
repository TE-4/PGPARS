using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using PGPARS.Data;
using PGPARS.Models;
using System.Globalization;

namespace PGPARS.Services
{
    public class CsvService
    {
        public IEnumerable<Applicant> ReadCsvFile(Stream fileStream)
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
                    // add the custom class mapping for Applicant
                    csv.Context.RegisterClassMap<ApplicantMap>();

                    // get the records with the CsvReader and return them as a list
                    var records = csv.GetRecords<Applicant>();
                    return records.ToList();
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
    }
}
