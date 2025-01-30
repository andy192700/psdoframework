using DoFramework.Domain;

namespace DoFramework.Processing;

public interface IComposerBuilder
{
    IComposer Build(ComposerDescriptor composerDescriptor);
}
