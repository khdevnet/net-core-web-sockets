using NightChat.WebApi.Common.Data.Models;
using NightChat.WebApi.Common.Data.Repositories;
using NightChat.WebApi.Facebook.Models;

namespace NightChat.WebApi.Common.Services
{
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository usersRepository;

        public UsersService(
            IUsersRepository usersRepository)
        {
            this.usersRepository = usersRepository;
        }

        public void AddOrUpdateUser(UserInfoModel userModel)
        {
            User user = usersRepository.GetUserById(userModel.Id);

            if (user == null)
            {
                usersRepository.Add(Convert(userModel));
            }
            else
            {
                usersRepository.Update(Convert(userModel));
            }
        }

        private static User Convert(UserInfoModel userModel)
        {
            return new User { Id = userModel.Id, Name = userModel.Name, Url = userModel.Picture.Data.Url };
        }
    }
}