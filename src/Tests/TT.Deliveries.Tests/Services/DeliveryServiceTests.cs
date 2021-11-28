using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using SharedCodeLibrary.Interfaces;
using SharedCodeLibrary.Providers;
using System;
using System.Data;
using System.Threading.Tasks;
using TT.Deliveries.Core.Entities;
using TT.Deliveries.Core.Exceptions;
using TT.Deliveries.Core.Interfaces;
using TT.Deliveries.Core.Services;
using TT.Deliveries.Core.Validators;

namespace TT.Deliveries.Tests.Controllers
{
    [TestFixture]
    public class DeliveryServiceTests
    {
        private readonly Guid NewlyCreatedDeliveryId = Guid.NewGuid();

        [Test]
        public void AddDeliveryAsync_Throws_DeliveryNullException_WhenDelivery_Is_Null()
        {
            var services = GetServices();
            var deliveryService = services.GetService<IDeliveryService>();

            Assert.ThrowsAsync<DeliveryNullException>(async() => await deliveryService.AddDeliveryAsync(null));
        }

        [Test]
        public async Task AddDeliveryAsync_Successful()
        {
            var services = GetServices();
            var deliveryService = services.GetService<IDeliveryService>();
            var dateTimeProvider = services.GetService<IDateTimeProvider>();

            var startDate = dateTimeProvider.GetCurrentTimeUTC();
            var endDate = startDate.AddDays(1);

            var delivery = new Delivery()
            {
                AccessWindow = new AccessWindow()
                {
                    StartTime = startDate,
                    EndTime = endDate
                },
                Order = new Order()
                {
                    OrderDetails = new OrderDetails()
                    {
                        OrderNumber = "123",
                        Sender = "test"
                    }
                },
                Recipient = new DeliveryRecipient()
                {
                    Address = "test address",
                    Email = "test@email.com",
                    Name = "test",
                    PhoneNumber = "12345678"
                }
            };

            var newDeliveryId = await deliveryService.AddDeliveryAsync(delivery);

            Assert.AreEqual(NewlyCreatedDeliveryId, newDeliveryId);
        }

        private IServiceProvider GetServices()
        {
            var dateTimeFormatter = "yyyy-MM-dd";

            return new ServiceCollection()
                .AddSingleton<IDbConnectionProvider, DummyDbConnectionProvider>()
                .AddSingleton<IDeliveryValidator, DeliveryValidator>()
                .AddTransient<IDeliveryService, DeliveryService>()
                .AddSingleton<IDateTimeProvider>(new DateTimeProvider(dateTimeFormatter))
                .AddSingleton(GetDeliveryDataAccess())
                .AddSingleton(GetOrderDataAccess())
                .BuildServiceProvider();
        }

        private static IOrderDataAccess GetOrderDataAccess()
        {
            return new Mock<IOrderDataAccess>().Object;
        }

        private IDeliveryDataAccess GetDeliveryDataAccess()
        {
            var mockDeliveryDataAccess = new Mock<IDeliveryDataAccess>();

            mockDeliveryDataAccess
                .Setup(d => d.AddDeliveryAsync(It.IsAny<IDbConnection>(),
                    It.IsAny<IDbTransaction>(),
                    It.IsAny<Delivery>()))
                .ReturnsAsync(NewlyCreatedDeliveryId);

            return mockDeliveryDataAccess.Object;
        }
    }
}
