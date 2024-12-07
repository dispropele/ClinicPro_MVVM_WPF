using System;
using System.Threading.Tasks;
using BCrypt.Net;
using ClinicPro_MVVM_WPF.Data;
using ClinicPro_MVVM_WPF.Model;
using Microsoft.EntityFrameworkCore;

public class UserRepository
{
    private readonly ClinicDbContext _context;

    public UserRepository(ClinicDbContext context)
    {
        _context = context;
    }

    public async Task AddUserAsync(string login, string plainPassword, string role)
    {
        // Хеширование пароля
        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(plainPassword);

        // Создание нового пользователя
        var user = new UserModel()
        {
            Login = login,
            HashPassword = hashedPassword,
            Role = role
        };

        // Добавление пользователя в базу данных
        _context.User.Add(user);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateUserLoginAsync(UserModel user)
    {
        _context.User.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateUserPasswordAsync(UserModel user, string plainPassword)
    {
        // Хеширование пароля
        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(plainPassword);
        user.HashPassword = hashedPassword;
        _context.User.Update(user);
        await _context.SaveChangesAsync();
    }
    
    public async Task<UserModel?> GetUserByLoginAsync(string login)
    {
        // Поиск пользователя по логину
        return await _context.User.FirstOrDefaultAsync(u => u.Login == login);
    }

    public bool VerifyPassword(string plainPassword, string hashedPassword)
    {
        // Проверка пароля с использованием хеша
        return BCrypt.Net.BCrypt.Verify(plainPassword, hashedPassword);
    }

    public async Task<bool> IsLoginExistsAsync(string login)
    {
        return await _context.User.AnyAsync(u => u.Login == login);
    }
    
}