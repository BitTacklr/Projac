namespace Projac.Testing
{
    /// <summary>
    /// The act of building a projection test specification.
    /// </summary>
    public interface ITestSpecificationBuilder
    {
        /// <summary>
        /// Builds the test specification thus far.
        /// </summary>
        /// <returns>The test specification.</returns>
        TestSpecification Build();
    }
}