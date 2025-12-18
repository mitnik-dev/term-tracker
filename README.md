# Term Tracker

Term Tracker is android mobile app developed using .NET MAUI, for keeping academic timelines organized for student end-users.

## Features

- Maintain multiple academic terms with automatic active/future/completed status calculation.
- Add courses with instructor contact details and keep them grouped under their respective term.
- Track objective and performance assessments per course, including due dates and progress indicators.
- Capture quick or long-form course notes that surface directly inside the course view.
- Persist everything locally via `sqlite-net-pcl`, with seed data added on first launch to explore the experience.

## Tech Stack

- .NET 8 and .NET MAUI single-project architecture
- CommunityToolkit.Maui UI helpers
- SQLite (via `sqlite-net-pcl`) for local storage
- MVVM-friendly view components with XAML and C#


