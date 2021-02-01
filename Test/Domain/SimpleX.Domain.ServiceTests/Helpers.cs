using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using Moq;
using SimpleX.Data.Context;
using SimpleX.Domain.Models.Relations;
using SimpleX.Domain.Services.Helpers;
using SimpleX.Domain.Services.Relations;
using SimpleX.Domain.Validation;
using SimpleX.Helpers.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SimpleX.Domain.ServiceTests
{
    public static class Helpers
    {
        public static IDbContextFactory<SimpleContext> GetTestDbContextFactory()
        {
            var factoryMock = new Mock<IDbContextFactory<SimpleContext>>();
            factoryMock.Setup(f => f.CreateDbContext()).Returns(GetTestDbContext());
            return factoryMock.Object;
        }

        private static SimpleContext GetTestDbContext()
        {
            var options = new DbContextOptionsBuilder<SimpleContext>();
            options.UseSqlServer("");

            var context = new SimpleContext(options.Options, GetTestLoggerFactory().CreateLogger<SimpleContext>(), GetTestAuthenticationService());
            return context;
        }

        public static IMapper GetTestMapper()
        {
            var configuration = new MapperConfiguration(cfg =>
                cfg.AddMaps(Assembly.Load("SimpleX.Domain.Mapper")));

            return configuration.CreateMapper();
        }

        public static IAuthenticationService GetTestAuthenticationService()
        {
            var mock = new Mock<IAuthenticationService>();
            mock.Setup(s => s.CurrentUsername).Returns("Test");
            mock.Setup(s => s.CurrentUserLanguageCode).Returns("nl");

            return mock.Object;
        }

        public static IValidator<T> GetTestValidator<T>()
        {
            return (IValidator<T>)Assembly.Load("SimpleX.Domain.Validation").GetTypes().FirstOrDefault(t => t.IsSubclassOf(typeof(T)));
        }

        public static ILoggerFactory GetTestLoggerFactory()
        {
            return new Mock<ILoggerFactory>().Object;
        }

        public static DefaultValueSetter GetDefValueSetter()
        {
            return new DefaultValueSetter(GetTestLoggerFactory());
        }

        public static ICustomerService GetTestCustomerService()
        {
            var service = new CustomerService(
                GetTestLoggerFactory(),
                GetTestAuthenticationService(),
                GetTestDbContextFactory(), 
                GetTestMapper(),
                GetTestValidator<CustomerDto>(),
                GetDefValueSetter());

            return service;
        }
    }
}
