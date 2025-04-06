using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Factories
{
    // The FACTORY PATTERN is a creative design pattern used to create objects
    // without specifying the exact class of the object to be created.
    public class UserFactory
    {
        public static User CreateUser(string name, string password)
        {
            return new User
            {
                Name = name,
                Password = password
            };
        }
    }
}
