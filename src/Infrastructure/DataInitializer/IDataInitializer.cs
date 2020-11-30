using Domain.Common.Dependencies;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.DataInitializer
{
    public interface IDataInitializer : IScoped
    {
        void Init();
    }
}
