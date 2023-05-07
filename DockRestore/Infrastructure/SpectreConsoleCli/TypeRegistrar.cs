// This code was derived from an example in the Spectre.Console repository (MIT License).
// Original source: https://github.com/spectreconsole/spectre.console/blob/main/examples/Cli/Injection/Infrastructure/TypeRegistrar.cs
// Copyright (c) 2020 Patrik Svensson, Phil Scott, Nils Andresen

using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

namespace DockRestore.Infrastructure.SpectreConsoleCli;

/// <summary>
/// A custom implementation of <see cref="ITypeRegistrar"/> for integrating Spectre.Console.Cli with the dependency injection system.
/// </summary>
public sealed class TypeRegistrar : ITypeRegistrar
{
    private readonly IServiceCollection _builder;

    /// <summary>
    /// Initializes a new instance of the <see cref="TypeRegistrar"/> class.
    /// </summary>
    /// <param name="builder">An <see cref="IServiceCollection"/> object used for registering services.</param>
    public TypeRegistrar(IServiceCollection builder)
    {
        _builder = builder;
    }

    /// <inheritdoc/>
    public ITypeResolver Build()
    {
        return new TypeResolver(_builder.BuildServiceProvider());
    }

    /// <inheritdoc/>
    public void Register(Type service, Type implementation)
    {
        _builder.AddSingleton(service, implementation);
    }

    /// <inheritdoc/>
    public void RegisterInstance(Type service, object implementation)
    {
        _builder.AddSingleton(service, implementation);
    }

    /// <inheritdoc/>
    public void RegisterLazy(Type service, Func<object> func)
    {
        if (func is null)
        {
            throw new ArgumentNullException(nameof(func));
        }

        _builder.AddSingleton(service, (provider) => func());
    }
}