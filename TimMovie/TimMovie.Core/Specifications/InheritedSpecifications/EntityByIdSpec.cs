using TimMovie.SharedKernel.BaseEntities;
using TimMovie.SharedKernel.Specification;

namespace TimMovie.Core.Specifications.InheritedSpecifications;

public class EntityByIdSpec<TEntity>: Specification<TEntity> 
    where TEntity: BaseEntity
{
    public EntityByIdSpec(Guid id)
    {
        Conditional = entity => entity.Id == id;
    }
}