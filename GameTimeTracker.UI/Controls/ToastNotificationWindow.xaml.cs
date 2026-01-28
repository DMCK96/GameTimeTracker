using System;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using Wpf.Ui.Controls;

namespace GameTimeTracker.UI.Controls;

public partial class ToastNotificationWindow : Window
{
    private readonly DispatcherTimer _autoCloseTimer;
    private readonly double _screenOffsetX;
    private readonly double _screenStartX;

    public ToastNotificationWindow(string title, string message, SymbolRegular icon = SymbolRegular.Info24, int autoCloseDurationSeconds = 5)
    {
        InitializeComponent();
        
        TitleText.Text = title;
        MessageText.Text = message;
        IconElement.SetCurrentValue(SymbolIcon.SymbolProperty, icon);
        
        // Calculate position (bottom-right corner with margin)
        var workArea = SystemParameters.WorkArea;
        _screenOffsetX = workArea.Right - Width - 20;
        _screenStartX = workArea.Right + 20; // Start off-screen
        
        Left = _screenStartX;
        Top = workArea.Bottom - Height - 20;
        
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

