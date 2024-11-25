# User Data Fetcher

A .NET 8.0 console application that fetches user data from multiple APIs and combines them into a single output file (JSON or CSV).

## Features

- Fetches user data from multiple sources:
  - Random User API (randomuser.me)
  - JSONPlaceholder (jsonplaceholder.typicode.com)
  - DummyJSON (dummyjson.com)
  - ReqRes API (reqres.in)
- Concurrent API calls for improved performance
- Supports JSON and CSV output formats
- Timestamp-based file naming
- Error handling for each API source

## Requirements

- .NET 8.0 SDK
- Internet connection for API access

## Usage

1. Clone the repository
2. Run the application
3. Enter the desired output folder path
4. Choose output format (JSON or CSV)
5. The application will fetch data and save it to the specified location

## Project Structure

```
UserDataFetcher/
├── Models/
│   └── User.cs
├── Interfaces/
│   ├── IUserDataSource.cs
│   └── IDataWriter.cs
├── Services/
│   ├── DataSources/
│   │   ├── RandomUserSource.cs
│   │   ├── JsonPlaceholderSource.cs
│   │   ├── DummyJsonSource.cs
│   │   └── ReqResSource.cs
│   └── DataWriters/
│       ├── JsonDataWriter.cs
│       └── CsvDataWriter.cs
└── Program.cs
```

## Adding New Data Sources

To add a new data source:
1. Create a new class in the DataSources folder
2. Implement the IUserDataSource interface
3. Add the new source to the dataSources list in Program.cs