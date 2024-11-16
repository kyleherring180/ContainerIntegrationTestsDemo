using Testcontainers.MsSql;

namespace ContainerIntegrationTestsDemo.IntegrationTests.Helpers;

public class MsSqlContainerFixture : IAsyncLifetime
{
    public MsSqlContainer MsSqlContainer { get; private set; }
    
    public async Task InitializeAsync()
    {
        MsSqlContainer = new MsSqlBuilder().Build();
        await MsSqlContainer.StartAsync();
    }

    public async Task DisposeAsync()
    {
        await MsSqlContainer.StopAsync();
    }
}