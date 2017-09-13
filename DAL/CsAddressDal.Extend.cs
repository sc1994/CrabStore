using Model.DBModel;
using System.Text;
namespace DAL
{
    /// <summary>
    /// 收货地址表  数据访问扩展层(此类中的代码不会被覆盖)
    /// </summary>
    public partial class CsAddressDal
    {
        /// <summary>
        /// 修改用户默认收货地址
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public int ChooseAddress(CsAddress address)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append($"update CsAddress set IsDefault=2 where UserId={address.UserId} and IsDefault=1;");
            strSql.Append($"update CsAddress set IsDefault=1 where AddressId={address.AddressId}");
            int number = DbClient.Excute(strSql.ToString());
            return number;
            
        }
    }
}
