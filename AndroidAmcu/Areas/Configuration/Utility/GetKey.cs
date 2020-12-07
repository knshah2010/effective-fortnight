
using AndroidAmcu.Areas.Configuration.Models;
using Framework.DataAccess.Utility;
using Models;

namespace AndroidAmcu.Areas.Configuration.Utility
{
    public class GetKey
    {
       public string ReturnValue(string ModelName, dynamic value=null)
        {
            switch (ModelName)
            {
                case "installation_detail": return GeneratePK.getPK<AndroidInstallationDetails>();               
                default: return null;
            }
        }
    }
}
