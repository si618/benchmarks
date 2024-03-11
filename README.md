# 🧪 Benchmarks

[![build](https://github.com/si618/benchmarks/actions/workflows/build.yml/badge.svg)](https://github.com/si618/benchmarks/actions/workflows/build.yml)
[![License](https://img.shields.io/badge/license-Apache_2.0-blue.svg)](LICENSE)

## 🏗 Build️

```bash
> dotnet --list-sdks
8.0.201 [/usr/share/dotnet/sdk]

> git --version
git version 2.43.0

> git clone https://github.com/si618/benchmarks.git
Cloning into 'benchmarks'...

> cd benchmarks
> dotnet build
```

## 🚀 Run

Includes an interactive console application to run benchmarks and display information.

```bash
> dotnet run --project .\Benchmarks.App
USAGE:
    Benchmark.exe [OPTIONS] <COMMAND>

EXAMPLES:
    Benchmark.exe benchmark GuidPrimaryKey
    Benchmark.exe info GuidPrimaryKey

OPTIONS:
    -h, --help       Prints help information
    -v, --version    Prints version information

COMMANDS:
    app          Run interactive console application
    benchmark    Run benchmarks
    info         Show benchmark details
    list         List benchmarks
    workflow     Run benchmarks for a GitHub workflow
```
