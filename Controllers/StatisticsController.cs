using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharpCardAPI.Repository;
using SharpCardAPI.Utility;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class StatisticsController: ControllerBase{
    private readonly IUserRepository _userRepo;
    public StatisticsController(IUserRepository userRepo){
        _userRepo = userRepo;
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetUserOverallStat([FromQuery]int count=1){
        var userIdStr = HttpContext.User.FindFirst(ClaimTypes.PrimarySid)!.Value;
        var countStr = HttpContext.Request.Query["count"].ToString();
        var statCount = MiscMethods.StringToInt(countStr, 5, 10);
        var userId =  int.Parse(userIdStr);
        var res = new Dictionary<string, object>();
        res["overall"] = await _userRepo.GetUserStats(userId);
        res["topQuestions"] = await _userRepo.GetTopQuestions(userId, statCount);
        res["bottomQuestions"] = await _userRepo.GetBottomQuestions(userId, statCount);
        return Ok(res);
    }

    [HttpGet("me/reset")]
    public async Task<IActionResult> ResetUserStats(){
        var userIdStr = HttpContext.User.FindFirst(ClaimTypes.PrimarySid)!.Value;
        await _userRepo.ResetData(int.Parse(userIdStr));
        return Ok("statistics have been reset");
    }
}
