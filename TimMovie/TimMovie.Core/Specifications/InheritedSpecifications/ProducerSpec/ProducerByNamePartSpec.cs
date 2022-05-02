using TimMovie.Core.Entities;
using TimMovie.SharedKernel.Specification;

namespace TimMovie.Core.Specifications.InheritedSpecifications.ProducerSpec;

/// <summary>
/// Фильтр на получение продюссеров
/// </summary>
public class ProducerByNamePartSpec : Specification<Producer>
{
    /// <summary>
    /// Создает фильтр для получения продюссеров по части имени и фамилии
    /// </summary>
    /// <param name="namePart"></param>
    public ProducerByNamePartSpec(string namePart) =>
        Conditional = p =>
            p.Surname == null ? p.Name.Contains(namePart) : (p.Name + " " + p.Surname).Contains(namePart);
}