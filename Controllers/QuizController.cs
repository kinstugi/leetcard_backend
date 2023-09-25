using Microsoft.AspNetCore.Mvc;
using SharpCardAPI.Repository;
using SharpCardAPI.Utility;

[ApiController]
[Route("api/[controller]")]
public class QuizController: ControllerBase{
    private readonly IQuestionRepository _qRepo;

    public QuizController(IQuestionRepository userRepository){
        _qRepo = userRepository;
    }

    [HttpGet("cards")]
    public async Task<IActionResult> GetRandomCards([FromQuery] int number=1, [FromQuery]int pack=1){
        if (pack != 1 && pack != 2) pack = 1;
        if (!(0 <= number && number <= 50)) number = 1;
        var cards = await _qRepo.GetRandomQuestion(MiscMethods.Packs[pack-1] ,number);
        return Ok(cards);
    }
}
