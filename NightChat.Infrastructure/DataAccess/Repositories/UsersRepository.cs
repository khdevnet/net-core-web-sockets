﻿using System.Linq;
using AutoMapper;
using NightChat.Domain.Entities;
using NightChat.Domain.Extensibility.Dto;
using NightChat.Domain.Extensibility.Repositories;
using NightChat.Infrastructure.DataAccess.DataContext;

namespace NightChat.Infrastructure.DataAccess.Repositories
{
    internal class UsersRepository : IUsersRepository
    {
        private readonly ISessionDataContext sessionDataContext;

        public UsersRepository(ISessionDataContext sessionDataContext)
        {
            this.sessionDataContext = sessionDataContext;
        }

        public void Add(UserData user)
        {
            if (GetUserById(user.Id) == null)
            {
                sessionDataContext.Users.Add(Mapper.Map<User>(user));
                sessionDataContext.SaveChanges();
            }
        }

        public void Update(UserData user)
        {
            User userEntity = GetUserEntityById(user.Id);
            userEntity.Name = user.Name;
            userEntity.Url = user.Avatar;
            sessionDataContext.SaveChanges();
        }

        public UserData GetUserById(string id)
        {
            User user = GetUserEntityById(id);
            return Mapper.Map<UserData>(user);
        }

        private User GetUserEntityById(string id)
        {
            return sessionDataContext.Users.SingleOrDefault(u => u.Id == id);
        }
    }
}