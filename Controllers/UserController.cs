using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharpCardAPI.Models;
using SharpCardAPI.Repository;
using SharpCardAPI.Utility;
using System.Security.Claims;


namespace SharpCardAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserController: ControllerBase{
    private readonly UserRepository _userRepository;
    public UserController(UserRepository userRepository){
        _userRepository = userRepository;
    }

    [HttpGet("cards")]
    public async Task<IActionResult> GetCards(){
        var userIdStr = HttpContext.User.FindFirst(ClaimTypes.PrimarySid)!.Value;
        var cards = await _userRepository.GetUserQuestionCards(int.Parse(userIdStr));
        return Ok(cards);
    }

    [HttpGet("cards/random")]
    public async Task<IActionResult> GetRandomCards(){
        var userIdStr = HttpContext.User.FindFirst(ClaimTypes.PrimarySid)!.Value;
        string numStr = HttpContext.Request.Query["number"].ToString();
        int numberOfQuestions = 20;
        if (numStr != string.Empty){
            bool res = int.TryParse(numStr, out numberOfQuestions);
            if (!res){
                numberOfQuestions = 20;
            }
        }
        var cards = await _userRepository.GetUserQuestionCards(int.Parse(userIdStr));
        var questionSet = MiscMethods.SelectRandomDistinct<QuestionCard>(cards, numberOfQuestions);
        return Ok(questionSet);
    }

    [HttpPost("cards/update")]
    public async Task<IActionResult> UpdateCardsBunch(List<QuestionCard> cards){
        // this is to update after every learning session
        await Task.Delay(1);
        return Ok();
    }
}