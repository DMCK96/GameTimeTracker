using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using GameTimeTracker.Data.Models;
using GameTimeTracker.UI.Services;
using Wpf.Ui.Controls;

namespace GameTimeTracker.UI.Pages;

public partial class SettingsPage : Page
{
    private readonly NotificationService _notificationService;
    private readonly SettingsService _settingsService;
    private UserSettings _currentSettings;

    public SettingsPage(NotificationService notificationService, SettingsService settingsService)
    {
        InitializeComponent();
        _notificationService = notificationService;
        _settingsService = settingsService;
        
        LoadSettings();
    }

    private void LoadSettings()
    {
        _currentSettings = _settingsService.GetSettings();
        
        // Load notification settings
        NotificationsEnabledToggle.IsChecked = _currentSettings.NotificationsEnabled;
        
        // Set location
        var locationItem = LocationComboBox.Items.Cast<ComboBoxItem>()
            .FirstOrDefault(item => item.Tag.ToString() == _currentSettings.NotificationLocation.ToString());
        if (locationItem != null)
            LocationComboBox.SelectedItem = locationItem;
        else
            LocationComboBox.SelectedIndex = 3; // Default to BottomRight
        
        // Set auto dismiss
        AutoDismissToggle.IsChecked = _currentSettings.AutoDismissNotifications;
        DurationSlider.Value = _currentSettings.AutoDismissDurationSeconds;
        UpdateAutoDismissVisibility();
        
        // Set hotkey
        var hotkeyItem = HotkeyComboBox.Items.Cast<ComboBoxItem>()
            .FirstOrDefault(item => item.Tag.ToString() == _currentSettings.DismissHotkey);
        if (hotkeyItem != null)
            HotkeyComboBox.SelectedItem = hotkeyItem;
        else
            HotkeyComboBox.SelectedIndex = 0; // Default to Escape
        
        // Set minimum game time
        MinGameTimeSlider.Value = _currentSettings.StreakMinimumGameTime.TotalMinutes;
        
        // Load reminder times
        LoadReminderTimes();
    }

    private void SaveSettings()
    {
        // Save notification enabled
        _currentSettings.NotificationsEnabled = NotificationsEnabledToggle.IsChecked ?? true;
        
        // Save location
        if (LocationComboBox.SelectedItem is ComboBoxItem locationItem)
        {
            _currentSettings.NotificationLocation = Enum.Parse<NotificationLocation>(locationItem.Tag.ToString());
        }
        
        // Save auto dismiss
        _currentSettings.AutoDismissNotifications = AutoDismissToggle.IsChecked ?? true;
        _currentSettings.AutoDismissDurationSeconds = (int)DurationSlider.Value;
        
        // Save hotkey
        if (HotkeyComboBox.SelectedItem is ComboBoxItem hotkeyItem)
        {
            _currentSettings.DismissHotkey = hotkeyItem.Tag.ToString();
        }
        
        // Save minimum game time
        _currentSettings.StreakMinimumGameTime = TimeSpan.FromMinutes(MinGameTimeSlider.Value);
        
        _settingsService.SaveSettings(_currentSettings);
    }

    private void LoadReminderTimes()
    {
        ReminderTimesPanel.Children.Clear();
        
        foreach (var time in _currentSettings.StreakReminderTimes)
        {
            AddReminderTimeControl(time);
        }
    }

    private void AddReminderTimeControl(TimeSpan? initialTime = null)
    {
        var panel = new Grid { Margin = new Thickness(0, 0, 0, 10) };
        panel.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(100) });
        panel.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        panel.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
        
        var hourBox = new Wpf.Ui.Controls.NumberBox
        {
            Minimum = 0,
            Maximum = 23,
            Value = initialTime?.Hours ?? 12,
            Margin = new Thickness(0, 0, 10, 0)
        };
        
        var minuteBox = new Wpf.Ui.Controls.NumberBox
        {
            Minimum = 0,
            Maximum = 59,
            Value = initialTime?.Minutes ?? 0,
            Margin = new Thickness(0, 0, 10, 0)
        };
        
        var stackPanel = new StackPanel { Orientation = Orientation.Horizontal };
        stackPanel.Children.Add(new System.Windows.Controls.TextBlock 
        { 
            Text = "Hour:", 
            VerticalAlignment = VerticalAlignment.Center, 
            Margin = new Thickness(0, 0, 5, 0) 
        });
        stackPanel.Children.Add(hourBox);
        stackPanel.Children.Add(new System.Windows.Controls.TextBlock 
        { 
            Text = "Minute:", 
            VerticalAlignment = VerticalAlignment.Center, 
            Margin = new Thickness(10, 0, 5, 0) 
        });
        stackPanel.Children.Add(minuteBox);
        
        var removeButton = new Wpf.Ui.Controls.Button
        {
            Icon = new SymbolIcon(SymbolRegular.Delete24),
            Appearance = ControlAppearance.Danger,
            Padding = new Thickness(8)
        };
        
        removeButton.Click += (s, e) =>
        {
            ReminderTimesPanel.Children.Remove(panel);
            UpdateReminderTimes();
        };
        
        hourBox.ValueChanged += (s, e) => UpdateReminderTimes();
        minuteBox.ValueChanged += (s, e) => UpdateReminderTimes();
        
        Grid.SetColumn(stackPanel, 0);
        Grid.SetColumnSpan(stackPanel, 2);
        Grid.SetColumn(removeButton, 2);
        
        panel.Children.Add(stackPanel);
        panel.Children.Add(removeButton);
        
        ReminderTimesPanel.Children.Add(panel);
    }

    private void UpdateReminderTimes()
    {
        _currentSettings.StreakReminderTimes.Clear();
        
        foreach (Grid panel in ReminderTimesPanel.Children)
        {
            var stackPanel = panel.Children.OfType<StackPanel>().FirstOrDefault();
            if (stackPanel == null) continue;
            
            var hourBox = stackPanel.Children.OfType<Wpf.Ui.Controls.NumberBox>().FirstOrDefault();
            var minuteBox = stackPanel.Children.OfType<Wpf.Ui.Controls.NumberBox>().Skip(1).FirstOrDefault();
            
            if (hourBox != null && minuteBox != null)
            {
                var time = new TimeSpan((int)hourBox.Value, (int)minuteBox.Value, 0);
                _currentSettings.StreakReminderTimes.Add(time);
            }
        }
        
        SaveSettings();
    }

    private void UpdateAutoDismissVisibility()
    {
        AutoDismissDurationPanel.Visibility = (AutoDismissToggle.IsChecked ?? false) 
            ? Visibility.Visible 
            : Visibility.Collapsed;
    }

    // Event Handlers
    private void NotificationsEnabledToggle_Changed(object sender, RoutedEventArgs e)
    {
        if (_currentSettings != null)
            SaveSettings();
    }

    private void LocationComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (_currentSettings != null)
            SaveSettings();
    }

    private void AutoDismissToggle_Changed(object sender, RoutedEventArgs e)
    {
        UpdateAutoDismissVisibility();
        if (_currentSettings != null)
            SaveSettings();
    }

    private void DurationSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        if (DurationValueText != null)
        {
            DurationValueText.Text = $"{(int)e.NewValue}s";
            if (_currentSettings != null)
                SaveSettings();
        }
    }

    private void HotkeyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (_currentSettings != null)
            SaveSettings();
    }

    private void MinGameTimeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        if (MinGameTimeValueText != null)
        {
            MinGameTimeValueText.Text = $"{(int)e.NewValue} min";
            if (_currentSettings != null)
                SaveSettings();
        }
    }

    private void AddReminderTime_Click(object sender, RoutedEventArgs e)
    {
        AddReminderTimeControl();
        UpdateReminderTimes();
    }

    // Test notification handlers
    private void TestStreakNotification_Click(object sender, RoutedEventArgs e)
    {
        _notificationService.SendStreakNotification("Elden Ring", 7);
    }

    private void TestReminderNotification_Click(object sender, RoutedEventArgs e)
    {
        _notificationService.SendDailyReminderNotification();
    }

    private void TestCustomNotification_Click(object sender, RoutedEventArgs e)
    {
        _notificationService.SendTestNotification(
            "?? GameTimeTracker",
            "This is a test notification! Everything is working correctly.");
    }
}


