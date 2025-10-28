using System.Data;
using System.Data.SqlClient;
using tasksAction.Conn;
using tasksAction.Models;

namespace tasksAction.Data
{
    public class CustomerData
    {

        #region GetCustomerExecon SP_TrackPoint_SelAccountIvanti
        public static async Task<CustomerExecon> GetCustomerId(CustomerExecon parametros, string server)
        {
            Connection cn = new Connection();
            CustomerExecon customerModel = new CustomerExecon();
            SqlConnection sql = new SqlConnection();
            string spName = "";

            if (server == "XCNIVANTIP")
            {
                spName = "EXsp_TrackPoint_SelAccountIvanti";
                sql = new SqlConnection(cn.SqlCommITSMPRO());
            }
            else
            {
                spName = "SP_TrackPoint_SelAccountIvanti";
                sql = new SqlConnection(cn.SqlComm());
            }

            //switch (server)
            //{
            //    case "CARSA-HEAT2015":
            //        spName = "SP_TrackPoint_SelAccountIvanti";
            //        sql = new SqlConnection(cn.SqlComm());
            //    break;
            //    case "XCNIVANTIP":
            //        spName = "EXsp_TrackPoint_SelAccountIvanti";
            //        sql = new SqlConnection(cn.SqlCommITSMPRO
            //    break;
            //}

            try
            {
                customerModel = parametros;
                //using (var sql = new SqlConnection(cn.SqlComm()))
                using ( sql )
                {
                    //using (var cmd = new SqlCommand("SP_TrackPoint_SelAccountIvanti", sql))
                    using (var cmd = new SqlCommand(spName, sql))
                    {
                        await sql.OpenAsync();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CustId", customerModel.client_IdCustomer);
                        using (var item = await cmd.ExecuteReaderAsync())
                        {
                            while (await item.ReadAsync())
                            {
                                customerModel.recId             = Convert.ToString(item["recId"]);
                                customerModel.nameCustomer      = Convert.ToString(item["name"]);
                                customerModel.mailAdminTP       = Convert.ToString(item["mail"]);
                                customerModel.uuidAdminTP       = Convert.ToString(item["user_uuid_id"]);
                                customerModel.contactCustomer   = Convert.ToString(item["contact"]);
                                customerModel.addressCustomer   = Convert.ToString(item["address"]);
                                customerModel.telephoneCustomer = Convert.ToString(item["telephone"]);
                                customerModel.email             = Convert.ToString(item["email"]);
                                customerModel.init              = Convert.ToBoolean(item["init"]);
                                customerModel.close             = Convert.ToBoolean(item["close"]);
                                customerModel.scheduled         = Convert.ToBoolean(item["scheduled"]);
                                customerModel.daybefore         = Convert.ToBoolean(item["daybefore"]);
                                customerModel.send_ics          = Convert.ToBoolean(item["send_ics"]);
                                //customerModel.client_IdCustomer    = Convert.ToString(item["client_id"]);
                            }
                        }
                        await sql.CloseAsync();
                    }
                }
                return customerModel;
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return customerModel;
            }
        }
        #endregion
    }
}
