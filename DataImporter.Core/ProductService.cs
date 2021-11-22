using DataImporter.Core.Abstractions;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DataImporter.Core
{
    public class ProductService : IProductService
    {
        private readonly IConfiguration configuration;
        public string ConnectionString { get; }
        public string TestData { get; }
        public string FilesFilePath { get; }

        public ProductService(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.ConnectionString = configuration.GetSection("ConnectionString").Value;
            this.TestData = configuration.GetSection("TestData").Value;
        }

        public List<Product> GetProducts()
        {
            List<Product> products = new List<Product>();

            using SqlConnection con = new SqlConnection(ConnectionString);
            using SqlCommand cmd = new SqlCommand("GetProducts", con);
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            try
            {
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Product product = new Product
                    {
                        Name = reader[0].ToString(),
                        Description = reader[1].ToString(),
                        Brand = reader[2].ToString(),
                        UniqueId = reader[3].ToString()
                    };
                    products.Add(product);
                    Console.WriteLine(product.ToString());
                }
            }
                        finally
            {
                con.Close();
            }

            return products;
        }
    }
}
