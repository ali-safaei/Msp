using Domain.Common.Dependencies;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Abstractions.Identity
{
    public interface ICurrentUser : ITransient
    {
        public string UserId { get; }
        public bool IsAuthenticated { get; }
    }
}
