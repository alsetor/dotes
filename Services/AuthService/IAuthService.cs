using AuthService.BE;
using System.Threading.Tasks;

namespace AuthService
{
    public interface IAuthService
    {
        /// <summary>
        /// Авторизоваться в ААС КНД
        /// </summary>
        /// <param name="login">имя пользователя</param>
        /// <param name="password">пароль</param>
        /// <returns></returns>
        Task<KNDAuthInfo> Auth(string login, string password);

        /// <summary>
        /// Получить информация о пользователе
        /// </summary>
        /// <param name="token">токен</param>
        /// <param name="tokenType">тип авторизации</param>
        /// <returns></returns>
        Task<KndUserInfo> GetUserInfo(string token, string tokenType);        
    }
}
