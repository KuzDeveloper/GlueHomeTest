using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using SharedCodeLibrary.Interfaces;
using SharedCodeLibrary.Providers;
using System;
using TT.Deliveries.Core.Entities;
using TT.Deliveries.Core.Exceptions;
using TT.Deliveries.Core.Interfaces;
using TT.Deliveries.Core.Validators;

namespace TT.Deliveries.Tests.Controllers
{
    [TestFixture]
    public class DeliveryValidatorTests
    {
        [Test]
        public void ValidateForCreation_Throws_DeliveryNullException_WhenDelivery_Is_Null()
        {
            var services = GetServices();
            var validator = services.GetService<IDeliveryValidator>();

            Assert.Throws<DeliveryNullException>(() => validator.ValidateForCreation(null));
        }

        [Test]
        public void ValidateForCreation_Throws_DeliveryAccessWindowException_When_Dates_Outside_Of_Current_Date()
        {
            var services = GetServices();
            var validator = services.GetService<IDeliveryValidator>();
            var dateTimeProvider = services.GetService<IDateTimeProvider>();

            var startDate = dateTimeProvider.GetCurrentTimeUTC().AddDays(1);
            var endDate = startDate.AddDays(1);

            var delivery = new Delivery()
            {
                AccessWindow = new AccessWindow()
                {
                    StartTime = startDate,
                    EndTime = endDate
                }
            };

            Assert.Throws<DeliveryAccessWindowException>(() => validator.ValidateForCreation(delivery));
        }

        private static IServiceProvider GetServices()
        {
            var dateTimeFormatter = "yyyy-MM-dd";

            return new ServiceCollection()
                .AddSingleton<IDeliveryValidator, DeliveryValidator>()
                .AddSingleton<IDateTimeProvider>(new DateTimeProvider(dateTimeFormatter))
                .BuildServiceProvider();
        }
    }
}
