namespace DoFramework.Processing;

public interface IContextVerifier
{
    bool Verify();

    IContextVerifier ComposedBy(string composerName);

    IContextVerifier ConfirmKey(string key);

    IContextVerifier ProcessSucceeded(string processName);
}
