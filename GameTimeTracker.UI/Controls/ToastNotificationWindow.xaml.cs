using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using GameTimeTracker.Data.Models;
using Wpf.Ui.Controls;

namespace GameTimeTracker.UI.Controls;

public partial class ToastNotificationWindow : Window
{
    private readonly DispatcherTimer _autoCloseTimer;
    private readonly double _screenOffsetX;
    private readonly double _screenStartX;
    private readonly NotificationLocation _location;
    private readonly Key _dismissKey;

    public ToastNotificationWindow(
        string title, 
        string message, 
        SymbolRegular icon = SymbolRegular.Info24, 
        int autoCloseDurationSeconds = 5,
        NotificationLocation location = NotificationLocation.BottomRight,
        string dismissHotkey = "Escape")
    {
        InitializeComponent();
        
        _location = location;
        
        // Parse dismiss hotkey
        if (Enum.TryParse<Key>(dismissHotkey, true, out var key))
        {
            _dismissKey = key;
        }
        else
        {
            _dismissKey = Key.Escape;
        }
        
        TitleText.Text = title;
        MessageText.Text = message;
        IconElement.SetCurrentValue(SymbolIcon.SymbolProperty, icon);
        
        // Calculate position based on location
        var workArea = SystemParameters.WorkArea;
        
        switch (location)
        {
            case NotificationLocation.TopLeft:
                _screenOffsetX = workArea.Left + 20;
                _screenStartX = workArea.Left - Width - 20;
                break;
            case NotificationLocation.TopRight:
                _screenOffsetX = workArea.Right - Width - 20;
                _screenStartX = workArea.Right + 20;
                break;
            case NotificationLocation.BottomLeft:
                _screenOffsetX = workArea.Left + 20;
                _screenStartX = workArea.Left - Width - 20;
                break;
            case NotificationLocation.BottomRight:
            default:
                _screenOffsetX = workArea.Right - Width - 20;
                _screenStartX = workArea.Right + 20;
                break;
        }
        
        Left = _screenStartX;
        Top = workArea.Bottom - Height - 20; // Initial, will be set by NotificationService
        
        // Setup auto-close timer if duration > 0
        if (autoCloseDurationSeconds > 0)
        {
            _autoCloseTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(autoCloseDurationSeconds)
            };
            _autoCloseTimer.Tick += (s, e) =>
            {
                _autoCloseTimer.Stop();
                CloseWithAnimation();
            };
        }
        
        // Add hotkey handler
        KeyDown += OnKeyDown;
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == _dismissKey)
        {
            _autoCloseTimer?.Stop();
            CloseWithAnimation();
            e.Handled = true;
        }
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        // Start slide-in animation
        var slideIn = (Storyboard)FindResource("SlideInAnimation");
        var leftAnimation = (DoubleAnimation)slideIn.Children[0];
        leftAnimation.From = _screenStartX;
        leftAnimation.To = _screenOffsetX;
        
        slideIn.Begin(this);
        
        // Start auto-close timer
        _autoCloseTimer?.Start();
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        _autoCloseTimer?.Stop();
        CloseWithAnimation();
    }

    private void CloseWithAnimation()
    {
        var slideOut = (Storyboard)FindResource("SlideOutAnimation");
        var leftAnimation = (DoubleAnimation)slideOut.Children[0];
        leftAnimation.From = _screenOffsetX;
        leftAnimation.To = _screenStartX;
        
        slideOut.Begin(this);
    }

    private void SlideOut_Completed(object sender, EventArgs e)
    {
        Close();
    }
}

