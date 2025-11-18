using Database.Entities;
using Endpoints.Interfaces.Authorization;
using FakeItEasy;
using Services.Interfaces;

namespace Services.Tests;

public class ResourceServiceTests
{
    public ResourceServiceTests()
    {
        A.CallTo(() => _transactor.BeginTransaction()).Returns(_transaction).Once();
        A.CallTo(() => _transaction.TutorDao).Returns(_tutorDao);
        A.CallTo(() => _transaction.ResourceDao).Returns(_resourceDao);
        A.CallTo(() => _tutorDao.GetTutor()).Returns(new DbTutor
        {
            ResourcePathPrefix = "tutor",
        });
        _service = new ResourceService(_transactor, _fileStorage);
    }

    [Fact]
    public void DownloadResourceAsTutor()
    {
        A.CallTo(() => _resourceDao.GetResourceByGuid(_resourceGuid)).Returns(new DbResource
        {
            Filename = "resource_name",
        });
        A.CallTo(() => _fileStorage.GetDownloadUrl("tutor/resource_name"))
            .Returns("protocol://tutor/resource_name");
        var dto = _service.GetDownloadUrlForTutor(_resourceGuid, new TutorRole());
        Assert.Equal("protocol://tutor/resource_name", dto.Url);
    }

    [Fact]
    public void DownloadNonexistingResource()
    {
        A.CallTo(() => _resourceDao.GetResourceByGuid(_resourceGuid)).Returns(null);
        var dto = _service.GetDownloadUrlForTutor(_resourceGuid, new TutorRole());
        Assert.Null(dto.Url);
    }

    [Fact]
    public void UploadResource()
    {
        A.CallTo(() => _fileStorage.GetUploadUrl("tutor/geometry.pdf"))
            .Returns("protocol://tutor/geometry.pdf");
        var dto = _service.BeginUpload("geometry.pdf", new TutorRole());
        A.CallTo(() => _resourceDao.SaveSingleResource(
                "geometry.pdf", "(single) geometry.pdf"))
            .MustHaveHappenedOnceExactly();
        A.CallTo(() => _transaction.Commit()).MustHaveHappenedOnceExactly();
        Assert.Equal("protocol://tutor/geometry.pdf", dto.Url);
    }

    private readonly Guid _resourceGuid = Guid.Parse("537A0371-1F8A-4635-8DAA-E0462442DD9A");

    private readonly ITransactor _transactor = A.Fake<ITransactor>();
    private readonly ITransaction _transaction = A.Fake<ITransaction>();
    private readonly IResourceDao _resourceDao = A.Fake<IResourceDao>();
    private readonly ITutorDao _tutorDao = A.Fake<ITutorDao>();
    private readonly IFileStorageClient _fileStorage = A.Fake<IFileStorageClient>();
    private readonly ResourceService _service;
}
