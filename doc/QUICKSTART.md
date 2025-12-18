# ? Quick Start Guide

Get up and running with AspireShowcase in 5 minutes!

## ?? Prerequisites Checklist

Before you start, make sure you have:

- ? Windows 11
- ? Visual Studio 2022 (latest version)
- ? 8GB+ RAM recommended
- ? 10GB+ free disk space

## ?? Installation Steps

### 1?? Install Node.js (2 minutes)

```powershell
# Download and install nvm-windows from:
# https://github.com/coreybutler/nvm-windows/releases

# After installation, restart terminal and run:
nvm install lts
nvm use lts
```

### 2?? Install Podman (3 minutes)

```powershell
# Download and install from:
# https://github.com/containers/podman/releases

# After installation, restart computer and run:
podman machine init --user-mode-networking
podman machine start
```

### 3?? Install .NET Aspire Workload (1 minute)

```powershell
dotnet workload install aspire
```

## ?? Run the Application

1. Open `NetChapterAspire.sln` in Visual Studio
2. Set `NetChapterAspire.AppHost` as startup project
3. Press **F5**

That's it! ??

## ?? Access Points

- **Dashboard**: http://localhost:15000
- **Application**: https://localhost:5173
- **API**: https://localhost:7000
- **MailPit**: http://localhost:1080

## ?? Next Steps

- Read the full [README.md](../README.md)
- Explore [ARCHITECTURE_GUIDE.md](ARCHITECTURE_GUIDE.md)
- Check [Aspire Documentation](https://aspire.dev/docs/)

## ?? Help?

See [Troubleshooting](../README.md#-troubleshooting) section in README.
