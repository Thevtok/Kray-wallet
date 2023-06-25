using Go_Saku.Net.Models;
using Go_Saku.Net.Repositories;
using System.Collections.Generic;

namespace Go_Saku.Net.Usecase
{
    public interface IUserUsecase
    {
        IEnumerable<User> FindAllUsers();
        User FindByUsername(string username);
        User FindById(Guid id);
        Task SaveDeviceToken(Guid userId, string token);
        Task<User> GetByEmailAndPassword(string email, string password, string token);
        void UpdateUserBalance(Guid userID, int newBalance);
        User FindyPhonBe(string phone);
         Task<string> CreateUser(User user);
    }

    public class UserUsecase : IUserUsecase
    {
        private readonly IUserRepository _userRepository;

        public UserUsecase(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IEnumerable<User> FindAllUsers()
        {
            return _userRepository.GetAll();
        }
        public User FindByUsername(string username)
        {
            return _userRepository.GetByUsername
                (username);
        }
        public User FindById(Guid id)
        {
            return _userRepository.GetById(id);
        }

        public async Task SaveDeviceToken(Guid userId, string token)
        {
            await _userRepository.SaveDeviceToken(userId, token);
        }

        public async Task<User> GetByEmailAndPassword(string email, string password, string token)
        {
            return await _userRepository.GetByEmailAndPassword(email, password, token);
        }
        public void UpdateUserBalance(Guid userID, int newBalance)
        {
            try
            {
                _userRepository.UpdateBalance(userID, newBalance);
                Console.WriteLine("User balance updated successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating user balance: {ex.Message}");
            }
        }
        public User FindyPhonBe(string phone)
        {
            return _userRepository.GetByPhone(phone);
        }

        public async Task<string> CreateUser(User newUser)
        {
            if (string.IsNullOrEmpty(newUser.Name) ||
                string.IsNullOrEmpty(newUser.Username) ||
                string.IsNullOrEmpty(newUser.Email) ||
                string.IsNullOrEmpty(newUser.Password) ||
                string.IsNullOrEmpty(newUser.PhoneNumber) ||
                string.IsNullOrEmpty(newUser.Address))
            {
                throw new ArgumentException("Invalid Input: Required fields are empty");
            }

            if (!newUser.Email.EndsWith("@gmail.com"))
            {
                throw new ArgumentException("Email must be a gmail address");
            }

            if (newUser.Password.Length < 8)
            {
                throw new ArgumentException("Invalid Input: Password must have at least 8 characters");
            }

            if (!IsValidPassword(newUser.Password))
            {
                throw new ArgumentException("Password must contain at least one uppercase letter and one number");
            }

            if (newUser.PhoneNumber.Length < 11 || newUser.PhoneNumber.Length > 13)
            {
                throw new ArgumentException("Phone number must be 11 - 13 digits");
            }

            // Lakukan operasi lainnya untuk membuat pengguna baru
            try
            {
                // Lakukan operasi penyimpanan data pengguna ke dalam database melalui _userRepository
                await _userRepository.Create(newUser);

                return "User created successfully";
            }
            catch (Exception ex)
            {
                // Tangani kesalahan jika penyimpanan data gagal
                throw new Exception("Failed to create user: " + ex.Message);
            }
        }
        public bool IsValidPassword(string password)
        {
            bool hasNumber = false;
            bool hasUpper = false;

            foreach (char c in password)
            {
                if (char.IsDigit(c))
                {
                    hasNumber = true;
                }
                else if (char.IsUpper(c))
                {
                    hasUpper = true;
                }
            }

            return hasNumber && hasUpper;
        }




    }
}
