using Application.Abstractions.Dtos;
using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Abstractions.Mapping
{
    public interface ICustomMapper<TEntity, TDto>
        where TEntity : IBaseEntity
        where TDto : IBaseDto
    {
        TEntity ToEntity(TDto dto);
        TDto ToDto(TEntity entity);
    }
}
