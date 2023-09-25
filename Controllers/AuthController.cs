using Microsoft.AspNetCore.Mvc;
using SharpCardAPI.Models;
using SharpCardAPI.Repository;

namespace SharpCardAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class Auth: ControllerBase{
    private readonly IAuthRepository _authRepo;
    private readonly IUserRepository _userRepo;
    public Auth(IAuthRepository authRepository, IUserRepository userRepo){
        _authRepo = authRepository;
        _userRepo = userRepo;
    }

    [HttpPost("register")]
    public async Task<IActionResult> CreateNewUser(UserDTO userDTO){
        try{
            var nUser = await _authRepo.CreateUserAccount(userDTO);
            await _userRepo.CreateUserQuestionCards(nUser);
            return Ok("user created");
        }catch(Exception ex){
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> LogInUser(UserDTO userDTO){
        try{
            var token = await _authRepo.AuthenticateUser(userDTO);
            return Ok(token);
        }catch(Exception ex){
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("reset")]
    public async Task<IActionResult> ResetAccount(string email){
        await Task.Delay(1);
        return Ok("email sent");
    }
}