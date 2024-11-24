using System.Reflection;
using System.Text.Json;
using System.Xml.Serialization;
using ContainerIntegrationTestsDemo.Application.Extensions;
using ContainerIntegrationTestsDemo.Contracts.Queue;
using ContainerIntegrationTestsDemo.Data.Extensions;
using ContainerIntegrationTestsDemo.IntegrationTests.Helpers;
using ContainerIntegrationTestsDemo.IntegrationTests.Mocks;
using ContainerIntegrationTestsDemo.Model;
using ContainerIntegrationTestsDemo.QueueConsumer.Consumers;
using ContainerIntegrationTestsDemo.QueueConsumer.Extensions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.MsSql;
using Xunit.Abstractions;

namespace ContainerIntegrationTestsDemo.IntegrationTests;

public class IntegrationTest1 : IClassFixture<MsSqlContainerFixture>, IAsyncLifetime
{
    private readonly ITestOutputHelper _output;
    private ServiceProvider _serviceProvider;
    private readonly MsSqlContainer _msSqlContainer;
    private string _dbName;

    public IntegrationTest1(ITestOutputHelper output, MsSqlContainerFixture msSqlContainerFixture)
    {
        _output = output;
        _msSqlContainer = msSqlContainerFixture.MsSqlContainer;
    }
    
    public async Task InitializeAsync()
    {
        _dbName = $"IntegrationTestsDb-{Guid.NewGuid():N}";

        var connectionString = $"{_msSqlContainer.GetConnectionString()};";

        var connectionStringBuilder = new SqlConnectionStringBuilder(connectionString)
        {
            InitialCatalog = _dbName
        };

        var updatedConnectionString = connectionStringBuilder.ConnectionString;

        var serviceCollection = new ServiceCollection()
            .AddApplication()
            .AddDataWithoutContext()
            .AddMsSqlTestContainer(updatedConnectionString)
            .AddQueueConsumer();

        _serviceProvider = serviceCollection.BuildServiceProvider();
        await ScopedTestDataRepository().SetupDatabase();
        await ScopedTestDataRepository().ClearDatabase();
    }

    public async Task DisposeAsync()
    {
        await ScopedTestDataRepository().DeleteDb();
        await _serviceProvider.DisposeAsync();
    }

    [Fact]
    public async Task TestDbSave()
    {
        _output.WriteLine($"{ScopedTestDataRepository().GetCurrentConnectionString()}");

        //When: events arrive
        await WhenCustomerMessageArrives("IncomingEvents/C01_CustomerInformationEvent.xml");
        
        //Then: situation in database after test
        ThenCustomerIsInExpectedState("ExpectedResultsModelState/C01_ExpectedCustomer.json");
        
        Assert.True(true);
        
    }

    private async Task WhenCustomerMessageArrives(string eventMessageFilePath)
    {
        var customerMessage = DeserializeFromXmlFile<CustomerMessage>(eventMessageFilePath);
        var customerInformationConsumer = GetRequiredScopedService<CustomerInformationConsumer>();
        await customerInformationConsumer.HandleQueueMessage(customerMessage);
    }

    private void ThenCustomerIsInExpectedState(string expectedEndStateFilePath)
    {
        var expectedCustomer = DeserializeFromJsonFile<Customer>(expectedEndStateFilePath);
        ArgumentNullException.ThrowIfNull(expectedCustomer);
        var customerAfterService = ScopedTestDataRepository().GetCustomer(expectedCustomer.FirstName);

        customerAfterService.Should().BeEquivalentTo(expectedCustomer, options => 
            options.Excluding(c => c.Id));
    }
    
    static T DeserializeFromXmlFile<T>(string relativeFilePath)
    {
        var filePath = DetermineAndValidateFullFilePath(relativeFilePath);
        
        var serializer = new XmlSerializer(typeof(T));
        using (var fileStream = new FileStream(filePath, FileMode.Open))
        {
            return (T)serializer.Deserialize(fileStream);
        }
    }
    
    static T? DeserializeFromJsonFile<T>(string relativeFilePath)
    {
        var filePath = DetermineAndValidateFullFilePath(relativeFilePath);

        // Read all text from the file and deserialize
        string json = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<T>(json);
    }

    private static string DetermineAndValidateFullFilePath(string relativePath)
    {
        var codeBaseUrl = new Uri(Assembly.GetExecutingAssembly().Location);
        var codeBasePath = Uri.UnescapeDataString(codeBaseUrl.AbsolutePath);
        var dirPath = Path.GetDirectoryName(codeBasePath) ?? throw new ArgumentNullException(nameof(codeBasePath));
        var filePath = Path.Combine(dirPath, relativePath);

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"File not found: {filePath}");
        }

        return filePath;
    }

    private TestDataRepository ScopedTestDataRepository() => GetRequiredScopedService<TestDataRepository>();

    private T GetRequiredScopedService<T>()
    {
        return _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<T>();
    }
}