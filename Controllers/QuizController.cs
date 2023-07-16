using Microsoft.AspNetCore.Mvc;
using SharpCardAPI.Repository;
using SharpCardAPI.Utility;

[ApiController]
[Route("api/[controller]")]
public class QuizController: ControllerBase{
    private readonly QuestionRepository _qRepo;

    public QuizController(QuestionRepository userRepository){
        _qRepo = userRepository;
    }

    [HttpGet("cards")]
    public async Task<IActionResult> GetRandomCards(){
        string numStr = HttpContext.Request.Query["number"].ToString();
        string pack = HttpContext.Request.Query["pack"].ToString();
        int numberOfQuestions = MiscMethods.StringToInt(numStr, 5, 20);
        int pId = MiscMethods.StringToInt(pack, 1, 2);
        var cards = await _qRepo.GetRandomQuestion(MiscMethods.Packs[pId-1] ,numberOfQuestions);
        return Ok(cards);
    }
}