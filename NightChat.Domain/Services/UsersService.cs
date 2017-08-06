using NightChat.Domain.Dto;
using NightChat.Domain.Entities;
using NightChat.Domain.Repositories;

namespace NightChat.Domain.Services
{
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository usersRepository;

        public UsersService(
            IUsersRepository usersRepository)
        {
            this.usersRepository = usersRepository;
        }

        public void AddOrUpdateUser(UserData user)
        {
            User userEntity = usersRepository.GetUserById(user.Id);

            if (userEntity == null)
            {
                usersRepository.Add(Convert(user));
            }
            else
            {
                usersRepository.Update(Convert(user));
            }
        }

        private static User Convert(UserData user)
        {
            return new User { Id = user.Id, Name = user.Name, Url = user.Avatar };
        }
    }
}