using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System;
using System.Collections.Generic;
using TechnicalTestMasivian.Entities;
using Microsoft.Extensions.Configuration;

namespace TechnicalTestMasivian.Data
{
    public class DBFunctions
    {
        SqlConnection sqlConnection = new SqlConnection(new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("ConfigGlobalValues")["ConnectionString"].ToString());
        public List<Roulette> GetAllRoulletes()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("SP_GET_ALL_ROULLETES", sqlConnection);
                cmd.CommandType = CommandType.StoredProcedure;
                sqlConnection.Open();
                SqlDataReader sqlDataReader = cmd.ExecuteReader();
                List<Roulette> listRoulettes = new List<Roulette>();
                Roulette roulette;
                while (sqlDataReader.Read())
                {
                    roulette = new Roulette();
                    roulette.Id = Convert.ToInt32(sqlDataReader[0]);
                    roulette.Status = Convert.ToBoolean(sqlDataReader[1]);
                    roulette.CreateDateTime = Convert.ToDateTime(sqlDataReader[2].ToString());
                    listRoulettes.Add(roulette);
                }
                sqlConnection.Close();

                return listRoulettes;
            }
            catch (Exception e)
            {
                sqlConnection.Close();
                throw;
            }
            finally
            {
                sqlConnection.Close();
            }
        }

        public int CreateRoullete()
        {
            try
            {
                int result = 0;
                SqlCommand cmd = new SqlCommand("SP_CREATE_ROULETTE", sqlConnection);
                cmd.CommandType = CommandType.StoredProcedure;
                sqlConnection.Open();
                SqlDataReader sqlDataReader = cmd.ExecuteReader();
                List<Roulette> listRoulettes = new List<Roulette>();
                Roulette roulette;
                while (sqlDataReader.Read())
                {
                    result = Convert.ToInt32(sqlDataReader[0]);
                }
                sqlConnection.Close();

                return result;
            }
            catch (Exception e)
            {
                sqlConnection.Close();
                throw;
            }
            finally
            {
                sqlConnection.Close();
            }
        }
    }
}
