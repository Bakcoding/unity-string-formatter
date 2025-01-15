# Unity-StringFormatter Library
Simple but powerful string formatting utility library for Unity. Provides easy-to-use methods for formatting numbers, data sizes, time durations, and percentages.

## Features

### Comma
- Format numbers with thousands separators (comma)
- Culture-aware formatting
- Supports multiple numeric types (int, long, float, double, decimal)

```csharp
int number = 1234567;
Debug.Log(number.ToCommaString());  // "1,234,567"
```

### Compact
- Convert large numbers to compact form with units (K, M, B, T, Q)
- Customizable decimal places and units
- Support for uppercase/lowercase units

```csharp
long value = 1234567;
Debug.Log(value.ToCompactString());  // "1.2M"
Debug.Log(value.ToCompactString(upperCase: false));  // "1.2m"
```

### Data Size
- Convert byte sizes to human-readable format
- Supports B, KB, MB, GB units
- Optional detailed format showing original bytes

```csharp
long bytes = 1536000;
Debug.Log(bytes.ToFileSize());  // "1.5MB"
Debug.Log(bytes.ToDetailedFileSize());  // "1.5MB (1,536,000 bytes)"
```

### Time
- Convert seconds to time strings
- Multiple format options (with/without days)
- Supports both separator format (HH:MM:SS) and label format (1h 30m 45s)

```csharp
float seconds = 3665f;
Debug.Log(seconds.ToTimeString());  // "01:01:05"
Debug.Log(seconds.ToTimeStringWithLabels());  // "01h01m05s"
```

### Percent
- Convert numbers to percentage strings
- Support for ratio calculations
- Customizable decimal places and unit symbol

```csharp
float value = 0.756f;
Debug.Log(value.ToPercentString());  // "75.6%"

int current = 75, total = 100;
Debug.Log(current.ToPercentStringWithTotal(total));  // "75.0%"
```

### Usage Examples
```csharp
using DinoLabs;

// Number formatting
Debug.Log(1234567.ToCommaString());  // "1,234,567"
Debug.Log(1234567.ToCompactString());  // "1.2M"

// Data size
Debug.Log((2 * 1024 * 1024).ToFileSize());  // "2.0MB"

// Time duration
Debug.Log(3665f.ToTimeString());  // "01:01:05"

// Percentage
Debug.Log(0.756f.ToPercentString());  // "75.6%"
```

## Installation
1. Copy the StringFormatter.cs file to your Unity project's Scripts folder
2. Add using DinoLabs; to your scripts