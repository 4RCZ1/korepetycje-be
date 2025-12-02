using Database.Entities;
using Endpoints.Dto;
using Endpoints.Interfaces;
using Endpoints.Interfaces.Authorization;
using FakeItEasy;
using Services.Interfaces;

namespace Services.Tests;

public class ResourceGroupTests
{
    public ResourceGroupTests()
    {
        A.CallTo(() => _transactor.BeginTransaction()).Returns(_transaction).Once();
        A.CallTo(() => _transaction.ResourceDao).Returns(_resourceDao);
        _service = new ResourceService(_transactor, A.Dummy<IFileStorageClient>());
    }

    [Fact]
    public void CreateResourceGroup()
    {
        A.CallTo(() => _resourceDao.GetResourceByGuid(_resourceGuid1)).Returns(new DbResource
        {
            Filename = string.Empty,
            Id = 1,
        });
        A.CallTo(() => _resourceDao.GetResourceByGuid(_resourceGuid2)).Returns(new DbResource
        {
            Filename = string.Empty,
            Id = 2,
        });
        A.CallTo(() => _resourceDao.SaveResourceGroup(A<DbResourceGroup>._))
            .Invokes((DbResourceGroup g) =>
            {
                Assert.Equivalent(new DbResourceGroup
                {
                    IsSingle = false,
                    Name = "group name",
                    Memberships = [
                        new DbResourceMembership { ResourceId = 1 },
                        new DbResourceMembership { ResourceId = 2 },
                    ],
                }, g, strict: true);
            });
        var dto = new ResourceGroupDto
        {
            Name = "group name",
            Resources = [
                new ResourceDto
                {
                    Id = _resourceGuid1.ToString(),
                    Name = string.Empty,
                },
                new ResourceDto
                {
                    Id = _resourceGuid2.ToString(),
                    Name = string.Empty,
                },
            ],
        };
        _service.CreateResourceGroup(dto, new TutorRole());
        A.CallTo(() => _resourceDao.SaveResourceGroup(A<DbResourceGroup>._))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public void ThrowWhenResourceNotFound()
    {
        A.CallTo(() => _resourceDao.GetResourceByGuid(_resourceGuid1)).Returns(null);
        var dto = new ResourceGroupDto
        {
            Name = "group name",
            Resources = [
                new ResourceDto
                {
                    Id = _resourceGuid1.ToString(),
                    Name = string.Empty,
                },
            ],
        };
        var action = () => _service.CreateResourceGroup(dto, new TutorRole());
        var exception = Assert.Throws<BadRequestException>(action);
        Assert.StartsWith("Nie znaleziono jednego z podanych zasobów", exception.Message);
    }

    private readonly Guid _resourceGuid1 = Guid.Parse("A7B40FF1-F32B-4BB9-932E-7C1970CE7D2B");
    private readonly Guid _resourceGuid2 = Guid.Parse("9F362979-9275-447F-B29A-5F36612D5C8D");

    private readonly ITransactor _transactor = A.Fake<ITransactor>();
    private readonly ITransaction _transaction = A.Fake<ITransaction>();
    private readonly IResourceDao _resourceDao = A.Fake<IResourceDao>();
    private readonly ResourceService _service;
}
