public class ConsoleCommands
{
    [ConsoleCommand("changeStage")]
    private static void ChangeStage(int stageNum)
    {
        var stage = (StageNum)stageNum;
        EventService.Invoke(new OnStageEndEvent {NextStage = stage});
    }

    [ConsoleCommand("spawn")]
    private static void Spawn(string sp)
    {
        var spawnName = char.ToUpper(sp[0]) + sp[1..];
        if (!G.GetService<ObjectDataService>().TryGetObjectConfigByName(spawnName, out var config)) return;
        config.Spawn(true);
    }
}