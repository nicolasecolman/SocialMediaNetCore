using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using System.Threading.Tasks;

namespace SocialMedia.Core.Services
{
    public class SecurityService : ISecurityService
    {
        private readonly IUnitOfWork _unitOfWork;
        public SecurityService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Security> GetSecurityByCredentials(UserLogin user)
        {
            return await _unitOfWork.SecurityRepository
                .GetSecurityByCredentials(user);
        }

        public async Task RegisterUser(Security user)
        {
            await _unitOfWork.SecurityRepository.Add(user);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
