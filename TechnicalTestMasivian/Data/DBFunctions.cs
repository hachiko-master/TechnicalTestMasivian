using System.Data.SqlClient;
using System.Data;
using System;
using System.Collections.Generic;
using TechnicalTestMasivian.Models;
using Microsoft.Extensions.Configuration;

namespace TechnicalTestMasivian.Data
{
    public class DBFunctions
    {
        SqlConnection sqlConnection = new SqlConnection(new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("ConfigGlobalValues")["ConnectionString"].ToString());
        public List<Roulette> GetAllRoulettes()
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

        public int CreateRoulette()
        {
            try
            {
                int rouleteId = 0;
                SqlCommand cmd = new SqlCommand("SP_CREATE_ROULETTE", sqlConnection);
                cmd.CommandType = CommandType.StoredProcedure;
                sqlConnection.Open();
                SqlDataReader sqlDataReader = cmd.ExecuteReader();
                List<Roulette> listRoulettes = new List<Roulette>();
                while (sqlDataReader.Read())
                {
                    rouleteId = Convert.ToInt32(sqlDataReader[0]);
                }
                sqlConnection.Close();

                return rouleteId;
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

        public int OpenRoulette(int rouletteId)
        {
            try
            {
                int result = 0;
                SqlCommand cmd = new SqlCommand("SP_OPEN_ROULETTE", sqlConnection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@rouletteId", rouletteId);
                sqlConnection.Open();
                SqlDataReader sqlDataReader = cmd.ExecuteReader();
                List<Roulette> listRoulettes = new List<Roulette>();
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

        public int CreateBet(int rouletteId, int userId, int betOption, double betMoney)
        {
            try
            {
                int result = 0;
                SqlCommand cmd = new SqlCommand("SP_CREATE_BET_ROULETTE", sqlConnection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@rouletteId", rouletteId);
                cmd.Parameters.AddWithValue("@userId", userId);
                cmd.Parameters.AddWithValue("@betOption", betOption);
                cmd.Parameters.AddWithValue("@betMoney", betMoney); 
                sqlConnection.Open();
                SqlDataReader sqlDataReader = cmd.ExecuteReader();
                List<Roulette> listRoulettes = new List<Roulette>();
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
