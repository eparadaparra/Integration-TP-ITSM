using System.Data;
using System.Data.SqlClient;
using tasksAction.Conn;
using tasksAction.Models;

namespace tasksAction.Data
{
    public class CustomerDataAcc
    {
        Connection cn = new Connection();

        #region GetCustomerExecon SP_TrackPoint_SelAccountIvanti
        public async Task<List<CustomerExecon>> SelCustomer()
        {
            List<CustomerExecon> lstCarga = new List<CustomerExecon>();
            try
            {
                using (var sql = new SqlConnection(cn.SqlComm()))
                {
                    using (var cmd = new SqlCommand("SP_TrackPoint_SelAccountInsert", sql))
                    {
                        await sql.OpenAsync();
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (var item = await cmd.ExecuteReaderAsync())
                        {
                            while (await item.ReadAsync())
                            {
                                lstCarga.Add(new CustomerExecon()
                                {
                                    recId             = Convert.ToString(item["recId"]),
                                    nameCustomer      = Convert.ToString(item["name"]),
                                    mailAdminTP       = Convert.ToString(item["mail"]),
                                    uuidAdminTP       = Convert.ToString(item["user_uuid_id"]),
                                    contactCustomer   = Convert.ToString(item["contact"]),
                                    addressCustomer   = Convert.ToString(item["address"]),
                                    telephoneCustomer = Convert.ToString(item["telephone"]),
                                    client_IdCustomer = Convert.ToString(item["client_id"]),
                                    email             = Convert.ToString(item["email"]),
                                    init              = Convert.ToBoolean(item["init"]),
                                    close             = Convert.ToBoolean(item["close"]),
                                    scheduled         = Convert.ToBoolean(item["scheduled"]),
                                    daybefore         = Convert.ToBoolean(item["daybefore"]),
                                    send_ics          = Convert.ToBoolean(item["send_ics"])
                                });
                            }
                        }
                        await sql.CloseAsync();
                    }
                }
                    return lstCarga; 
            } catch (Exception ex)
            {
                return lstCarga;
            }
        }
        #endregion
    }
}
