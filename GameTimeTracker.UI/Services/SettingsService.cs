using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using GameTimeTracker.Data.Models;

namespace GameTimeTracker.UI.Services;

public class SettingsService
{
    private readonly string _settingsPath;
    private UserSettings _currentSettings;

    public SettingsService()
    {
        var appDataPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "GameTimeTracker");
        
        Directory.CreateDirectory(appDataPath);
        _settingsPath = Path.Combine(appDataPath, "settings.json");
        
        _currentSettings = LoadSettings();
    }

    public UserSettings GetSettings()
    {
        return _currentSettings;
    }

    public void SaveSettings(UserSettings settings)
    {
        _currentSettings = settings;
        var json = JsonSerializer.Serialize(settings, new JsonSerializerOptions 
        { 
            WriteIndented = true 
        });
        File.WriteAllText(_settingsPath, json);
    }

    private UserSettings LoadSettings()
    {
        if (!File.Exists(_settingsPath))
        {
            var defaultSettings = new UserSettings();
            SaveSettings(defaultSettings);
            return defaultSettings;
        }

        try
        {
            var json = File.ReadAllText(_settingsPath);
            return JsonSerializer.Deserialize<UserSettings>(json) ?? new UserSettings();
        }
        catch
        {
            return new UserSettings();
        }
    }
}
