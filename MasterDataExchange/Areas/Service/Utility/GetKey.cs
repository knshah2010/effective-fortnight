
using Framework.DataAccess.Utility;
using Models;

namespace DataExchange.Areas.Service.Utility
{
    public class GetKey
    {
       public string ReturnValue(string ModelName, dynamic value=null)
        {
            switch (ModelName)
            {
                case "rate_applicability": return GeneratePK.getPK<PurchaseRateApplicability>();
                case "customer_code": return GeneratePK.getPK<CustomerMaster>();
                default: return null;
            }
        }
    }
}
