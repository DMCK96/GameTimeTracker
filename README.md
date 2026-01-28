# GameTimeTracker

A modern .NET application for tracking game playtime, daily streaks, and gaming habits across all platforms.

## Features

- **Universal Game Tracking** - Monitors all games via process names (platform-agnostic)
- **Streak System** - Track daily streaks per game and overall gaming activity
- **Smart Notifications** - Windows toast notifications for streak reminders
- **Modern UI** - Windows 11-style interface with WPF UI framework
- **Theme Support** - Light/Dark/System themes with custom accent colors
- **Detailed Statistics** - View playtime metrics, session history, and best streaks

## Architecture

### Projects

1. **GameTimeTracker.Service** - Background Windows Service
   - Monitors running game processes
   - Records play sessions to database
   - Sends streak notifications

2. **GameTimeTracker.Data** - Shared Data Layer
   - Entity Framework Core with SQLite
   - Data models and repositories
   - Business logic for streak calculations

3. **GameTimeTracker.UI** - WPF Desktop Application
   - Modern Windows 11-style interface
   - Dashboard with gaming statistics
   - Settings and game management

## Technology Stack

- .NET 8.0
- WPF with WPF UI framework
- Entity Framework Core
- SQLite
- Microsoft.Toolkit.Uwp.Notifications

## Database Schema

### Games
- Tracks individual games by process name
- Stores display names, icons, total playtime
- Current and best streak records

### PlaySessions
- Individual gaming sessions with timestamps
- Links to Games table
- Used for detailed analytics and streak calculations

### UserSettings
- Minimum daily playtime threshold
- Notification preferences
- Theme settings

## Getting Started

### Prerequisites

- Visual Studio 2022
- .NET 8.0 SDK
- Windows 10/11

### Setup

Run the PowerShell setup script:

```powershell
.\setup.ps1
```

This will:
- Create the solution and project structure
- Install required NuGet packages
- Set up the folder hierarchy
- Initialize the database schema

### Manual Setup

If you prefer manual setup:

1. Open Visual Studio 2022
2. Create new solution "GameTimeTracker"
3. Add three projects as described above
4. Install NuGet packages (see setup script)
5. Build and run

## Configuration

### Minimum Daily Time
Configure in Settings page - determines when a day counts toward your streak (default: 30 minutes)

### Notifications
Set your preferred reminder time for daily streak checks

### Game Management
- Rename process names to friendly display names
- Set custom icons for games
- Hide or archive games you no longer track

## Roadmap

### Phase 1: Core Tracking (MVP)
- Background service for process monitoring
- SQLite database with EF Core
- Basic session recording

### Phase 2: GUI & Stats
- WPF dashboard with playtime statistics
- Streak calculations and display
- Settings page

### Phase 3: Notifications
- Toast notifications for streaks
- Configurable reminder system

### Future Enhancements
- Export data to CSV/JSON
- Weekly/monthly reports
- Goal setting features
- Steam/Epic API integration for game metadata
- Cloud sync for multi-PC setups

## License

MIT License - See LICENSE file for details

## Contributing

This is a personal project but suggestions and improvements are welcome.