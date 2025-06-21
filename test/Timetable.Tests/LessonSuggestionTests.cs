using Database.Entities;
using Endpoints.Interfaces.Authorization;
using FakeItEasy;
using Services;
using Timetable.Interfaces;

namespace Timetable.Tests;

public class LessonSuggestionTests
{
    public LessonSuggestionTests()
    {
        A.CallTo(() => _transactor.BeginTransaction()).Returns(_transaction).Once();
        A.CallTo(() => _transaction.LessonSuggestionDao).Returns(_suggestionDao);
        A.CallTo(() => _transaction.LessonDao).Returns(_lessonDao);
        _service = new TimetableService(_transactor, TimeZoneInfo.Utc);
    }

    [Fact]
    public void AcceptSuggestedNewLesson()
    {
        A.CallTo(() => _suggestionDao.GetLessSuggById(SuggestionId)).Returns(new DbLessonSuggestion
        {
            TimeslotId = SuggestedTimeslotId,
            StudentId = RecipientId,
            AddressId = SuggestedAddressId,
        });
        _service.AcceptSuggestion(ExternalSuggestionId, true, _role);
        A.CallTo(() => _lessonDao.CreateSchedule(A<DbSchedule>.That.Matches(s =>
            s.AddressId == SuggestedAddressId
            && s.Lessons.Single().TimeslotId == SuggestedTimeslotId
            && s.Lessons.Single().Attendances.Single().StudentId == RecipientId
        ))).MustHaveHappenedOnceExactly();
        A.CallTo(() => _suggestionDao.DeleteSuggestionPreservingTimeslot(SuggestionId))
            .MustHaveHappenedOnceExactly();
        A.CallTo(() => _transaction.Commit()).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public void AcceptLessonReplacement()
    {
        A.CallTo(() => _suggestionDao.GetLessSuggById(SuggestionId)).Returns(new DbLessonSuggestion
        {
            TimeslotId = SuggestedTimeslotId,
            StudentId = RecipientId,
            AddressId = SuggestedAddressId,
            LessonId = OriginalLessonId,
        });
        _service.AcceptSuggestion(ExternalSuggestionId, true, _role);
        A.CallTo(() => _lessonDao.RemoveLessonsCascading(new List<int> { OriginalLessonId }))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public void RejectLessonReplacement()
    {
        A.CallTo(() => _suggestionDao.GetLessSuggById(SuggestionId)).Returns(new DbLessonSuggestion
        {
            TimeslotId = SuggestedTimeslotId,
            StudentId = RecipientId,
            AddressId = SuggestedAddressId,
            LessonId = OriginalLessonId,
        });
        _service.AcceptSuggestion(ExternalSuggestionId, false, _role);
        A.CallTo(() => _suggestionDao.DeleteLessonSuggestion(SuggestionId))
            .MustHaveHappenedOnceExactly();
        A.CallTo(() => _lessonDao.CreateSchedule(A<DbSchedule>._)).MustNotHaveHappened();
    }

    [Fact]
    public void OnlyAuthorizeRecipient()
    {
        A.CallTo(() => _suggestionDao.GetLessSuggById(SuggestionId)).Returns(new DbLessonSuggestion
        {
            TimeslotId = SuggestedTimeslotId,
            StudentId = RecipientId,
            AddressId = SuggestedAddressId,
        });
        var action = () => _service.AcceptSuggestion(ExternalSuggestionId, true, _intruderRole);
        Assert.Throws<AuthException>(action);
    }

    private const string ExternalSuggestionId = "101";
    private const int SuggestionId = 101;
    private const string ExternalRecipientId = "201";
    private const int RecipientId = 201;
    private const string IntruderId = "202";
    private const int OriginalLessonId = 301;
    private const int SuggestedAddressId = 401;
    private const int SuggestedTimeslotId = 501;

    private readonly StudentRole _role = new() { ExternalStudentId = ExternalRecipientId };
    private readonly StudentRole _intruderRole = new() { ExternalStudentId = IntruderId };
    private readonly ITransactor _transactor = A.Fake<ITransactor>();
    private readonly ITransaction _transaction = A.Fake<ITransaction>();
    private readonly ILessonSuggestionDao _suggestionDao = A.Fake<ILessonSuggestionDao>();
    private readonly ILessonDao _lessonDao = A.Fake<ILessonDao>();
    private readonly TimetableService _service;
}
