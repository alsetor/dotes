using System.Security.Claims;
using System.Threading.Tasks;

namespace Templates.Web.Auth
{
    public interface IJwtFactory
    {
        Task<string> GenerateEncodedToken(string userName, ClaimsIdentity identity);
        ClaimsIdentity GenerateClaimsIdentityWithCamundaAccess(string userName);
    }
}