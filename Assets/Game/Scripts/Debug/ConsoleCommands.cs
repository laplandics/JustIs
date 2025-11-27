public class ConsoleCommands
{
    [ConsoleCommand("changeStage")]
    private static void ChangeStage(int stageNum)
    {
        var stage = (StageNum)stageNum;
        EventService.Invoke(new StageEvents.OnStageEndEvent {NextStage = stage});
    }

    [ConsoleCommand("spawn")]
    private static void Spawn(string sp)
    {
        var spawnName = char.ToUpper(sp[0]) + sp[1..] + "_CO";
        var config = G.GetService<ConfigsService>().GetConfigByName(spawnName);
        config.SpawnDebug();
    }
}