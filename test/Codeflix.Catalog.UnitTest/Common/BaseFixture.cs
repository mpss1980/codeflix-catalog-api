using Bogus;

namespace Codeflix.Catalog.UnitTest.Common;

public abstract class BaseFixture
{
    public Faker Faker { get; } = new("pt_BR");
}