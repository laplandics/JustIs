using System.Collections;

public interface IGameService
{
    public IEnumerator Run();
    public void Stop();
}