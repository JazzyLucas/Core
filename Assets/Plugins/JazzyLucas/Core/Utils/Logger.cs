using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEditor.PackageManager;
using UnityEditor.VersionControl;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace JazzyLucas.Core.Utils
{
    public abstract class Logger
    {
        private const string LOGGER_SYMBOL = "@";
        private static readonly Color _loggerSymbolColor = Color.black;
        private static readonly Color _messageColor = Color.white;
        private static readonly Dictionary<Color, string> _colors = new()
        {
            {Color.red, "red"},
            {Color.yellow, "yellow"},
            {Color.green, "green"},
            //{Color.cyan, "cyan"}, // for some reason cyan and magenta logging is buggy
            {Color.blue, "blue"},
            //{Color.magenta, "magenta"},
            {Color.black, "black"},
            {Color.grey, "grey"},
            {Color.white, "white"},
        };

        private static KeyValuePair<Color, string> GetRandomColorEntry()
        {
            int randomIndex = Random.Range(0, _colors.Count);
            var randomEntry = _colors.ElementAt(randomIndex);
            return randomEntry;
        }

        private static Dictionary<Object, Color> _senders;
        private static Dictionary<Object, Color> senders => _senders ??= new();

        public void ConfigureLog(Object sender, Color newColor)
        {
            if (!_colors.ContainsKey(newColor))
                return;
            senders.TryAdd(sender, newColor);
            senders[sender] = newColor;
        }
        
        public static void Log(Object sender, string message, Color prefixColor = default)
        {
            if (prefixColor == default)
                prefixColor = GetRandomColorEntry().Key;
            senders.TryAdd(sender, prefixColor);
            DoComplexLog(senders[sender], sender.name, message, sender);
        }
        public static void Log(LogSeverity severity, string message)
        {
            var logMessage = $"<color={_colors[_loggerSymbolColor]}>{LOGGER_SYMBOL}</color> " +
                             $"<color={_colors[Color.white]}>:</color> " +
                             $"<color={_colors[_messageColor]}>{message}</color>";
            DoSimpleLog(logMessage, severity);
        }
        public static void Log(string message)
        {
            var logMessage = $"<color={_colors[_loggerSymbolColor]}>{LOGGER_SYMBOL}</color> " +
                             $"<color={_colors[Color.white]}>:</color> " +
                             $"<color={_colors[_messageColor]}>{message}</color>";
            DoSimpleLog(logMessage);
        }
        
        private static void DoSimpleLog(string message, LogSeverity severity = LogSeverity.INFO)
        {
            switch (severity)
            {
                case LogSeverity.WARNING:
                    Debug.LogWarning(message);
                    break;
                case LogSeverity.ERROR:
                    Debug.LogError(message);
                    break;
                case LogSeverity.INFO:
                default:
                    Debug.Log(message);
                    break;
            }
        }
        private static void DoComplexLog(Color prefixColor, string prefix, string message, Object sender)
        {
            Debug.Log($"<color={_colors[_loggerSymbolColor]}>{LOGGER_SYMBOL}</color> <color={_colors[prefixColor]}>{prefix}:</color> <color={_colors[_messageColor]}>{message}</color>", sender);
        }
    }

    public enum LogSeverity
    {
        INFO,
        WARNING,
        ERROR,
    }
}
