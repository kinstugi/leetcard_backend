using SharpCardAPI.Data;
using SharpCardAPI.Models;
using SharpCardAPI.Utility;

namespace SharpCardAPI.Repository;

public class AuthRepository{
    private readonly AppDbContext _dbContext;
    private readonly IConfiguration _configuration;

    public AuthRepository(AppDbContext dbContext, IConfiguration configuration){
        _dbContext = dbContext;
        _configuration = configuration;
    }

    public async Task<User> CreateUserAccount(UserDTO userDTO, bool isPersonel = false){
        var dbUser = await Task.Run(()=> _dbContext.Users.Where(u => u.Email == userDTO.Email).FirstOrDefault());
        if (dbUser != null)
            throw new Exception("An account with email already exist");
        byte[] passwordHash;
        byte[] passwordSalt;

        AuthMethods.CreatePasswordHash(userDTO.Password, out passwordSalt, out passwordHash);
        var newUser = new User{
            Email = userDTO.Email,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
            IsPersonnel = isPersonel
        };
        await _dbContext.Users.AddAsync(newUser);
        await _dbContext.SaveChangesAsync();
        return newUser;
    }

    public async Task<string> AuthenticateUser(UserDTO userDTO){
        var dbUser = await Task.Run(()=> _dbContext.Users.Where(u => u.Email == userDTO.Email).FirstOrDefault());
        if (dbUser == null)
            throw new Exception("Account not found");
        bool isPasswordCorrect = AuthMethods.VerifyPasswordHash(userDTO.Password, dbUser.PasswordSalt, dbUser.PasswordHash);
        if (!isPasswordCorrect)
            throw new Exception("Auth creds incorrect");
        try{
            var token = AuthMethods.CreateAuthToken(dbUser, _configuration);
            return token;
        } catch(Exception ex){
            throw ex;
        }
    }
}