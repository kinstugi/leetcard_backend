using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharpCardAPI.Models;
using SharpCardAPI.Repository;
using SharpCardAPI.Utility;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class InterviewController: ControllerBase{
    private readonly UserRepository _userRepository;
    public InterviewController(UserRepository userRepository){
        _userRepository = userRepository;
    }

    [HttpGet("cards")]
    public async Task<IActionResult> GetRandomCards(){
        var userIdStr = HttpContext.User.FindFirst(ClaimTypes.PrimarySid)!.Value;
        string numStr = HttpContext.Request.Query["number"].ToString();
        string pack = HttpContext.Request.Query["pack"].ToString();
        int numberOfQuestions = MiscMethods.StringToInt(numStr, 5, 20);
        int pId = MiscMethods.StringToInt(pack, 1, 2);
        var cards = await _userRepository.GetUserQuestionCards(int.Parse(userIdStr), MiscMethods.Packs[pId-1]);
        var questionSet = MiscMethods.SelectRandomDistinct<QuestionCard>(cards, numberOfQuestions);
        return Ok(questionSet);
    }

    [HttpPost("{questionId}/solve")]
    public async Task<IActionResult> UpdateStat(int questionId, QuestionResponseDTO responseDTO){
        var userIdStr = HttpContext.User.FindFirst(ClaimTypes.PrimarySid)!.Value;
        await _userRepository.UpdateQuestionCardStat(int.Parse(userIdStr), questionId, responseDTO.DidAnswerCorrectly);
        return Ok();
    }
}