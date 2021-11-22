using System;

namespace DataImporter.Core
{
    public class Company
    {
        public int Company_ID { get; set; }
        public String Company_Name { get; set; }
        public override string ToString()
        {
            return string.Format("Company with ID: {0}, Name: {1}",
                Company_ID, Company_Name);
        }
    }
}
