using HelloWorld.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Database.Contract.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LessonController : ControllerBase
{
    private readonly ILessonDao _lessonDao;

    public LessonController(ILessonDao lessonDao)
    {
        _lessonDao = lessonDao;
    }
    
    [HttpGet]
    public async Task<ActionResult<List<LessonDto>>> GetAllLessonsAsync()
    {
        List<LessonDto?> lessons = new List<LessonDto?>();
        var response = await _lessonDao.GetAllLessonsAsync();
        if (response != null && response.Count() != 0)
            lessons.AddRange(response);
        else
            return NoContent();
        return Ok(lessons);
    }

    [HttpGet("{scheduleId}/lesson/{ordinal}")]
    public async Task<ActionResult<LessonDto?>> GetLessonAsync([FromRoute]int scheduleId, [FromRoute]int ordinal)
    {
        LessonDto? lesson = await _lessonDao.GetLessonAsync(scheduleId, ordinal);
        if(lesson == null)
            return NoContent();
        return Ok(lesson);
    }
    
    [HttpPut("{scheduleId}/lesson/{ordinal}")]
    public async Task<ActionResult<LessonDto?>> UpdateLessonAsync([FromRoute] int scheduleId, 
                                                                    [FromRoute] int ordinal, [FromBody] LessonDto? lesson)
    {
        LessonDto? updatedLesson = await _lessonDao.UpdateLessonAsync(scheduleId, ordinal, lesson);
        if(updatedLesson == null)
            return NoContent();
        return Ok(updatedLesson);
    }
    
    [HttpPost]
    public ActionResult<string> AddScheduleAsync([FromBody] ScheduleDto schedule)
    {
        int? scheduleId;
        try
        {
            scheduleId = _lessonDao.CreateSchedule(schedule);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
        return Ok(scheduleId.ToString());
    }
    
    
}