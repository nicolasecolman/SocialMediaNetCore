using SocialMedia.Core.Entities;
using System.Threading.Tasks;

namespace SocialMedia.Core.Interfaces
{
    public interface ISecurityService
    {
        Task<Security> GetSecurityByCredentials(UserLogin user);
        Task RegisterUser(Security user);
    }
}