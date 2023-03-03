using FluentAssertions;
using NetArchTest.Rules;
using Xunit;

namespace TPSecurity.Architecture.Tests;

public class ArchitectureTests
{

    private const string DomainNamespace = "TPSecurity.Domain";
    private const string ApplicationNamespace = "TPSecurity.Application";
    private const string InfrastructureNamespace = "TPSecurity.Infrastructure";
    private const string ContractsNamespace = "TPSecurity.Contracts";
    private const string ApiNamespace = "TPSecurity.Api";

    [Fact]
    public void DomainLayer_ShouldNot_HaveDependencyOnOtherProjects()
    {
        //Arrange
        var assembly = typeof(Domain.AssemblyReference).Assembly;

        var otherProjects = new[]
        {
            ApplicationNamespace,
            InfrastructureNamespace,
            ContractsNamespace,
            ApiNamespace
        };

        //Act
        var testResult = Types
            .InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOnAny(otherProjects)
            .GetResult();

        //Assert
        testResult.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void ApplicationLayer_ShouldNot_HaveDependencyOnOtherProjects()
    {
        //Arrange
        var assembly = typeof(Application.AssemblyReference).Assembly;

        var otherProjects = new[]
        {
            InfrastructureNamespace,
            ContractsNamespace,
            ApiNamespace
        };

        //Act
        var testResult = Types
            .InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOnAny(otherProjects)
            .GetResult();

        //Assert
        testResult.IsSuccessful.Should().BeTrue();
    } 
    
    [Fact]
    public void InfrastructureLayer_ShouldNot_HaveDependencyOnOtherProjects()
    {
        //Arrange
        var assembly = typeof(Infrastructure.AssemblyReference).Assembly;

        var otherProjects = new[]
        {
            ContractsNamespace,
            ApiNamespace
        };

        //Act
        var testResult = Types
            .InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOnAny(otherProjects)
            .GetResult();

        //Assert
        testResult.IsSuccessful.Should().BeTrue();
    }
    
    [Fact]
    public void Contracts_Should_Not_HaveDependencyOnOtherProjects()
    {
        //Arrange
        var assembly = typeof(Contracts.AssemblyReference).Assembly;

        var otherProjects = new[]
        {      
            InfrastructureNamespace,
            ApplicationNamespace,
            DomainNamespace,
            ApiNamespace
        };

        //Act
        var testResult = Types
            .InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOnAny(otherProjects)
            .GetResult();

        //Assert
        testResult.IsSuccessful.Should().BeTrue();
    }


    [Fact]
    public void Handlers_Should_Have_DependencyOnDomain()
    {
        var assembly = typeof(Application.AssemblyReference).Assembly;

        var testResult = Types
            .InAssembly(assembly)
            .That()
            .HaveNameEndingWith("Handler")
            .Should()
            .HaveDependencyOn(DomainNamespace)
            .GetResult();

        testResult.IsSuccessful.Should().BeTrue();
    }
}
