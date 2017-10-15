using System.Threading.Tasks;
using Xunit;

namespace Construktion.Samples
{
    public abstract class IntegrationTestBase : IAsyncLifetime
    {
        public virtual Task InitializeAsync() => Task.CompletedTask;

        public virtual Task DisposeAsync() => Task.CompletedTask;
    }
}