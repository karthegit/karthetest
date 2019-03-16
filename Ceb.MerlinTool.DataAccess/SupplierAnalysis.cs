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
    public class SupplierAnalysis
    {

        private static string _strConnString = ConfigurationManager.ConnectionStrings["ApplicationConnStr"].ConnectionString;

        public DataSet GetSupplierScore(int companyId, int categoryId, int supplierId)
        {
            SqlParameter[] sqlParams = new SqlParameter[3];
            sqlParams[0] = new SqlParameter("@CompanyId", companyId);
            sqlParams[1] = new SqlParameter("@CategoryId", categoryId);
            sqlParams[2] = new SqlParameter("@SupplierId", supplierId);
            return SqlHelper.ExecuteDataset(_strConnString, CommandType.StoredProcedure, "SP_GetSupplierOverallScore", sqlParams);
        }

        public DataSet GetSupplierScoreByCategory(int companyId, int categoryId, int supplierId, int demographicId, string demographicSelect, string demographicIdColn)
        {
            SqlParameter[] sqlParams = new SqlParameter[6];
            sqlParams[0] = new SqlParameter("@CompanyId", companyId);
            sqlParams[1] = new SqlParameter("@CategoryId", categoryId);
            sqlParams[2] = new SqlParameter("@SupplierId", supplierId);
            sqlParams[3] = new SqlParameter("@DemographicSelectColumn", demographicSelect);
            sqlParams[4] = new SqlParameter("@DemographicIdColumn", demographicIdColn);
            sqlParams[5] = new SqlParameter("@DemographicId", demographicId);
            return SqlHelper.ExecuteDataset(_strConnString, CommandType.StoredProcedure, "SP_GetSupplierScoreByDemographic", sqlParams);
        }

        public DataSet GetSupplierMetricScore(int companyId, int categoryId, int supplierId)
        {
            SqlParameter[] sqlParams = new SqlParameter[3];
            sqlParams[0] = new SqlParameter("@CompanyId", companyId);
            sqlParams[1] = new SqlParameter("@CategoryId", categoryId);
            sqlParams[2] = new SqlParameter("@SupplierId", supplierId);
            return SqlHelper.ExecuteDataset(_strConnString, CommandType.StoredProcedure, "SP_GetSupplierMetricScore", sqlParams);
        }

        public DataSet GetSupplierMetricScoreByCategory(int companyId, int categoryId, int supplierId, int demographicId, string demographicSelect, string demographicIdColn)
        {
            SqlParameter[] sqlParams = new SqlParameter[6];
            sqlParams[0] = new SqlParameter("@CompanyId", companyId);
            sqlParams[1] = new SqlParameter("@CategoryId", categoryId);
            sqlParams[2] = new SqlParameter("@SupplierId", supplierId);
            sqlParams[3] = new SqlParameter("@DemographicSelectColumn", demographicSelect);
            sqlParams[4] = new SqlParameter("@DemographicIdColumn", demographicIdColn);
            sqlParams[5] = new SqlParameter("@DemographicId", demographicId);
            return SqlHelper.ExecuteDataset(_strConnString, CommandType.StoredProcedure, "SP_GetSupplierMetricScoreByDemographic", sqlParams);
        }

        public DataSet GetSupplierKPIScore(int companyId, int categoryId, int supplierId)
        {
            SqlParameter[] sqlParams = new SqlParameter[3];
            sqlParams[0] = new SqlParameter("@CompanyId", companyId);
            sqlParams[1] = new SqlParameter("@CategoryId", categoryId);
            sqlParams[2] = new SqlParameter("@SupplierId", supplierId);
            return SqlHelper.ExecuteDataset(_strConnString, CommandType.StoredProcedure, "SP_GetSupplierKPIScore", sqlParams);
        }

        public DataSet GetSupplierKPIScoreByCategory(int companyId, int categoryId, int supplierId, int demographicId, string demographicIdColn ,string demographicSelect)
        {
            SqlParameter[] sqlParams = new SqlParameter[6];
            sqlParams[0] = new SqlParameter("@CompanyId", companyId);
            sqlParams[1] = new SqlParameter("@CategoryId", categoryId);
            sqlParams[2] = new SqlParameter("@SupplierId", supplierId);
            sqlParams[3] = new SqlParameter("@DemographicIdColumn", demographicIdColn);
            sqlParams[4] = new SqlParameter("@DemographicId", demographicId);
            sqlParams[5] = new SqlParameter("@DemographicSelectColumn", demographicSelect);
            return SqlHelper.ExecuteDataset(_strConnString, CommandType.StoredProcedure, "SP_GetSupplierKPIScoreByDemographic", sqlParams);
        }

        public DataSet GetSupplierComments(int companyId, int categoryId, int supplierId)
        {
            SqlParameter[] sqlParams = new SqlParameter[3];
            sqlParams[0] = new SqlParameter("@CompanyId", companyId);
            sqlParams[1] = new SqlParameter("@CategoryId", categoryId);
            sqlParams[2] = new SqlParameter("@SupplierId", supplierId);
            return SqlHelper.ExecuteDataset(_strConnString, CommandType.StoredProcedure, "SP_GetSupplierComments", sqlParams);
        }

        public DataSet GetSupplierComments(int companyId, int categoryId, int supplierId, int demographicId, string demographicIdColn, string demographicSelect)
        {
            SqlParameter[] sqlParams = new SqlParameter[6];
            sqlParams[0] = new SqlParameter("@CompanyId", companyId);
            sqlParams[1] = new SqlParameter("@CategoryId", categoryId);
            sqlParams[2] = new SqlParameter("@SupplierId", supplierId);
            sqlParams[3] = new SqlParameter("@DemographicIdColumn", demographicIdColn);
            sqlParams[4] = new SqlParameter("@DemographicId", demographicId);
            sqlParams[5] = new SqlParameter("@DemographicSelectColumn", demographicSelect);

            return SqlHelper.ExecuteDataset(_strConnString, CommandType.StoredProcedure, "SP_GetSupplierCommentsByDemographic", sqlParams);
        }
    }
}
