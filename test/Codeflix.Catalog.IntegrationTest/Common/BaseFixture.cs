using Bogus;

namespace Codeflix.Catalog.IntegrationTest.Common;

public abstract class BaseFixture
{
    public Faker Faker { get; } = new("pt_BR");
}