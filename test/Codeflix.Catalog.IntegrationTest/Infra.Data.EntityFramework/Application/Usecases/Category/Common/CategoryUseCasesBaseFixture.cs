using Codeflix.Catalog.IntegrationTest.Common;

namespace Codeflix.Catalog.IntegrationTest.Infra.Data.EntityFramework.Application.Usecases.Category.Common;

public class CategoryUseCasesBaseFixture : BaseFixture
{
    public string GetValidName() => Faker.Random.String2(3, 255);

    public string GetValidDescription() => Faker.Random.String2(1, 10000);

    public bool GetValidIsActive() => Faker.Random.Bool();

    public Domain.Entities.Category GetValidCategory() => new(
        GetValidName(),
        GetValidDescription(),
        GetValidIsActive()
    );

    public List<Domain.Entities.Category> GetCategoryList(int length = 10) =>
        Enumerable.Range(0, length).Select(_ => GetValidCategory()).ToList();
}