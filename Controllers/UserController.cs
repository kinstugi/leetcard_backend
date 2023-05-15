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
    private readonly QuestionRepository _questionRepo;
    public UserController(UserRepository userRepository, QuestionRepository questionRepository){
        _userRepository = userRepository;
        _questionRepo = questionRepository;
    }

    [HttpGet("cards/random")]
    public async Task<IActionResult> GetRandomCards(){
        var userIdStr = HttpContext.User.FindFirst(ClaimTypes.PrimarySid)!.Value;
        string numStr = HttpContext.Request.Query["number"].ToString();
        int numberOfQuestions = MiscMethods.StringToInt(numStr, 5, 20);
        var cards = await _userRepository.GetUserQuestionCards(int.Parse(userIdStr));
        var questionSet = MiscMethods.SelectRandomDistinct<QuestionCard>(cards, numberOfQuestions);
        return Ok(questionSet);
    }
    
    [HttpGet("cards/learning")]
    public async Task<IActionResult> GetLearningCards(){
        var userIdStr = HttpContext.User.FindFirst(ClaimTypes.PrimarySid)!.Value;
        string numStr = HttpContext.Request.Query["number"].ToString();
        string pack = HttpContext.Request.Query["pack"].ToString();
        int pId = MiscMethods.StringToInt(pack, 1, 2);
        int numberOfQuestions = MiscMethods.StringToInt(numStr, 5, 20);
        var resDict = new Dictionary<string, object>();
        var reviewCards = await _userRepository.GetCardsToReview(int.Parse(userIdStr));
        resDict["hasReview"] = reviewCards.Count > 0;
        if (reviewCards.Count <= 0){
            var cards = await _userRepository.GetLearningCards(int.Parse(userIdStr), numberOfQuestions, MiscMethods.Packs[pId-1]);
            resDict["cards"] = cards;
        }else{
            resDict["cards"] = reviewCards;
        }
        return Ok(resDict);
    }

    [HttpPost("cards/response")]
    public async Task<IActionResult> UpdateCardsBunch(List<QuestionCard> cards){
        // this is to update after every learning session
        await Task.Delay(1);
        return Ok();
    }

    [HttpPost("cards/response/{cardId}")]
    public async Task<IActionResult> UpdateSingleCard(int cardId, bool answer){
        var userIdStr = HttpContext.User.FindFirst(ClaimTypes.PrimarySid)!.Value;
        await _questionRepo.UpdateQuestionCard(int.Parse(userIdStr), cardId, answer);
        return Ok("updated");
    }
}