using System;

namespace Domain.Abstractions
{
    public interface IEntity
    {
        Guid Id { get; }
    }
}