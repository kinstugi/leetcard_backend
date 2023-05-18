using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharpCardAPI.Repository;
using SharpCardAPI.Utility;
using SharpCardAPI.Models;

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
        var cards = await _userRepository.GetLearningCards(int.Parse(userIdStr), numberOfQuestions, MiscMethods.Packs[pId-1]);
        return Ok(cards);
    }

    [HttpPost("{questionId}/solve")]
    public async Task<IActionResult> SolveQuestion(int questionId, QuestionResponseDTO responseDTO){
        var userIdStr = HttpContext.User.FindFirst(ClaimTypes.PrimarySid)!.Value;
        var res = await _userRepository.UpdateNextReviewDate(int.Parse(userIdStr), questionId, responseDTO.AnswerQuality);
        return Ok(res);
    }

    [HttpGet("cards/review")]
    public async Task<IActionResult> GetReviewCards(){
        var userIdStr = HttpContext.User.FindFirst(ClaimTypes.PrimarySid)!.Value;
        var cards = await _userRepository.GetCardsToReview(int.Parse(userIdStr));
        return Ok(cards);
    }
}