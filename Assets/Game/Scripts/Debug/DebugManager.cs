using UnityEngine;
using UnityEngine.InputSystem;

public class DebugManager : MonoBehaviour, ISceneManager, IInputsChanger
{
    [SerializeField] private DebugConsole consolePrefab; 
    private DebugConsole _console;
    private GameInputs _gameInputs;
    private Canvas _globalCanvas;
    
    public InputsType InputsType => InputsType.Debug;
    
    public void Initialize()
    {
        ConsoleCommandsHandler.RegisterConsoleCommands();
        _gameInputs = new GameInputs();
        _gameInputs.Debug.Enable();
        _gameInputs.Debug.CallDebugConsole.performed += CallDebugConsole;
    }

    private void CallDebugConsole(InputAction.CallbackContext _)
    {
        _gameInputs.Debug.CallDebugConsole.performed -= CallDebugConsole;
        if (_globalCanvas == null) _globalCanvas = DataInjector.InjectState<GlobalCanvasSpawned>().Get();
        _console = SpawnManager.Spawn(consolePrefab, Vector3.zero, Quaternion.identity, _globalCanvas.transform);
        var consoleRect = _console.GetComponent<RectTransform>();
        consoleRect.offsetMax = Vector2.zero;
        consoleRect.offsetMin = Vector2.zero;
        EventService.Invoke(new UiEvents.OnUiInteractionStarted {InputsType = InputsType, CameraConfigPreset = null});
        _gameInputs.Debug.ConfirmConsoleCommand.performed += RegisterConsoleInput;
        _gameInputs.Debug.CallDebugConsole.performed += HideDebugConsole;
    }

    private void RegisterConsoleInput(InputAction.CallbackContext _)
    {
        var input = _console.consoleInput.text;
        ConsoleCommandsHandler.Execute(input);
        _console.consoleInput.text = "";
    }

    private void HideDebugConsole(InputAction.CallbackContext _)
    {
        _gameInputs.Debug.ConfirmConsoleCommand.performed -= RegisterConsoleInput;
        _gameInputs.Debug.CallDebugConsole.performed -= HideDebugConsole;
        SpawnManager.Despawn(_console.gameObject);
        _console = null;
        EventService.Invoke(new UiEvents.OnUiInteractionEnded());
        _gameInputs.Debug.CallDebugConsole.performed += CallDebugConsole;
    }

    public void Deinitialize()
    {
        _gameInputs.Debug.CallDebugConsole.performed -= CallDebugConsole;
        _gameInputs.Debug.CallDebugConsole.performed -= HideDebugConsole;
        _gameInputs.Debug.ConfirmConsoleCommand.performed -= RegisterConsoleInput;
        _gameInputs.Debug.Disable();
    }

}