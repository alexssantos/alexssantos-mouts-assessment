using System;
using MediatR;

namespace Ambev.DeveloperEvaluation.Domain.Common
{
    public abstract class DomainEvent : INotification
    {
        public DateTime Timestamp { get; }
        public bool IsPublished { get; set; }

        protected DomainEvent()
        {
            Timestamp = DateTime.UtcNow;
        }
    }
}