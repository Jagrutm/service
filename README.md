# Microservice Applications

Welcome to the Banking Project Microservice application repository. This repository contains the server side application source code for the banking related operation.

## Architecture Overview

## Technology Stack Overview

## Microservices Overview

| Name | Description| Readme File |
|--|--|--|
| Agency Service | Service to manage agencies| [AgencyService](.\services\AgencyService\README.md) |
| Account Process Service | Service to manage account processes| [AccountProcessService](.\services\AccountProcessService\README.md) |
| Account Service | Service to manage accounts| [AccountService](.\services\AccountService\README.md) |
| FPS Process Service | Service to manage FPS payment process| [FPSProcessService](.\services\FPSProcessService\README.md) |

## Microservice Project Structure

Microservice Project Structure built using Onion architecture. To get overview of different layers in microservice project refer [Microservice Project Structure]

## Development

### Prerequisites

Following tools should be installed in your system.

+ [Git](https://gitforwindows.org/)
+ [.NET 6.0.x SDK](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
+ VS Code or Visual Studio 2022 (Recommended)
+ [AWS CLI](https://docs.aws.amazon.com/cli/latest/userguide/install-cliv2-windows.html)
+ [Docker Desktop 64bit For Windows](https://docs.docker.com/docker-for-windows/install/) Note: Use Linux containers while installing docker desktop on windows
+ MSSQL server

### Getting Started

BatchJobs project is an command line application with the capabilities to define and execute different commands using command line interface.

To implement command line application, we are using [CommandLineParser](https://github.com/commandlineparser/commandline) library.

#### How to add new command capability in BatchJobs?

+ Create new command class (inherit `ICommand`) with required command arguments property. Below is the example of echo command class
```cs
[Verb("echo", HelpText = "Echo command"]
public class EchoCommand : ICommand
{
    [Options("m", "message", Required = false, HelpText = "Echo message"]
    public string Message { get; set; } = "Agency service batch jobs called";
    public Task ExecuteAsync(IServiceProvider serviceProvider)
    {
        return Task.CompletedTask;
    }
}
```

+ Add above command to ParseArguments collection in Program.cs

```cs
public static class Program
{
	public static async Task Main(string[] args)
	{
       var parsedResult = Parser.Default.ParseArguments<EchoCommand>(args);
    }
}
```

#### How to execute BatchJob command?

+ Go to BatchJob project folder and open command line interface (e.g. `services/<ServiceName>/src/<ServiceName>.BatchJobs/`)
+ Execute `dotnet run <CommandName> [CommandArguments]`
	+ e.g. `dotnet run echo --message "Agency service echo command"`

#### How to create docker image for BatchJob?

+ Execute `docker run -f build-batchjobs.docker --Target <TargetToRun> --ServiceName <ServiceName>`
    + e.g `docker run -f build-batchjobs.docker --Target DockerPush --ServiceName AgencyService`
	
#### How to execute BatchJob command using docker container?

+ Execute `docker run <ImageName> <BatchJob Command> <Command Arguments>`
    + e.g. `docker run services/agencyserviceapi echo --message "Agency service echo command"`

## Coding and Development Guidelines
