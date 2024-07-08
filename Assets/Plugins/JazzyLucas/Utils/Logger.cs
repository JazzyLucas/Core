using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace JazzyLucas.Utils
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
            {Color.green, "blue"},
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
            DoLog(senders[sender], sender.name, message, sender);
        }
        private static void DoLog(Color prefixColor, string prefix, string message, Object sender)
        {
            Debug.Log($"<color={_colors[_loggerSymbolColor]}>{LOGGER_SYMBOL}</color> <color={_colors[prefixColor]}>{prefix}:</color> <color={_colors[_messageColor]}>{message}</color>", sender);
        }
        public static void Log(string message)
        {
            Debug.Log($"<color={_colors[_loggerSymbolColor]}>{LOGGER_SYMBOL}</color> <color={_colors[Color.white]}>:</color> <color={_colors[_messageColor]}>{message}</color>");
        }
    }
}
