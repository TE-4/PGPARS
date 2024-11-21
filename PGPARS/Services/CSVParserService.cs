﻿using PGPARS.Models;

public class CSVParserService
{
    public List<Applicant> ParseApplicantsFromCsv(Stream fileStream)
    {
        var applicants = new List<Applicant>();

        using (var stream = new StreamReader(fileStream))
        {
            string? line;
            int lineNumber = 0;

            while ((line = stream.ReadLine()) != null)
            {
                lineNumber++;

                // Skip header row
                if (lineNumber == 1) continue;

                var values = line.Split(',');

                if (values.Length < 3) continue; 

                try
                {
                    var applicant = new Applicant
                    {
                        Nnumber = values[0].Trim(),
                        FirstName = values[1].Trim(),
                        LastName = values[2].Trim()


                    };
                    applicants.Add(applicant);
                }
                catch
                {
                    // Handle error parsing a single line (log or collect errors)
                }
            }
        }

        return applicants;
    }
}
