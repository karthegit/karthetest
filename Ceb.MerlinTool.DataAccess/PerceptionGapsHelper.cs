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
    public class PerceptionGapsHelper
    {
        private static string _strConnString = ConfigurationManager.ConnectionStrings["ApplicationConnStr"].ConnectionString;

        //need to be removed in next release

        public int CheckIfPerceptionGapDataExists(int companyId)
        {
            SqlParameter[] sqlParams = new SqlParameter[1];
            sqlParams[0] = new SqlParameter("@CompanyId", companyId);
            return Convert.ToInt16(SqlHelper.ExecuteScalar(_strConnString, CommandType.StoredProcedure, "SP_CheckIfPerceptionGapDataExists", sqlParams));
        }

        public DataSet GetSupplierScore(int companyId, int categoryId, int supplierId)
        {
            SqlParameter[] sqlParams = new SqlParameter[3];
            sqlParams[0] = new SqlParameter("@CompanyId", companyId);
            sqlParams[1] = new SqlParameter("@CategoryId", categoryId);
            sqlParams[2] = new SqlParameter("@SupplierId", supplierId);
            return SqlHelper.ExecuteDataset(_strConnString, CommandType.StoredProcedure, "SP_GetOverallScorePerceptionGap", sqlParams);
        }

        public DataSet GetSupplierMetricScore(int companyId, int categoryId, int supplierId)
        {
            SqlParameter[] sqlParams = new SqlParameter[3];
            sqlParams[0] = new SqlParameter("@CompanyId", companyId);
            sqlParams[1] = new SqlParameter("@CategoryId", categoryId);
            sqlParams[2] = new SqlParameter("@SupplierId", supplierId);
            return SqlHelper.ExecuteDataset(_strConnString, CommandType.StoredProcedure, "SP_GetMetricScorePerceptionGap", sqlParams);
        }

        public DataSet GetSupplierKPIScore(int companyId, int categoryId, int supplierId)
        {
            SqlParameter[] sqlParams = new SqlParameter[3];
            sqlParams[0] = new SqlParameter("@CompanyId", companyId);
            sqlParams[1] = new SqlParameter("@CategoryId", categoryId);
            sqlParams[2] = new SqlParameter("@SupplierId", supplierId);
            return SqlHelper.ExecuteDataset(_strConnString, CommandType.StoredProcedure, "SP_GetKPIScorePerceptionGap", sqlParams);
        }

        public DataSet GetSupplierComments(int companyId, int categoryId, int supplierId)
        {
            SqlParameter[] sqlParams = new SqlParameter[3];
            sqlParams[0] = new SqlParameter("@CompanyId", companyId);
            sqlParams[1] = new SqlParameter("@CategoryId", categoryId);
            sqlParams[2] = new SqlParameter("@SupplierId", supplierId);
            return SqlHelper.ExecuteDataset(_strConnString, CommandType.StoredProcedure, "SP_GetSupplierComments", sqlParams);
        }
    }
}
