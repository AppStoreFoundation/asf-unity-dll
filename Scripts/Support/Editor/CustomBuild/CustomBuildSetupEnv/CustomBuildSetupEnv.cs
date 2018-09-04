public abstract class CustomBuildSetupEnv
{
    internal virtual void Setup(AppcoinsGameObject a)
    {
        a.CheckAppcoinsGameobject();
    }
}