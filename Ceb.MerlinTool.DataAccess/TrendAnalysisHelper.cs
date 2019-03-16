using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ceb.MerlinTool.DataAccess
{
    public class TrendAnalysisHelper
    {
        private static string _strConnString = ConfigurationManager.ConnectionStrings["ApplicationConnStr"].ConnectionString;
        private static int commandTimeout = Convert.ToInt32(ConfigurationManager.AppSettings["CommandTimeout"]);

        public DataSet GetAllTimePeriod(int companyId)
        {
            SqlParameter[] sqlParams = new SqlParameter[1];
            sqlParams[0] = new SqlParameter("@CompanyId", companyId);
            return SqlHelper.ExecuteDataset(_strConnString, CommandType.StoredProcedure, "SP_TimePeriodForMember", sqlParams);
        }

        public DataSet GetOverallScore(int companyId, int categoryId, int supplierId, string timePeriod)
        {
            DataSet dsResult = new DataSet();
            SqlConnection sqlConnection = new SqlConnection(_strConnString);
            SqlCommand cmd = new SqlCommand();
       
            SqlDataAdapter adapter;
            cmd.CommandTimeout = commandTimeout;
            cmd.CommandText = "SP_GetOverallScoreByTimeperiod";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@CompanyId", SqlDbType.Int).Value = companyId;
            cmd.Parameters.Add("@CategoryId", SqlDbType.Int).Value = categoryId;
            cmd.Parameters.Add("@SupplierId", SqlDbType.Int).Value = supplierId;
            cmd.Parameters.Add("@TimePeriod", SqlDbType.NVarChar).Value = timePeriod.Equals("All", StringComparison.OrdinalIgnoreCase) ? string.Empty : timePeriod;
            cmd.Connection = sqlConnection;

            sqlConnection.Open();

            adapter = new SqlDataAdapter(cmd);
            adapter.Fill(dsResult);
            sqlConnection.Close();
            return dsResult;
            //SqlParameter[] sqlParams = new SqlParameter[4];
            //sqlParams[0] = new SqlParameter("@CompanyId", companyId);
            //sqlParams[1] = new SqlParameter("@CategoryId", categoryId);
            //sqlParams[2] = new SqlParameter("@SupplierId", supplierId);
            //sqlParams[3] = new SqlParameter("@TimePeriod", timePeriod.Equals("All", StringComparison.OrdinalIgnoreCase) ? string.Empty : timePeriod);
            //return SqlHelper.ExecuteDataset(_strConnString, CommandType.StoredProcedure, "SP_GetOverallScoreByTimeperiod", sqlParams);
        }

        public DataSet GetMetricsScore(int companyId, int categoryId, int supplierId, string timePeriod)
        {
            DataSet dsResult = new DataSet();
            SqlConnection sqlConnection = new SqlConnection(_strConnString);
            SqlCommand cmd = new SqlCommand();

            SqlDataAdapter adapter;
            cmd.CommandTimeout = commandTimeout;
            cmd.CommandText = "SP_GetMetricsScoreByTimeperiod";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@CompanyId", SqlDbType.Int).Value = companyId;
            cmd.Parameters.Add("@CategoryId", SqlDbType.Int).Value = categoryId;
            cmd.Parameters.Add("@SupplierId", SqlDbType.Int).Value = supplierId;
            cmd.Parameters.Add("@TimePeriod", SqlDbType.NVarChar).Value = timePeriod.Equals("All", StringComparison.OrdinalIgnoreCase) ? string.Empty : timePeriod;
            cmd.Connection = sqlConnection;

            sqlConnection.Open();

            adapter = new SqlDataAdapter(cmd);
            adapter.Fill(dsResult);
            sqlConnection.Close();
            return dsResult;

            //SqlParameter[] sqlParams = new SqlParameter[4];
            //sqlParams[0] = new SqlParameter("@CompanyId", companyId);
            //sqlParams[1] = new SqlParameter("@CategoryId", categoryId);
            //sqlParams[2] = new SqlParameter("@SupplierId", supplierId);
            //sqlParams[3] = new SqlParameter("@TimePeriod", timePeriod.Equals("All", StringComparison.OrdinalIgnoreCase) ? string.Empty : timePeriod);
            //return SqlHelper.ExecuteDataset(_strConnString, CommandType.StoredProcedure, "SP_GetMetricsScoreByTimeperiod", sqlParams);
        }

        public DataSet GetKPIScore(int companyId, int categoryId, int supplierId, string timePeriod)
        {
            DataSet dsResult = new DataSet();
            SqlConnection sqlConnection = new SqlConnection(_strConnString);
            SqlCommand cmd = new SqlCommand();

            SqlDataAdapter adapter;
            cmd.CommandTimeout = commandTimeout;
            cmd.CommandText = "SP_GetKPIScoreByTimeperiod";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@CompanyId", SqlDbType.Int).Value = companyId;
            cmd.Parameters.Add("@CategoryId", SqlDbType.Int).Value = categoryId;
            cmd.Parameters.Add("@SupplierId", SqlDbType.Int).Value = supplierId;
            cmd.Parameters.Add("@TimePeriod", SqlDbType.NVarChar).Value = timePeriod.Equals("All", StringComparison.OrdinalIgnoreCase) ? string.Empty : timePeriod;
            cmd.Connection = sqlConnection;

            sqlConnection.Open();

            adapter = new SqlDataAdapter(cmd);
            adapter.Fill(dsResult);
            sqlConnection.Close();
            return dsResult;

            //SqlParameter[] sqlParams = new SqlParameter[4];
            //sqlParams[0] = new SqlParameter("@CompanyId", companyId);
            //sqlParams[1] = new SqlParameter("@CategoryId", categoryId);
            //sqlParams[2] = new SqlParameter("@SupplierId", supplierId);
            //sqlParams[3] = new SqlParameter("@TimePeriod", timePeriod.Equals("All", StringComparison.OrdinalIgnoreCase) ? string.Empty : timePeriod);
            //return SqlHelper.ExecuteDataset(_strConnString, CommandType.StoredProcedure, "SP_GetKPIScoreByTimeperiod", sqlParams);
        }
    }
}
