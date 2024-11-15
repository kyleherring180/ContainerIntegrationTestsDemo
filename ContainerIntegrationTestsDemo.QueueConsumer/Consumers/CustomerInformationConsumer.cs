using ContainerIntegrationTestsDemo.Application.Abstraction.Services;
using ContainerIntegrationTestsDemo.Contracts.Queue;
using ContainerIntegrationTestsDemo.QueueConsumer.Extensions.MapToModel;

namespace ContainerIntegrationTestsDemo.QueueConsumer.Consumers;

public class CustomerInformationConsumer
{
    private readonly ICustomerService _customerService;

    public CustomerInformationConsumer(ICustomerService customerService)
    {
        _customerService = customerService;
    }
    
    public async Task HandleQueueMessage(CustomerMessage customerMessage )
    {
        await _customerService.CustomerEvent(customerMessage.ToModel());
    }
}