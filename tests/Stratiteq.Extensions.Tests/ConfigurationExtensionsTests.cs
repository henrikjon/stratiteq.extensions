﻿// Copyright (c) Stratiteq Sweden AB. All rights reserved.
//
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using Stratiteq.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace Stratiteq.Extensions.Tests
{
    public class ConfigurationExtensionsTests
    {
        private IConfiguration configuration = null;

        [SetUp]
        public void SetupFixture()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            this.configuration = builder.Build();
        }

        [Test]
        public void Get_Valid_AzureADConfiguration()
        {
            var validConfiguration = this.configuration.GetSection("Configuration1").GetValid<AzureADConfiguration>();

            Assert.IsNotNull(validConfiguration);
        }

        [Test]
        public void Get_Valid_AzureADConfiguration_With_Default_Scopes()
        {
            var validConfiguration = this.configuration.GetSection("Configuration2").GetValid<AzureADConfiguration>();

            Assert.IsNotNull(validConfiguration);
        }

        [Test]
        public void Get_Valid_CertificateConfiguration()
        {
            var validConfiguration = this.configuration.GetSection("CertificateConfiguration1").GetValid<CertificateConfiguration>();

            Assert.IsNotNull(validConfiguration);
        }

        [Test]
        public void Invalid_Configuration_Should_Fail_Validation()
        {
            Assert.Throws<ValidationException>(() => this.configuration?.GetSection("InvalidConfiguration1").GetValid<AzureADConfiguration>());
        }

        [Test]
        public void Configuration_With_Missing_AppIdentifier_Should_Fail()
        {
            Assert.Throws<ValidationException>(() => this.configuration.GetSection("InvalidConfiguration3").GetValid<AzureADConfiguration>());
        }

        [Test]
        public void Configuration_With_Invalid_AppIdentifier_Should_Fail()
        {
            Assert.Throws<ValidationException>(() => this.configuration.GetSection("InvalidConfiguration4").GetValidUri("AppIdentifier"));
        }

        [Test]
        public void Configuration_With_Missing_Key_Should_Fail()
        {
            Assert.Throws<ValidationException>(() => this.configuration.GetSection("InvalidConfiguration4").GetValidValue<string>("TenantId"));
        }

        [Test]
        public void GetConfigurationWithAppIdentifier()
        {
            var validConfiguration =
                this.configuration.GetSection("Configuration1").GetValid<AzureADConfiguration>("http://AppIdentifier");

            Assert.IsNotNull(validConfiguration);
            Assert.AreEqual(validConfiguration.AppIdentifier, "http://AppIdentifier");
        }

        [Test]
        public void Configuration_With_Different_AppIdentifier_Should_Have_Default_Scope()
        {
            var validConfiguration =
                this.configuration.GetSection("Configuration2").GetValid<AzureADConfiguration>("http://AppIdentifier");

            Assert.IsNotNull(validConfiguration);
            Assert.AreEqual(validConfiguration.Scopes[0], "http://AppIdentifier/.default");
        }
    }
}
