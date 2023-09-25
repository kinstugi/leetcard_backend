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
    private readonly IUserRepository _userRepository;
    public InterviewController(IUserRepository userRepository){
        _userRepository = userRepository;
    }

    [HttpGet("cards")]
    public async Task<IActionResult> GetRandomCards([FromQuery]int pack=1, [FromQuery]int number=1){
        var userIdStr = HttpContext.User.FindFirst(ClaimTypes.PrimarySid)!.Value;
        if (pack != 1 && pack != 2) pack = 1;
        if (!(0 <= number && number <= 50)) number = 1;
        var cards = await _userRepository.GetUserQuestionCards(int.Parse(userIdStr), MiscMethods.Packs[pack-1]);
        var questionSet = MiscMethods.SelectRandomDistinct<QuestionCard>(cards, number);
        return Ok(questionSet);
    }

    [HttpPost("{questionId}/solve")]
    public async Task<IActionResult> UpdateStat(int questionId, QuestionResponseDTO responseDTO){
        var userIdStr = HttpContext.User.FindFirst(ClaimTypes.PrimarySid)!.Value;
        var res = await _userRepository.UpdateQuestionCardStat(int.Parse(userIdStr), questionId, responseDTO.DidAnswerCorrectly);
        return Ok(res);
    }
}
