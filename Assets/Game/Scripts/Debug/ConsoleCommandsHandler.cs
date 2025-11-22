using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public static class ConsoleCommandsHandler
{
    private static readonly Dictionary<string, MethodInfo> Commands = new();

    public static void RegisterConsoleCommands()
    {
        foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
        {
            foreach (var method in type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
            {
                var attr = method.GetCustomAttribute<ConsoleCommandAttribute>();
                if (attr != null) Commands[attr.Name.ToLower()] = method;
            }
        }
    }

    public static void Execute(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return;
        var parts = input.Split(' ');
        var commandName = parts[0].ToLower();
        var args = parts.Length > 1 ? parts[1..] : Array.Empty<string>();
        if (!Commands.TryGetValue(commandName, out var method)) return;
        Invoke(method, args);
    }

    private static void Invoke(MethodInfo method, string[] args)
    {
        var parameters = method.GetParameters();
        if (parameters.Length != args.Length) return;
        var converted = new object[args.Length];
        for (var i = 0; i < args.Length; i++)
        {
            try { converted[i] = Convert.ChangeType(args[i], parameters[i].ParameterType); }
            catch
            {
                Debug.LogError($"Argument {i + 1} ('{args[i]}') cannot be converted to {parameters[i].ParameterType.Name}");
                return;
            }
        }
        method.Invoke(null, converted);
    }
}
