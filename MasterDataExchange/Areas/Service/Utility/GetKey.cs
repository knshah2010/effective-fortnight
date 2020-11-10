
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
                default: return null;
            }
        }
    }
}
