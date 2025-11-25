# Term Tracker

Term Tracker is a .NET MAUI application for keeping academic timelines organized across mobile and desktop. It ships with a card-based UI, sample data, and a lightweight SQLite database so you can monitor terms, courses, assessments, and notes without extra setup.

## Features

- Maintain multiple academic terms with automatic active/future/completed status calculation.
- Add courses with instructor contact details and keep them grouped under their respective term.
- Track objective and performance assessments per course, including due dates and progress indicators.
- Capture quick or long-form course notes that surface directly inside the course view.
- Persist everything locally via `sqlite-net-pcl`, with seed data added on first launch to explore the experience.
- Target Android and Windows out of the box; additional MAUI platforms can be enabled with minimal changes.

## Tech Stack

- .NET 8 and .NET MAUI single-project architecture
- CommunityToolkit.Maui UI helpers
- SQLite (via `sqlite-net-pcl`) for local storage
- MVVM-friendly view components with XAML and C#

## Getting Started

### Prerequisites

- .NET SDK 8.0 or later (`dotnet --version`)
- .NET MAUI workload (`dotnet workload install maui`)
- Visual Studio 2022 17.8+ with the .NET MAUI workload **or** the `dotnet` CLI with appropriate platform emulators/runtimes installed

### Clone and Run

```bash
git clone https://github.com/<your-account>/term-tracker.git
cd term-tracker
```

Build and launch on Android (default emulator or connected device):

```bash
dotnet build TermTracker.sln -t:Run -f net8.0-android
```

Build and launch on Windows (from an elevated Developer Command Prompt or PowerShell):

```bash
dotnet build TermTracker.sln -t:Run -f net8.0-windows10.0.19041.0
```

> Tip: Visual Studio users can right-click `TermTracker` in Solution Explorer and choose **Set as Startup Project**, then press `F5`.

## Project Structure

- `TermTracker/Models` – Data models for terms, courses, assessments, and notes plus supporting enums.
- `TermTracker/Services/DatabaseService.cs` – SQLite initialization, seed data, and CRUD helpers.
- `TermTracker/Views` – MAUI XAML pages, popups, and reusable card components.
- `TermTracker/Resources` – App icons, fonts, images, colors, and styles.
- `TermTracker/MauiProgram.cs` – App bootstrap and dependency registration.

## Data and Persistence

On first run the app creates `TermTracker.db` in the MAUI application data directory and inserts demo terms, courses, assessments, and notes. Subsequent launches reuse the same database so your changes persist locally.

