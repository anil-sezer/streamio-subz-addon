<p align="center">
  <img src="Subz/wwwroot/logo.png" alt="Subz Logo" width="500"/>
</p>

# Stremio Subtitle Addon

## About Subz

Subz is a powerful subtitle addon for the Stremio streaming platform. 
It integrates with OpenSubtitles.org to provide a seamless subtitle experience for your movies and TV shows.

### Key Features

- **Multiple Language Support**: Search and download subtitles in multiple languages
- **Hearing Impaired Options**: Easily find subtitles with hearing impaired support
- **Custom Configuration**: Configure your subtitle preferences through a user-friendly interface
- **Discord Notifications**: Optional integration with Discord webhooks
- **Caching**: Efficient subtitle caching to improve performance

## Getting Started

1. Visit the configuration page to set up your OpenSubtitles.org credentials
2. Select your preferred languages
3. Set your hearing impaired preferences
4. Generate and install the addon to Stremio

## Technical Details

Subz is built with:
- Docker
- ASP.NET Core 9
- Blazor
- Integration with OpenSubtitles API
- Regular JavaScript & CSS styles for the frontend

## Installation

To install Subz to your Stremio application, visit the configuration page and copy the generated manifest URL to Stremio's addon section.

## Self Hosting

For local development:

```bash
# Clone the repository
git clone https://github.com/anil-sezer/streamio-subz-addon.git

# Navigate to the project directory
cd subz

# Build and run the application
dotnet run --project Subz
```

## Development

For local development:

```bash
# Clone the repository
git clone https://github.com/anil-sezer/streamio-subz-addon.git

# Navigate to the project directory
cd subz

# Build and run the application
dotnet run --project Subz
```