// This code was derived from an example in the Spectre.Console repository (MIT License).
// Original source: https://github.com/spectreconsole/spectre.console/blob/main/examples/Cli/Injection/Infrastructure/TypeResolver.cs
// Copyright (c) 2020 Patrik Svensson, Phil Scott, Nils Andresen

using Spectre.Console.Cli;

namespace DockRestore.Infrastructure.SpectreConsoleCli;

/// <summary>
/// Represents a type resolver used to resolve dependencies for the Spectre.Console CLI.
/// </summary>
public sealed class TypeResolver : ITypeResolver, IDisposable
{
    private readonly IServiceProvider _provider;

    /// <summary>
    /// Initializes a new instance of the <see cref="TypeResolver"/> class.
    /// </summary>
    /// <param name="provider">The service provider used for resolving dependencies.</param>
    /// <exception cref="ArgumentNullException">Thrown if the provider is null.</exception>
    public TypeResolver(IServiceProvider provider)
    {
        _provider = provider ?? throw new ArgumentNullException(nameof(provider));
    }

    /// <inheritdoc/>
    public object? Resolve(Type? type)
    {
        return type is null ? null : _provider.GetService(type);
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        if (_provider is IDisposable disposable)
        {
            disposable.Dispose();
        }
    }
}