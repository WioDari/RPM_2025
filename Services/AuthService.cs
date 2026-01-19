using MusicPlus.Data;
using MusicPlus.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Collections.Generic;

namespace MusicPlus.Services
{
    public class AuthService
    {
        private readonly ApplicationDbContext _context;
        private static readonly Dictionary<string, int> _loginAttempts = new();
        private static readonly Dictionary<string, DateTime> _blockedUsers = new();

        public AuthService(ApplicationDbContext context)
        {
            _context = context;
        }

        public AuthResult Authenticate(string login, string password, string captcha, string captchaAnswer)
        {
            if (string.IsNullOrWhiteSpace(captcha) || !captcha.Equals(captchaAnswer, StringComparison.OrdinalIgnoreCase))
            {
                return new AuthResult { Success = false, Message = "Неверная капча"                 };
            }

            if (_blockedUsers.ContainsKey(login) && _blockedUsers[login] > DateTime.Now)
            {
                var remainingTime = _blockedUsers[login] - DateTime.Now;
                return new AuthResult 
                { 
                    Success = false, 
                    Message = $"Пользователь заблокирован. Разблокировка через {remainingTime.Minutes} мин. {remainingTime.Seconds} сек." 
                };
            }

            var user = _context.Users.FirstOrDefault(u => u.UserLogin == login);
            if (user == null)
            {
                IncrementLoginAttempts(login);
                return new AuthResult { Success = false, Message = "Неверный логин или пароль" };
            }

            if (user.IsBlocked && user.Role != "Admin")
            {
                return new AuthResult { Success = false, Message = "Пользователь заблокирован администратором" };
            }

            if (user.UserPassword != password)
            {
                IncrementLoginAttempts(login);
                
                int attempts = _loginAttempts.ContainsKey(login) ? _loginAttempts[login] : 0;
                
                if (attempts == 3)
                {
                    return new AuthResult 
                    { 
                        Success = false, 
                        Message = "Предупреждение: после еще двух неправильных попыток вход будет заблокирован на час",
                        Warning = true
                    };
                }
                
                if (attempts >= 5)
                {
                    if (user.Role != "Admin")
                    {
                        _blockedUsers[login] = DateTime.Now.AddHours(1);
                        user.IsBlocked = true;
                        _context.SaveChanges();
                        return new AuthResult { Success = false, Message = "Пользователь заблокирован на 1 час после 5 неудачных попыток" };
                    }
                    else
                    {
                        return new AuthResult { Success = false, Message = "Неверный логин или пароль" };
                    }
                }

                return new AuthResult { Success = false, Message = "Неверный логин или пароль" };
            }

            ResetLoginAttempts(login);
            user.LastLogin = DateTime.Now;
            _context.SaveChanges();

            return new AuthResult 
            { 
                Success = true, 
                User = user,
                Message = "Успешная авторизация"
            };
        }

        private void IncrementLoginAttempts(string login)
        {
            if (!_loginAttempts.ContainsKey(login))
            {
                _loginAttempts[login] = 0;
            }
            _loginAttempts[login]++;
        }

        private void ResetLoginAttempts(string login)
        {
            _loginAttempts.Remove(login);
            _blockedUsers.Remove(login);
        }

        public void UnblockUser(string login)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserLogin == login);
            if (user != null && user.Role != "Admin")
            {
                user.IsBlocked = false;
                _context.SaveChanges();
            }
            ResetLoginAttempts(login);
        }
    }

    public class AuthResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public User? User { get; set; }
        public bool Warning { get; set; }
    }
}
