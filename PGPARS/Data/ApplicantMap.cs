﻿using CsvHelper.Configuration;
using PGPARS.Models;

namespace PGPARS.Data
{
    public class ApplicantMap : ClassMap<Applicant>
    {
        public ApplicantMap() 
        {
            Map(m => m.Nnumber).Name("N# or Slate ID");
            Map(m => m.FirstName).Name("FirstName");
            Map(m => m.LastName).Name("LastName");
            Map(m => m.email).Name("Email");
            Map(m => m.Phone).Name("Cell phone");
            
        }

    }
}