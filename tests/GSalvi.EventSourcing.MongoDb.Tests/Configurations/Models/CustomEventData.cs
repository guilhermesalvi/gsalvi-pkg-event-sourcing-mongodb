using System;
using System.Diagnostics.CodeAnalysis;

namespace GSalvi.EventSourcing.MongoDb.Tests.Configurations.Models;

[ExcludeFromCodeCoverage]
public class CustomEventData : EventData
{
    public Guid UserId { get; set; }
}