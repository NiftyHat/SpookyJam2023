using Godot;
using SpookyBotanyGame.Core;
using System.Collections.Generic;

namespace SpookyBotanyGame.UI.Screens.Settings;

public class SettingsSection
{
    public Dictionary<string, VariantBinding> Items { get; protected set; }
    
    public void Load(Godot.Collections.Dictionary<string, Variant> loadedValues)
    {
        foreach (var kvp in loadedValues)
        {
            if (Items.TryGetValue(kvp.Key, out var item))
            {
                item.Set(kvp.Value);
            }
            else
            {
                Items[kvp.Key] = new VariantBinding(kvp.Value);
            }
        }
    }
}