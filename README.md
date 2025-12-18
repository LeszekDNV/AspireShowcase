# 🚀 AspireShowcase

> A comprehensive .NET Aspire showcase application demonstrating modern cloud-ready architecture patterns, orchestration capabilities, and best practices for building distributed applications.

[![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![Aspire](https://img.shields.io/badge/Aspire-13.1.0-512BD4)](https://aspire.dev/)
[![React](https://img.shields.io/badge/React-19.2.0-61DAFB?logo=react)](https://react.dev/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)

---

## 📋 Table of Contents

- [About](#-about)
- [Features](#-features)
- [Prerequisites](#-prerequisites)
- [Installation](#-installation)
  - [Node Version Manager (nvm)](#1-node-version-manager-nvm)
  - [Podman](#2-podman-container-engine)
  - [.NET Aspire Workload](#3-net-aspire-workload)
- [Configuration](#-configuration)
- [Running the Application](#-running-the-application)
- [Architecture](#-architecture)
- [Learn More](#-learn-more)
- [Troubleshooting](#-troubleshooting)

---

## 🎯 About

**AspireShowcase** is a production-ready demonstration application built with **.NET Aspire**, showcasing how to:

- 🏗️ **Build distributed applications** with ease using Aspire's orchestration
- ☁️ **Integrate cloud services** (Azure Storage, Service Bus, SQL Server)
- 📧 **Implement email services** with MailPit for local development
- 💾 **Work with databases** using Entity Framework Core
- 🎨 **Create modern frontends** with React and Vite
- 🔄 **Apply clean architecture** principles with SOLID design patterns

This project serves as a **reference implementation** for developers learning .NET Aspire and demonstrates real-world patterns for building cloud-native applications.

---

## ✨ Features

### Backend (.NET 10)
- ✅ **Azure Blob Storage** - File upload and management
- ✅ **Azure Service Bus** - Message queue demonstration
- ✅ **SQL Server** - Entity Framework Core with automatic migrations
- ✅ **Email Service** - MailPit integration for development
- ✅ **Clean Architecture** - Service layer with dependency injection
- ✅ **Global Error Handling** - Centralized exception management
- ✅ **API Response Wrapper** - Consistent response structure

### Frontend (React + TypeScript)
- ✅ **Modern UI** - Clean and responsive design
- ✅ **Type Safety** - Full TypeScript support
- ✅ **React Router** - Multi-page navigation
- ✅ **Vite** - Fast build and hot module replacement

### DevOps & Infrastructure
- ✅ **.NET Aspire** - Container orchestration
- ✅ **Podman** - Lightweight container runtime
- ✅ **Local Emulators** - Azure Storage, Service Bus, SQL Server
- ✅ **Hot Reload** - Fast development cycle

---

## 📦 Prerequisites

Before you begin, ensure you have the following installed on your Windows 11 machine:

### Required Software

| Software | Version | Purpose |
|----------|---------|---------|
| **Visual Studio 2022** | Latest (17.13+) | IDE with .NET 10 support |
| **Node.js** | Latest LTS | JavaScript runtime for frontend |
| **npm** | Latest | Package manager for Node.js |
| **Podman** | Latest | Container engine |
| **.NET SDK** | 10.0+ | .NET runtime and SDK |

> ⚠️ **Important**: Make sure your Visual Studio 2022 is updated to the latest version to ensure .NET 10 and Aspire support.

---

## 🛠️ Installation

Follow these steps to set up your development environment:

### 1. Node Version Manager (nvm)

**nvm** allows you to manage multiple Node.js versions on your machine.

#### Install nvm for Windows

1. **Download** the latest installer from [nvm-windows releases](https://github.com/coreybutler/nvm-windows/releases)
2. Run `nvm-setup.exe` and follow the installation wizard
3. **Restart your terminal** (PowerShell or Command Prompt)

#### Install Node.js LTS

```powershell
# List available Node.js versions
nvm list available

# Install the latest LTS version (e.g., 20.x.x)
nvm install lts

# Use the installed version
nvm use lts

# Verify installation
node --version
npm --version
```

> 💡 **Tip**: LTS (Long Term Support) versions are recommended for production applications.

---

### 2. Podman Container Engine

**Podman** is a lightweight, daemonless container engine compatible with Docker.

#### Install Podman

1. **Download** the latest Windows installer from [Podman releases](https://github.com/containers/podman/releases)
2. Run `podman-setup.exe`
3. Follow the installation wizard
4. **Restart your computer** after installation

#### Initialize Podman Machine

```powershell
# Initialize Podman machine with user-mode networking
podman machine init --user-mode-networking

# Start the machine
podman machine start

# Verify installation
podman --version
podman ps
```

#### Configure User-Mode Networking (Windows 11)

User-mode networking is required for proper container networking on Windows 11:

```powershell
# Stop the machine if running
podman machine stop

# Remove existing machine (if any)
podman machine rm podman-machine-default

# Initialize with user-mode networking
podman machine init --user-mode-networking

# Start the machine
podman machine start

# Verify the configuration
podman machine inspect
```

> 📝 **Note**: User-mode networking eliminates the need for WSL2 and provides better compatibility with Windows networking.

#### Test Container Engine

```powershell
# Pull a test image
podman pull hello-world

# Run the test container
podman run hello-world
```

If you see "Hello from Docker!" (or Podman), your setup is working correctly! ✅

---

### 3. .NET Aspire Workload

**.NET Aspire** provides tools and templates for building cloud-native applications.

#### Install Aspire Workload

Open PowerShell as Administrator and run:

```powershell
# Install .NET Aspire workload
dotnet workload update
dotnet workload install aspire

# Verify installation
dotnet workload list
```

Expected output should include:
```
aspire    13.1.0+...
```

#### Install Aspire Dashboard (Optional)

The Aspire Dashboard provides a visual interface for monitoring your application:

```powershell
# Install globally
dotnet tool install -g aspire-dashboard

# Or update if already installed
dotnet tool update -g aspire-dashboard
```

---

### Additional Prerequisites (Automatically Handled by Aspire)

The following are handled automatically by .NET Aspire when you run the application:

- ✅ **Azure Storage Emulator (Azurite)** - For Blob Storage and Queues
- ✅ **SQL Server** - Microsoft SQL Server container
- ✅ **Azure Service Bus Emulator** - For message queuing
- ✅ **MailPit** - SMTP server for email testing

> 💡 **No manual setup required!** Aspire will pull and configure all necessary containers automatically.

---

## ⚙️ Configuration

### Environment Setup

The application uses Aspire's built-in service discovery and configuration. No manual configuration files are needed!

### Default Ports

| Service | Port | URL |
|---------|------|-----|
| **Aspire Dashboard** | 15000 | http://localhost:15000 |
| **Frontend (Vite)** | 5173 | https://localhost:5173 |
| **Backend API** | 7000 | https://localhost:7000 |
| **SQL Server** | 2137 | localhost:2137 |
| **MailPit Web UI** | 1080 | http://localhost:1080 |
| **MailPit SMTP** | 1025 | localhost:1025 |
| **Azure Storage (Blob)** | 10000 | http://localhost:10000 |
| **Azure Storage (Queue)** | 10001 | http://localhost:10001 |

> ℹ️ **Note**: Ports may vary if already in use. Check the Aspire Dashboard for actual port assignments.

---

## 🚀 Running the Application

### Option 1: Visual Studio (Recommended)

1. **Open** `NetChapterAspire.sln` in Visual Studio 2022
2. **Set** `NetChapterAspire.AppHost` as the startup project (right-click → Set as Startup Project)
3. Press **F5** or click **Start** to run

Visual Studio will:
- ✅ Build all projects
- ✅ Start Podman containers
- ✅ Launch the Aspire Dashboard
- ✅ Start the frontend and backend
- ✅ Open your browser automatically

### Option 2: Command Line

```powershell
# Navigate to the AppHost project
cd Aspire\NetChapterAspire.AppHost

# Run the application
dotnet run
```

### First Run

On the first run, Aspire will:
1. Download required container images (SQL Server, Azurite, etc.)
2. Initialize databases
3. Apply EF Core migrations
4. Start all services

This may take a few minutes. ⏱️

### Accessing the Application

Once started, you'll see:

```
✅ Dashboard: http://localhost:15000
✅ Frontend:  https://localhost:5173
✅ Backend:   https://localhost:7000
✅ MailPit:   http://localhost:1080
```

Click on the **Dashboard** link to see all running services and their logs.

---

## 🏗️ Architecture

This application follows **Clean Architecture** principles with clear separation of concerns.

### High-Level Architecture

```mermaid
graph TB
    subgraph "Client Layer"
        Browser[🌐 Browser]
        ReactApp[⚛️ React + TypeScript<br/>Vite Dev Server]
    end
    
    subgraph "API Layer - .NET 10"
        Controllers[🎮 Controllers<br/>HTTP Endpoints]
        Middleware[🛡️ Global Exception Handler]
    end
    
    subgraph "Business Layer"
        Services[⚙️ Services<br/>Business Logic]
        Interfaces[🔌 Interfaces<br/>Abstractions]
    end
    
    subgraph "Data Layer"
        EFCore[💾 EF Core<br/>DbContext]
        Entities[📦 Entities<br/>Domain Models]
    end
    
    subgraph "Infrastructure - Azure & External Services"
        AzureBlob[☁️ Azure Blob Storage<br/>Azurite Emulator]
        AzureServiceBus[📨 Azure Service Bus<br/>Local Emulator]
        SQLServer[🗄️ SQL Server<br/>Container]
        MailPit[📧 MailPit<br/>SMTP Server]
    end
    
    subgraph "Orchestration - .NET Aspire"
        AspireHost[🎯 Aspire AppHost<br/>Orchestration]
        Dashboard[📊 Aspire Dashboard<br/>Monitoring]
    end
    
    Browser --> ReactApp
    ReactApp -->|HTTPS| Controllers
    Controllers --> Middleware
    Middleware --> Controllers
    Controllers --> Interfaces
    Interfaces --> Services
    Services --> EFCore
    Services --> AzureBlob
    Services --> AzureServiceBus
    Services --> MailPit
    EFCore --> Entities
    EFCore --> SQLServer
    
    AspireHost -.->|Orchestrates| ReactApp
    AspireHost -.->|Orchestrates| Controllers
    AspireHost -.->|Manages| AzureBlob
    AspireHost -.->|Manages| AzureServiceBus
    AspireHost -.->|Manages| SQLServer
    AspireHost -.->|Manages| MailPit
    Dashboard -.->|Monitors| AspireHost
    
    style Browser fill:#e1f5ff
    style ReactApp fill:#61dafb
    style Controllers fill:#512bd4
    style Middleware fill:#ff6b6b
    style Services fill:#ffd93d
    style Interfaces fill:#a8dadc
    style EFCore fill:#6bcf7f
    style Entities fill:#95e1d3
    style AzureBlob fill:#0078d4
    style AzureServiceBus fill:#0078d4
    style SQLServer fill:#cc2927
    style MailPit fill:#ff6b35
    style AspireHost fill:#512bd4
    style Dashboard fill:#457b9d
```

### Layered Architecture Details

```mermaid
flowchart LR
    subgraph Presentation["🎨 Presentation Layer"]
        direction TB
        UI[React UI Components]
        Router[React Router]
        State[State Management]
    end
    
    subgraph API["🎮 API Layer"]
        direction TB
        CTRL[Controllers]
        VAL[Validation]
        RESP[ApiResponse Wrapper]
    end
    
    subgraph Business["⚙️ Business Layer"]
        direction TB
        SVC[Services]
        INT[Interfaces]
        DTO[DTOs]
    end
    
    subgraph Data["💾 Data Layer"]
        direction TB
        CTX[DbContext]
        ENT[Entities]
        MIG[Migrations]
    end
    
    subgraph External["☁️ External Services"]
        direction TB
        AZURE[Azure SDK]
        SMTP[SMTP Client]
    end
    
    Presentation -->|HTTP/HTTPS| API
    API -->|Dependency Injection| Business
    Business -->|Data Access| Data
    Business -->|Cloud Calls| External
    
    style Presentation fill:#e1f5ff
    style API fill:#fff3e0
    style Business fill:#f3e5f5
    style Data fill:#c8e6c9
    style External fill:#ffebee
```

### Request Flow Example

```mermaid
sequenceDiagram
    actor User as 👤 User
    participant UI as ⚛️ React App
    participant API as 🎮 Controller
    participant SVC as ⚙️ Service
    participant DB as 💾 Database
    participant Azure as ☁️ Azure
    
    User->>UI: Upload file
    UI->>API: POST /api/BlobStorage/upload
    Note over API: Validate request
    API->>SVC: UploadFileAsync()
    Note over SVC: Business logic
    SVC->>Azure: Upload to Blob Storage
    Azure-->>SVC: Upload successful
    SVC->>DB: Log operation
    DB-->>SVC: Success
    SVC-->>API: Return file name
    Note over API: Wrap in ApiResponse
    API-->>UI: 200 OK { success, data }
    UI-->>User: Show success message
```

### Technology Stack

```mermaid
mindmap
  root((AspireShowcase))
    Frontend
      React 19
      TypeScript
      Vite
      React Router
    Backend
      .NET 10
      ASP.NET Core
      Entity Framework Core
      C# 14
    Cloud Services
      Azure Blob Storage
      Azure Service Bus
      SQL Server
      MailPit SMTP
    DevOps
      .NET Aspire 13.1
      Podman Containers
      Docker Compose
      Hot Reload
    Architecture
      Clean Architecture
      SOLID Principles
      DI Container
      Repository Pattern
```

### Key Patterns

- 🎯 **SOLID Principles** - Single Responsibility, Dependency Inversion
- 🔄 **Repository Pattern** - Data access abstraction
- 💉 **Dependency Injection** - Loose coupling
- 🎭 **Service Layer** - Business logic encapsulation
- 🛡️ **Global Error Handling** - Centralized exception management
- 📦 **API Response Wrapper** - Consistent response structure

### Components Overview

| Layer | Components | Responsibility |
|-------|-----------|----------------|
| **Frontend** | React Components, Pages, Hooks | User interface and interaction |
| **API** | Controllers, Middleware | HTTP request handling, routing |
| **Business** | Services, Interfaces | Business logic, orchestration |
| **Data** | DbContext, Repositories | Data access, persistence |
| **Infrastructure** | Azure SDK, SMTP | External service integration |

### For detailed architecture documentation, see:
📖 **[ARCHITECTURE_GUIDE.md](doc/ARCHITECTURE_GUIDE.md)**

This comprehensive guide includes:
- 🏗️ **Detailed layer responsibilities** with code examples
- 📝 **How to add new endpoints** step-by-step
- 💉 **Dependency injection best practices** and lifecycle management
- 🧪 **Testing strategies** for unit and integration tests
- 🔄 **Code migration patterns** from old to new architecture
- 📊 **Visual diagrams** with Mermaid for all major flows
- 🎯 **SOLID principles** application examples

---

## 📚 Learn More

### Official Documentation

- 📘 **.NET Aspire Docs** - [https://aspire.dev/docs/](https://aspire.dev/docs/)
- 🎓 **Get Started with Aspire** - [https://aspire.dev/get-started/](https://aspire.dev/get-started/)
- 📖 **Aspire Integrations** - [https://aspire.dev/integrations/](https://aspire.dev/integrations/)
- 🔧 **Aspire CLI** - [https://aspire.dev/docs/fundamentals/cli/](https://aspire.dev/docs/fundamentals/cli/)

### Tutorials & Resources

- 🎥 **Microsoft Learn** - [Build cloud-native apps with .NET Aspire](https://learn.microsoft.com/en-us/training/paths/aspire/)
- 📺 **YouTube Channel** - [.NET Aspire Playlist](https://www.youtube.com/playlist?list=PLdo4fOcmZ0oULyHSPBx-tQzePOYlhvrAU)
- 💬 **Community** - [.NET Aspire GitHub Discussions](https://github.com/dotnet/aspire/discussions)

### Related Technologies

- [React Documentation](https://react.dev/)
- [Vite Documentation](https://vitejs.dev/)
- [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/)
- [Podman Documentation](https://podman.io/)

---

## 🔧 Troubleshooting

### Common Issues

#### 1. Podman Machine Won't Start

**Problem**: `podman machine start` fails

**Solution**:
```powershell
# Remove and reinitialize
podman machine stop
podman machine rm podman-machine-default
podman machine init --user-mode-networking
podman machine start
```

#### 2. Port Already in Use

**Problem**: Ports 7000, 5173, or others are already in use

**Solution**:
- Check the Aspire Dashboard for actual port assignments
- Or manually stop the conflicting process:
```powershell
# Find process using port 7000
netstat -ano | findstr :7000

# Kill the process (replace PID)
taskkill /PID <PID> /F
```

#### 3. SQL Server Connection Errors

**Problem**: Cannot connect to SQL Server

**Solution**:
- Ensure Podman is running: `podman ps`
- Check Aspire Dashboard logs
- Try restarting the AppHost project

#### 4. Frontend Not Loading

**Problem**: Vite dev server won't start

**Solution**:
```powershell
# Navigate to client folder
cd netchapteraspire.client

# Clear node_modules and reinstall
Remove-Item -Recurse -Force node_modules
npm install

# Try running manually
npm run dev
```

#### 5. Aspire Dashboard Not Opening

**Problem**: Dashboard URL returns error

**Solution**:
- Check if AppHost is running
- Look for the actual Dashboard URL in Visual Studio output window
- Try accessing http://localhost:15000 directly


**Built with ❤️ using .NET Aspire**
