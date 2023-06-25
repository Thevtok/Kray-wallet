using Go_Saku.Net.Data;
using Go_Saku.Net.Models;
using Go_Saku.Net.Utils;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Go_Saku.Net.Repositories
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAll();
        User GetByUsername(string username);
        User GetById(Guid id);
        Task SaveDeviceToken(Guid userId, string token);
        Task<User> GetByEmailAndPassword(string email, string password, string token);
        void UpdateBalance(Guid userID, int newBalance);
        void UpdatePoint(Guid userID, int newPoint);
        User? GetByPhone(string phone);
         Task<object> Create(User user);
    }

    public class UserRepository : IUserRepository
    {
        private readonly UserApiDbContext _dbContext;

        public UserRepository(UserApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<User> GetAll()
        {
            return _dbContext.Users.Select(user => new User
            {
                ID = user.ID,
                Name = user.Name,
                Username = user.Username,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                Balance = user.Balance,
                Role = user.Role,
                Point = user.Point,
                Token = user.Token,
                BadgeID = user.BadgeID,
                TxCount = user.TxCount
            }).ToList();
        }


        public User? GetByUsername(string username)
        {
            return _dbContext.Users
                .Include(u => u.Badge) // Join dengan tabel mst_badges
                .Where(u => u.Username == username)
                .Select(u => new User
                {
                    ID = u.ID,
                    Name = u.Name,
                    Username = u.Username,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    Address = u.Address,
                    Balance = u.Balance,
                    Point = u.Point,
                    Token= u.Token,
                    BadgeID = u.BadgeID,
                    TxCount = u.TxCount,
                    BadgeName = u.Badge.BadgeName // Menambahkan properti BadgeName
                })
                .FirstOrDefault();
        }
        public User?  GetById (Guid id)
        {
            return _dbContext.Users
               .Include(u => u.Badge) // Join dengan tabel mst_badges
               .Where(u => u.ID == id)
               .Select(u => new User
               {
                   ID = u.ID,
                   Name = u.Name,
                   Username = u.Username,
                   Email = u.Email,
                   PhoneNumber = u.PhoneNumber,
                   Address = u.Address,
                   Balance = u.Balance,
                   Point = u.Point,
                   Token = u.Token,
                   BadgeID = u.BadgeID,
                   TxCount = u.TxCount,
                   BadgeName = u.Badge.BadgeName // Menambahkan properti BadgeName
               })
               .FirstOrDefault();
        }
        public async Task SaveDeviceToken(Guid userId, string token)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.ID == userId);
            if (user != null)
            {
                user.Token = token;
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new ArgumentException("User not found.");
            }
        }
        public async Task<User> GetByEmailAndPassword(string email, string password, string token)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                throw new ArgumentException("User not found.");
                
            }

            // Verify the password
            if (!PasswordUtils.VerifyPassword(password, user.Password))
            {
                throw new ArgumentException("Invalid credentials.");
            }

            // Update the user's device token
            user.Token = token;
            _dbContext.SaveChanges();

            return user;
        }
        public void UpdateBalance(Guid userID, int newBalance)
        {
            User user = _dbContext.Users.FirstOrDefault(u => u.ID == userID);
            if (user == null)
            {
                throw new ArgumentException("User not found.");
            }

            user.Balance = newBalance;
            _dbContext.SaveChanges();
        }
        public void UpdatePoint(Guid userID, int newPoint)
        {
          

            User user = _dbContext.Users.FirstOrDefault(u => u.ID == userID);
            if (user == null)
            {
                throw new ArgumentException("User not found.");
            }

            user.Point = newPoint;
            _dbContext.SaveChanges();
        }


        public User? GetByPhone(string phone)
        {
            return _dbContext.Users
               .Include(u => u.Badge) // Join dengan tabel mst_badges
               .Where(u => u.PhoneNumber == phone)
               .Select(u => new User
               {
                   ID = u.ID,
                   Name = u.Name,
                   Username = u.Username,
                   Email = u.Email,
                   PhoneNumber = u.PhoneNumber,
                   Address = u.Address,
                   Balance = u.Balance,
                   Point = u.Point,
                   Token = u.Token,
                   BadgeID = u.BadgeID,
                   TxCount = u.TxCount,
                   BadgeName = u.Badge.BadgeName // Menambahkan properti BadgeName
               })
               .FirstOrDefault();
        }
        public async Task<object> Create(User user)
        {
            // Hash the password
            string hashedPassword = PasswordUtils.HashPassword(user.Password);

            // Generate a new UUID
            Guid newUUID = Guid.NewGuid();

            // Create the user record in the database
            var newUser = new User
            {
                ID = newUUID,
                Name = user.Name,
                Username = user.Username,
                Email = user.Email,
                Password = hashedPassword,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                Balance = 0,
                Role = "user",
                Point = 0,
                Token = "",
                BadgeID = 1,
                TxCount = 0
            };

            _dbContext.Users.Add(newUser);
             _dbContext.SaveChanges();

            return "User created successfully";
        }




    }
}
