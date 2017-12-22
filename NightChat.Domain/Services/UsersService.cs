using NightChat.Domain.Extensibility.Dto;
using NightChat.Domain.Extensibility.Repositories;
using NightChat.Domain.Extensibility.Services;

namespace NightChat.Domain.Services
{
    internal class UsersService : IUsersService
    {
        private readonly IUsersRepository usersRepository;

        public UsersService(
            IUsersRepository usersRepository)
        {
            this.usersRepository = usersRepository;
        }

        public void AddOrUpdateUser(UserData user)
        {
            UserData userEntity = usersRepository.GetUserById(user.Id);

            if (userEntity == null)
            {
                usersRepository.Add(user);
            }
            else
            {
                usersRepository.Update(user);
            }
        }
    }
}