﻿global using BenchmarkDotNet.Attributes;
global using BenchmarkDotNet.Columns;
global using BenchmarkDotNet.Configs;
global using BenchmarkDotNet.Diagnosers;
global using Benchmarks.Core.Database;
global using Benchmarks.Core.Database.Postgres;
global using Benchmarks.Core.Database.SqlServer;
global using Benchmarks.Core.Entities;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.Design;
global using Microsoft.EntityFrameworkCore.Diagnostics;
global using System.ComponentModel;
global using System.ComponentModel.DataAnnotations;
global using Testcontainers.MsSql;
global using Testcontainers.PostgreSql;