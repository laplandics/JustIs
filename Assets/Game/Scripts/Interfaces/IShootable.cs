public interface IShootable
{
    public void TakeAim(float time, out bool isShot);
    public void ReleaseAim();
}