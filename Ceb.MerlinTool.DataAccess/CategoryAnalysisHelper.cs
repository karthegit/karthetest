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
    public class CategoryAnalysisHelper
    {
        private static string _strConnString = ConfigurationManager.ConnectionStrings["ApplicationConnStr"].ConnectionString;

        public DataSet GetSupplierScoreByCategory(int companyId, int categoryId)
        {
            SqlParameter[] sqlParams = new SqlParameter[2];
            sqlParams[0] = new SqlParameter("@CompanyId", companyId);
            sqlParams[1] = new SqlParameter("@CategoryId", categoryId);
            return SqlHelper.ExecuteDataset(_strConnString, CommandType.StoredProcedure, "SP_ScoreByCategory", sqlParams);
        }

        public DataSet GetSupplierScoreByCategoryMetric(int companyId, int categoryId, int metric)
        {
            SqlParameter[] sqlParams = new SqlParameter[3];
            sqlParams[0] = new SqlParameter("@CompanyId", companyId);
            sqlParams[1] = new SqlParameter("@CategoryId", categoryId);
            sqlParams[2] = new SqlParameter("@MetricId", metric);
            return SqlHelper.ExecuteDataset(_strConnString, CommandType.StoredProcedure, "SP_ScoreByCategoryMetric", sqlParams);
        }

        public DataSet GetSupplierScoreByCategoryKPIMetric(int companyId, int categoryId, int metric, int kpi)
        {
            SqlParameter[] sqlParams = new SqlParameter[4];
            sqlParams[0] = new SqlParameter("@CompanyId", companyId);
            sqlParams[1] = new SqlParameter("@CategoryId", categoryId);
            sqlParams[2] = new SqlParameter("@MetricId", metric);
            sqlParams[3] = new SqlParameter("@KPIId", kpi);
            return SqlHelper.ExecuteDataset(_strConnString, CommandType.StoredProcedure, "SP_ScoreByCategoryMetricKPI", sqlParams);
        }
    }
}
