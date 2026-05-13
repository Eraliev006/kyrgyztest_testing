using KyrgyzTest.Core.Entities;

namespace KyrgyzTest.Core.Interfaces;

public interface ISectionConfigRepository
{
    Task<SectionConfig?> GetById(Guid id);
    Task<List<SectionConfig>> GetAll();
    Task<SectionConfig> Create(SectionConfig config);
    Task<SectionConfig> Update(SectionConfig config);
    Task<SectionConfig> Delete(Guid id);
}
