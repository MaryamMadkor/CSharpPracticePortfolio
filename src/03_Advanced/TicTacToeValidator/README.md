# 🎮 Tic-Tac-Toe Win Checker

[![C#](https://img.shields.io/badge/C%23-12.0-blue)](https://dotnet.microsoft.com/)
[![.NET](https://img.shields.io/badge/.NET-8.0-purple)](https://dotnet.microsoft.com/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![JSON](https://img.shields.io/badge/Storage-JSON-green)](https://www.json.org/)

A professional-grade Tic-Tac-Toe validator with **game history persistence**, **JSON storage**, and **comprehensive unit tests**. Supports both standard (3x3) and Ultimate (9x9) game modes.

---

## ✨ Features

| Feature | Description |
|---------|-------------|
| **🎯 Win Detection** | Rows, columns, AND diagonals (8 win conditions) |
| **👾 Ultimate Mode** | 9x9 meta-board where each cell is a full game |
| **💾 JSON Storage** | All games saved to `GameHistory.json` with auto-incrementing IDs |
| **📜 Game History** | View and replay any past game |
| **🧪 Unit Tests** | 18+ test cases covering all win scenarios |
| **🏗️ Clean Architecture** | 5-layer separation of concerns |

---

## 🚀 Quick Start

```bash
# Clone the repository
git clone https://github.com/MaryamMadkor/CSharp-Practice-Portfolio.git

# Navigate to project
cd CSharp-Practice-Portfolio/03_Advanced/08_TicTacToe

# Run the application
dotnet run
```

## 🎮 How to Play
Standard Mode (3x3)
text
Enter board as 9 characters (row by row):
> x x x o - o x o x

Board:
 x | x | x
---+---+---
 o | - | o
---+---+---
 x | o | x

Result: X is the winner!
Ultimate Mode (9x9)
Each of the 9 cells represents a completed game. The meta-board shows who won each sub-game.

## 🧪 Test Coverage
csharp
✓ X Win (rows, columns, diagonals)
✓ O Win (rows, columns, diagonals)
✓ Draw detection
✓ Incomplete game detection

## 📚 What I Learned
- JSON Serialization with System.Text.Json
- Generic methods for reusable storage (SaveObject<T>, LoadObject<T>)
- Auto-incrementing IDs for entity tracking
- List<List<T>> vs 2D arrays for JSON compatibility
- AppSettings pattern for configurable file paths
- Comprehensive testing with 18+ test cases

## 📄 License
MIT License — Free to use, modify, and distribute with attribution.