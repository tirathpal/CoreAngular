﻿using Amazon.Runtime;
using Amazon.SimpleSystemsManagement;
using Amazon.SimpleSystemsManagement.Model;
using Konger.CoreAngular.Model;
using System.Data;
using System.Data.SqlClient;

namespace Konger.CoreAngular.DAL
{
    public abstract class SQLHelper
    {

        public static string GetDBConnectionString()
        {
            string connectionStr = string.Empty;

            //try
            //{
            //    var creds = new InstanceProfileAWSCredentials();
            //    var ssmClient = new AmazonSimpleSystemsManagementClient(creds);
            //    var request = new GetParameterRequest()
            //    {
            //        Name = "/kongsolution/Prod/ConnectionStr",
            //        WithDecryption = true
            //    };

            //    var response = ssmClient.GetParameterAsync(request).GetAwaiter().GetResult();

            //    if (response.Parameter != null)
            //    {
            //        connectionStr = response.Parameter.Value;

            //    }

            //}
            //catch
            //{

            //}

            if (string.IsNullOrEmpty(connectionStr))
                connectionStr = @"Data Source=kongsqldb.cugvbjkpc2us.ca-central-1.rds.amazonaws.com;Initial Catalog=awsdb;Integrated Security=false;User ID=wjkong;Password=Wj730615!";

            return connectionStr;
        }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public static SqlCommand CreateCommand(string comText)
        {
            var command = new SqlCommand
            {
                CommandText = comText,
                CommandType = CommandType.StoredProcedure,
                Connection = GetConnection()

            };

            return command;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public static void CreateCommand(SqlCommand com, string comText)
        {
            com.CommandText = comText;
            com.CommandType = CommandType.StoredProcedure;
            com.Connection = GetConnection();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public static void CreateQuery(SqlCommand com, string comText)
        {
            com.CommandText = comText;
            com.CommandType = CommandType.Text;
            com.Connection = GetConnection();
        }

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(GetDBConnectionString());
        }

        public static SqlParameter PrepareOutputParam(SqlCommand com, string param)
        {
            var parameter = new SqlParameter(param, SqlDbType.Int)
            {
                Direction = ParameterDirection.Output,
                Value = 0
            };

            com.Parameters.Add(parameter);
            return parameter;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public static DataSet PrepareTables(string[] comText)
        {
            var dataSet = new DataSet();
            var com = new SqlCommand();

            using (var adapter = new SqlDataAdapter())
            {
                for (int i = 0; i < comText.Length; i++)
                {
                    if (i == Utility.FIRST_TIME)
                    {
                        CreateCommand(com, comText[i]);
                        com.Connection.Open();
                    }
                    else
                    {
                        com.CommandText = comText[i];
                    }
                    adapter.SelectCommand = com;
                    adapter.Fill(dataSet, comText[i]);
                }
            }

            com.Connection.Close();
            return dataSet;
        }
    }
}
