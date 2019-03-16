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
    public class LookupHelper
    {
        private static string _strConnString = ConfigurationManager.ConnectionStrings["ApplicationConnStr"].ConnectionString;

        public DataSet GetCategory(int companyId)
        {
            SqlParameter[] sqlParams = new SqlParameter[1];
            sqlParams[0] = new SqlParameter("@CompanyId", companyId);
            return SqlHelper.ExecuteDataset(_strConnString, CommandType.StoredProcedure, "SP_GetCategory", sqlParams);
        }

        public DataSet GetMetric(int companyId,int categoryId)
        {
            SqlParameter[] sqlParams = new SqlParameter[2];
            sqlParams[0] = new SqlParameter("@CompanyId", companyId);
            sqlParams[1] = new SqlParameter("@CategoryId", categoryId);
            return SqlHelper.ExecuteDataset(_strConnString, CommandType.StoredProcedure, "SP_GetMetric", sqlParams);
        }

        public DataSet GetKPI(int companyId,int categoryId,int metricId)
        {
            SqlParameter[] sqlParams = new SqlParameter[3];
            sqlParams[0] = new SqlParameter("@CompanyId", companyId);
            sqlParams[1] = new SqlParameter("@CategoryId", categoryId);
            sqlParams[2] = new SqlParameter("@MetricId", metricId);
            return SqlHelper.ExecuteDataset(_strConnString, CommandType.StoredProcedure, "SP_GetKPI", sqlParams);
        }

        public DataSet GetSupplier(int companyId, int categoryId,int supplierId)
        {
            SqlParameter[] sqlParams = new SqlParameter[3];
            sqlParams[0] = new SqlParameter("@CompanyId", companyId);
            sqlParams[1] = new SqlParameter("@CategoryId", categoryId);
            sqlParams[2] = new SqlParameter("@SupplierId", supplierId);
            return SqlHelper.ExecuteDataset(_strConnString, CommandType.StoredProcedure, "SP_GetSupplier", sqlParams);
        }

        public DataSet GetDemographic(int companyId, int categoryId,int supplierId,string demographicId,string demographicText)
        {
            SqlParameter[] sqlParams = new SqlParameter[5];
            sqlParams[0] = new SqlParameter("@CompanyId", companyId);
            sqlParams[1] = new SqlParameter("@CategoryId", categoryId);
            sqlParams[2] = new SqlParameter("SupplierId", supplierId);
            sqlParams[3] = new SqlParameter("DemographicId", demographicId);
            sqlParams[4] = new SqlParameter("DemographicText", demographicText);
            return SqlHelper.ExecuteDataset(_strConnString, CommandType.StoredProcedure, "SP_GetDemographic", sqlParams);
        }


    }
}


