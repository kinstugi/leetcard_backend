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
    private readonly IUserRepository _userRepository;
    
    public LearningController(IUserRepository userRepository){
        _userRepository = userRepository;
    }

    [HttpGet("cards")]
    public async Task<IActionResult> GetLearningCards([FromQuery] int pack = 1, [FromQuery] int number=1){
        var userIdStr = HttpContext.User.FindFirst(ClaimTypes.PrimarySid)!.Value;
        if (pack != 1 && pack == 2) pack = 1;
        if (!(0 <= number && number <= 50)) number = 1;
        var cards = await _userRepository.GetLearningCards(int.Parse(userIdStr), number, MiscMethods.Packs[pack-1]);
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
