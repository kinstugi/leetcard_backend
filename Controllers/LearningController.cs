using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharpCardAPI.Repository;
using SharpCardAPI.Utility;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class LearningController: ControllerBase{
    private readonly UserRepository _userRepository;
    
    public LearningController(UserRepository userRepository){
        _userRepository = userRepository;
    }

    [HttpGet("cards")]
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

    [HttpPost("{questionId}/solve")]
    public async Task<IActionResult> SolveQuestion(){
        await Task.Delay(1);
        return Ok();
    }
}