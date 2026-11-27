# Veloform

A desktop app that generates cycling routes as GPX files, based on your preferences:
distance, total climb, surface type, and where along the ride the climbs happen.

Built with C# / .NET 10 and Avalonia, using OpenStreetMap data.

🚧 Work in progress

## Project structure

```
Veloform/
├── Veloform.Core/          # Routing engine: graph, algorithms, geo math, GPX. No UI.
├── Veloform.GraphBuilder/  # Console app: builds the routing graph from OSM + elevation data
├── Veloform.Desktop/       # Avalonia desktop app (UI). Uses Core.
├── Veloform.Tests/         # xUnit tests for Core
├── docs/                   # Design notes and data findings
├── data/                   # Local data files (OSM, elevation). Not in the repo
└── Veloform.sln
```





Map data © OpenStreetMap contributors, available under the [ODbL](https://www.openstreetmap.org/copyright).
