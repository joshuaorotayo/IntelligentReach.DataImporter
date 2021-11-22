using DataImporter.Core.Abstractions;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace DataImporter.Core
{
    public class DataImporterService : IDataImporterService
    {
        private readonly IConfiguration configuration;
        public string ConnectionString { get; }
        public string TestData { get; }

        public DataImporterService(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.ConnectionString = configuration.GetSection("ConnectionString").Value;
            this.TestData = configuration.GetSection("TestData").Value;
        }

        #region Adders
        public void AddCompanys()
        {
            using StreamReader sr = new StreamReader(Directory.GetCurrentDirectory() + "..\\..\\..\\..\\..\\Company.csv");
            string[] headers = sr.ReadLine().Split(',');
            while (!sr.EndOfStream)
            {
                string[] rows = sr.ReadLine().Split(',');
                for (int i = 0; i < headers.Length; i++)
                {
                    using SqlConnection con = new SqlConnection(ConnectionString);
                    using SqlCommand cmd = new SqlCommand("AddCompanys", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Company_ID", SqlDbType.Int).Value = rows[i];i++;
                    cmd.Parameters.Add("@Company_Name", SqlDbType.VarChar, -1).Value = rows[i];
                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }

                    catch (Exception)
                    {
                    }
                }
            }

        }

        public void AddFeeds()
        {
            using StreamReader sr = new StreamReader(Directory.GetCurrentDirectory() + "..\\..\\..\\..\\..\\Feed.csv");
            string[] headers = sr.ReadLine().Split(',');
            while (!sr.EndOfStream)
            {
                string[] rows = sr.ReadLine().Split(',');
                for (int i = 0; i < headers.Length; i++)
                {
                    using SqlConnection con = new SqlConnection(ConnectionString);
                    using SqlCommand cmd = new SqlCommand("AddFeeds", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Feed_ID", SqlDbType.Int).Value = rows[i];i++;
                    cmd.Parameters.Add("@Feed_Name", SqlDbType.VarChar, -1).Value = rows[i];
                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }

                    catch (Exception)
                    {
                        //Console.WriteLine(ex.Message);
                    }
                }
            }
        }
        #endregion

        #region Getters
        public List<Feed> GetFeeds()
        {
            List<Feed> feeds = new List<Feed>();
            using SqlConnection con = new SqlConnection(ConnectionString);
            using SqlCommand cmd = new SqlCommand("GetFeeds", con);
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            try
            {

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Feed feed = new Feed
                    {
                        Feed_ID = Int32.Parse(reader[0].ToString()),
                        Feed_Name = reader[1].ToString()
                    };
                    feeds.Add(feed);
                }
            }
            finally
            {
                con.Close();
            }
            return feeds;
        }

        public List<Company> GetCompanies()
        {
            List<Company> companies = new List<Company>();
            using SqlConnection con = new SqlConnection(ConnectionString);
            using SqlCommand cmd = new SqlCommand("GetCompanys", con);
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            try
            {

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Company company = new Company
                    {
                        Company_ID = Int32.Parse(reader[0].ToString()),
                        Company_Name = reader[1].ToString()
                    };
                    companies.Add(company);
                }
            }
            catch (Exception)
            {

            }
            finally
            {
                con.Close();
            }
            return companies;
        }
        #endregion

        #region Useful Functions
        public string ReadData(int company_id, int feed_id)
        {
            var companies = GetCompanies();
            var feeds = GetFeeds();

            bool companyExists = companies.Any(company => company.Company_ID == company_id);
            bool feedExists = feeds.Any(feed => feed.Feed_ID == feed_id);
            if (companyExists && feedExists)
            {
                //takes two numbers as input and returns the data using the Company_id and feed_id
                ClearProducts();
                var filePath = TestData + "Company_" + company_id + "\\Feed_" + feed_id + "\\Data.csv";

                try
                {
                    using StreamReader sr = new StreamReader(filePath);

                    string[] headers = sr.ReadLine().Split(',');
                    while (!sr.EndOfStream)
                    {
                        string[] rows = sr.ReadLine().Split(',');
                        using SqlConnection con = new SqlConnection(ConnectionString);
                        con.Open();
                        try
                        {
                            for (int i = 0; i < headers.Length; i++)
                            {
                                using SqlCommand cmd = new SqlCommand("AddProducts", con);
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add("@Product_UniqueID", SqlDbType.Int).Value = rows[i]; i++;
                                cmd.Parameters.Add("@Product_Name", SqlDbType.VarChar, -1).Value = rows[i]; i++;
                                cmd.Parameters.Add("@Product_Brand", SqlDbType.VarChar, -1).Value = rows[i]; i++;
                                cmd.Parameters.Add("@Product_Description", SqlDbType.VarChar, -1).Value = rows[i]; i++;
                                cmd.Parameters.Add("@Company_ID", SqlDbType.Int).Value = company_id;
                                cmd.Parameters.Add("@Feed_ID", SqlDbType.Int).Value = feed_id;
                                cmd.ExecuteNonQuery();
                            }
                        }

                        catch (Exception)
                        {
                        }
                        finally
                        {
                        con.Close();
                        }
                    }
                }
                catch (Exception)
                {
                    return "Mismatched Company ID and Feed ID";
                }
            }
            else
            {
                return "Company ID or Feed ID do not exist";
            }

            var productService = new ProductService(configuration);
            return JsonConvert.SerializeObject(productService.GetProducts());
                    }

        public void ClearProducts()
        {
            //empties the products table for the next search
            using SqlConnection con = new SqlConnection(ConnectionString);
            using SqlCommand clearCmd = new SqlCommand("ClearProducts", con);
            try
            {
                con.Open();
                clearCmd.ExecuteNonQuery().ToString();
                con.Close();
                //return true;
            }
            catch (Exception)
            {
            //throw;
            }
        }

        public DataTable ConvertCSVtoDataTable(string FilePath)
        {

            DataTable dt = new DataTable();
            using (StreamReader sr = new StreamReader(FilePath))
            {
                string[] headers = sr.ReadLine().Split(',');
                foreach (string header in headers)
                {
                    dt.Columns.Add(header);
                }
                while (!sr.EndOfStream)
                {
                    string[] rows = sr.ReadLine().Split(',');
                    DataRow dr = dt.NewRow();
                    for (int i = 0; i < headers.Length; i++)
                    {
                        dr[i] = rows[i];
                    }
                    dt.Rows.Add(dr);
                }

            }

            return dt;
        }
        #endregion
    }
}
