public static class DataInjector
{
    public static TState InjectState<TState>() where TState : SpecialGameState => G.GetService<SpecialGameStatesService>().GetState<TState>();
}