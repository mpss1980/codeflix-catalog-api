using Codeflix.Catalog.Application.Interfaces;
using Codeflix.Catalog.Domain.Repositories;
using Codeflix.Catalog.UnitTest.Common;
using Moq;

namespace Codeflix.Catalog.UnitTest.Application.Usecases.Category.Common;

public abstract class CategoryUsecasesBaseFixture : BaseFixture
{
    public string GetValidName() => Faker.Random.String2(3, 255);

    public string GetValidDescription() => Faker.Random.String2(1, 10000);

    public bool GetValidIsActive() => Faker.Random.Bool();
    
    public Mock<ICategoryRepository> GetCategoryRepositoryMock() => new Mock<ICategoryRepository>();
    
    public Mock<IUnitOfWork> GetUnitOfWorkMock() => new Mock<IUnitOfWork>();
    
    public Catalog.Domain.Entities.Category GetValidCategory() => new(
        GetValidName(),
        GetValidDescription(),
        GetValidIsActive()
    );
}