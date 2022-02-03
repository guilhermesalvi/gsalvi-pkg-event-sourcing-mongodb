using System;
using System.Diagnostics.CodeAnalysis;

namespace GSalvi.EventSourcing.MongoDb.Tests.Configurations.Models;

[ExcludeFromCodeCoverage]
public class CustomerUpdated
{
    public Guid CustomerId { get; set; }
    public string? CustomerName { get; set; }
}