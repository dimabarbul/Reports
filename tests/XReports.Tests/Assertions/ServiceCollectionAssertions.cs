using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using FluentAssertions.Collections;
using Microsoft.Extensions.DependencyInjection;

namespace XReports.Tests.Assertions
{
    public class ServiceCollectionAssertions : GenericCollectionAssertions<ServiceDescriptor>
    {
        public ServiceCollectionAssertions(IEnumerable<ServiceDescriptor> actualValue)
            : base(actualValue)
        {
        }

        protected override string Identifier => "service collection";

        public AndConstraint<ServiceCollectionAssertions> ContainDescriptor<TServiceType, TImplementationType>(ServiceLifetime lifetime)
            where TImplementationType : TServiceType
        {
            ServiceDescriptor serviceDescriptor = this.Subject.FirstOrDefault(sd => sd.ServiceType == typeof(TServiceType));
            serviceDescriptor.Should().NotBeNull("{context} should contain service descriptor for service type {0}", typeof(TServiceType));

            serviceDescriptor.Lifetime.Should().Be(lifetime);
            serviceDescriptor.ImplementationType.Should().Be<TImplementationType>();

            return new AndConstraint<ServiceCollectionAssertions>(this);
        }

        public AndConstraint<ServiceCollectionAssertions> ContainDescriptors<TServiceType>(ServiceLifetime lifetime, params Type[] implementationTypes)
        {
            ServiceDescriptor[] descriptors = this.Subject
                .Where(sd => sd.ServiceType == typeof(TServiceType))
                .ToArray();

            descriptors.Should().OnlyContain(d => d.Lifetime == lifetime);
            descriptors.Select(h => h.ImplementationType)
                .Should().BeEquivalentTo(implementationTypes);

            return new AndConstraint<ServiceCollectionAssertions>(this);
        }

        public AndConstraint<ServiceCollectionAssertions> NotContainDescriptor<TServiceType>()
        {
            this.Subject.Should().NotContain(sd => sd.ServiceType == typeof(TServiceType));

            return new AndConstraint<ServiceCollectionAssertions>(this);
        }
    }
}