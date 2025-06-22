using System.Globalization;
using Database.Entities;
using Endpoints.Interfaces;
using Endpoints.Interfaces.Authorization;
using Timetable.Interfaces;

namespace Services;

public class LessonSuggestionService : ILessonSuggestionService
{
    public LessonSuggestionService(ITransactor transactor)
    {
        _transactor = transactor;
    }

    public string AddLessonSuggestion(LessonSuggestionDto lessonSuggestionToAdd, TutorRole role)
    {
        if(lessonSuggestionToAdd.SuggestedStart == null
           || lessonSuggestionToAdd.SuggestedEnd == null
           || lessonSuggestionToAdd.Student?.ExternalId == null
           || lessonSuggestionToAdd.Address?.ExternalId == null)
            throw new BadRequestException("Required information is missing");

        DbTimeslot newTimeslot = new DbTimeslot()
        {
            StartTime = (DateTimeOffset)lessonSuggestionToAdd.SuggestedStart,
            EndTime = (DateTimeOffset)lessonSuggestionToAdd.SuggestedEnd
        };

        using var t = _transactor.BeginTransaction();

        var studentConnected = t.StudentDao.GetStudent(int.Parse(lessonSuggestionToAdd.Student.ExternalId));
        var addressConnected = t.AddressDao.GetAddress(int.Parse(lessonSuggestionToAdd.Address.ExternalId));

        if (studentConnected == null || addressConnected == null)
            throw new BadRequestException("Requested address or student does not exist!");

        List<DbTimeslot> takenTimeslots = t.LessonDao.GetTimeslots();

        if (t.LessonDao.IsTermTaken(new List<DbTimeslot>() { newTimeslot }, takenTimeslots))
            throw new BadRequestException("This term is taken!");


        DbLessonSuggestion lSuggestion = new DbLessonSuggestion()
        {
            Timeslot = newTimeslot,
            Address = addressConnected,
            Student = studentConnected,
        };

        if (int.TryParse(lessonSuggestionToAdd.Lesson?.LessonId, out var lessonId))
        {
            lSuggestion.Lesson = t.LessonDao.GetLessonById(lessonId);
            IsLessonAcceptable(lSuggestion.Lesson, studentConnected);
        }

        t.LessonSuggestionDao.SaveLessonSuggestion(lSuggestion);
        t.Commit();
        return "Lesson suggestion added!";
    }

    public void DeleteLessonSuggestion(string externalId, TutorRole role)
    {
        using var t = _transactor.BeginTransaction();
        t.LessonSuggestionDao.DeleteLessonSuggestion(int.Parse(externalId));
        t.Commit();
    }

    public void UpdateLessonSuggestion(
        string externalId, LessonSuggestionDto updatedLessonSuggestion, TutorRole role)
    {
        int id = int.Parse(externalId);
        DbLesson? connectedLesson = null;
        DbAddress? connectedAddress = null;
        using var t = _transactor.BeginTransaction();
        
        var lessSuggToUpdate = t.LessonSuggestionDao.GetLessSuggById(id);
        if(lessSuggToUpdate == null)
            throw new BadRequestException("The requested lesson suggestion does not exist!");
        
        if (int.TryParse(updatedLessonSuggestion.Lesson?.LessonId, out var lessonId))
        {
            connectedLesson = t.LessonDao.GetLessonById(lessonId);
            IsLessonAcceptable(connectedLesson, lessSuggToUpdate.Student!);
        }

        if(int.TryParse(updatedLessonSuggestion.Address?.ExternalId, out var addressId))
            connectedAddress = t.AddressDao.GetAddress(addressId);

        
        lessSuggToUpdate.Timeslot.StartTime = updatedLessonSuggestion.SuggestedStart
                                              ?? lessSuggToUpdate.Timeslot.StartTime;
        lessSuggToUpdate.Timeslot.EndTime = updatedLessonSuggestion.SuggestedEnd
                                            ?? lessSuggToUpdate.Timeslot.EndTime;

        lessSuggToUpdate.Lesson = connectedLesson ?? lessSuggToUpdate.Lesson;
        lessSuggToUpdate.Address = connectedAddress ?? lessSuggToUpdate.Address;

        var timeslotId = lessSuggToUpdate.TimeslotId;
        List<DbTimeslot> takenTimeslots = t.LessonDao.GetTimeslots().Where(ts => ts.Id !=timeslotId).ToList();

        if (t.LessonDao.IsTermTaken(new List<DbTimeslot>() { lessSuggToUpdate.Timeslot }, takenTimeslots))
            throw new BadRequestException("This term is taken!");

        t.LessonSuggestionDao.SaveLessonSuggestion(lessSuggToUpdate);
        t.Commit();
    }

    private static void IsLessonAcceptable(DbLesson? lesson, DbStudent student)
    {
        if(lesson == null)
            throw new BadRequestException("The requested lesson does not exist!");
        if(lesson.Attendances.Count>1)
            throw new BadRequestException("Cannot create lesson suggestion for multiple students lesson!");
        if(lesson.Attendances.SingleOrDefault()!.Student!.Id != student.Id)
            throw new BadRequestException("The chosen student do not attend to chosen lesson!");
    }

    public List<LessonSuggestionDto> GetLessonSuggestion(
        string? suggestedStart, string? suggestedEnd, string? studentExternalId, TutorRole role)
    {
        var startOffset = TryParseDateTime(suggestedStart) ?? DateTimeOffset.MinValue;
        var endOffset = TryParseDateTime(suggestedEnd) ?? DateTimeOffset.MaxValue;
        int? studentId = studentExternalId is null ? null : int.Parse(studentExternalId);
        using var t = _transactor.BeginTransaction();
        var lessonSuggestions = t.LessonSuggestionDao.GetLessSugg(startOffset, endOffset, studentId);
        List<LessonSuggestionDto> lessonSuggestionsDto = new List<LessonSuggestionDto>();
        foreach (var ls in lessonSuggestions)
        {
            LessonSuggestionDto lessSuggToGet = new LessonSuggestionDto()
            {
                ExternalId = ls.Id.ToString(),
                SuggestedStart = ls.Timeslot?.StartTime,
                SuggestedEnd = ls.Timeslot?.EndTime,
                Address = new AddressDto()
                {
                    ExternalId = ls.Address?.Id.ToString(),
                    AddressData = ls.Address?.AddressData,
                    AddressName = ls.Address?.AddressName
                },
                Lesson = ls.Lesson is null ? null : new LessonDto()
                {
                    LessonId = ls.Lesson?.Id.ToString() ?? "BRAK",
                    Address = ls.Lesson?.Schedule?.Address?.AddressData ?? "BRAK",
                    Attendances = ls.Lesson?.Attendances.Select(a => new AttendanceDto()
                    {
                        Confirmed = a.IsConfirmed,
                        StudentName = a.Student?.Name ?? "BRAK",
                        StudentSurname = a.Student?.Surname ?? "BRAK"
                    }).ToList() ?? new List<AttendanceDto>(),
                    Description = ls.Lesson?.TutorInfo ?? "BRAK",
                    StartTime = ls.Lesson!.Timeslot.StartTime,
                    EndTime = ls.Lesson!.Timeslot.EndTime
                },
                Student = new StudentDto()
                {
                    ExternalId = ls.Student?.Id.ToString(),
                    Name = ls.Student?.Name,
                    Surname = ls.Student?.Surname,
                    PhoneNumber = ls.Student?.PhoneNumber,
                    IsDeleted = ls.Student?.IsDeleted,
                    Address = new AddressDto()
                    {
                        ExternalId = ls.Student?.Address?.Id.ToString(),
                        AddressData = ls.Student?.Address?.AddressData,
                        AddressName = ls.Student?.Address?.AddressName
                    }
                }
            };
            lessonSuggestionsDto.Add(lessSuggToGet);
        }

        return lessonSuggestionsDto;
    }

    public List<LessonSuggestionDto> GetLessSuggsAsStudent(
        string? suggestedStart, string? suggestedEnd, StudentRole role)
    {
        var startOffset = TryParseDateTime(suggestedStart) ?? DateTimeOffset.MinValue;
        var endOffset = TryParseDateTime(suggestedEnd) ?? DateTimeOffset.MaxValue;
        var studentId = int.Parse(role.ExternalStudentId);
        using var t = _transactor.BeginTransaction();
        var lessonSuggestions = t.LessonSuggestionDao.GetLessSugg(startOffset, endOffset, studentId);
        List<LessonSuggestionDto> lessonSuggestionsDto = new List<LessonSuggestionDto>();
        foreach (var ls in lessonSuggestions)
        {
            LessonSuggestionDto lessSuggToGet = new LessonSuggestionDto()
            {
                ExternalId = ls.Id.ToString(),
                SuggestedStart = ls.Timeslot?.StartTime,
                SuggestedEnd = ls.Timeslot?.EndTime,
                Address = new AddressDto()
                {
                    ExternalId = ls.Address?.Id.ToString(),
                    AddressData = ls.Address?.AddressData,
                },
                Lesson = ls.Lesson is null ? null : new LessonDto()
                {
                    LessonId = ls.Lesson?.Id.ToString() ?? "BRAK",
                    Address = ls.Lesson?.Schedule?.Address?.AddressData ?? "BRAK",
                    Attendances = ls.Lesson?.Attendances.Select(a => new AttendanceDto()
                    {
                        Confirmed = a.IsConfirmed,
                        StudentName = a.Student?.Name ?? "BRAK",
                        StudentSurname = a.Student?.Surname ?? "BRAK"
                    }).ToList() ?? new List<AttendanceDto>(),
                    Description = "",
                    StartTime = ls.Lesson!.Timeslot.StartTime,
                    EndTime = ls.Lesson!.Timeslot.EndTime
                }
            };
            lessonSuggestionsDto.Add(lessSuggToGet);
        }

        return lessonSuggestionsDto;
    }

    private static DateTimeOffset? TryParseDateTime(string? s)
    {
        if (DateTimeOffset.TryParse(
                s,
                CultureInfo.InvariantCulture,
                DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal,
                out var time))
        {
            return time;
        }
        else
        {
            return null;
        }
    }


    private readonly ITransactor _transactor;
}
