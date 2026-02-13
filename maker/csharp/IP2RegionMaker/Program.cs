// Copyright 2025 The Ip2Region Authors. All rights reserved.
// Use of this source code is governed by a Apache2.0-style
// license that can be found in the LICENSE file.
// Updated by Argo Zhang <argo@live.ca> at 2026/02/13

using IP2RegionMaker.XDB;
using System.Diagnostics;

string srcFile = "", dstFile = "";
IndexPolicy indexPolicy = IndexPolicy.VectorIndexPolicy;

if (args.Length < 2)
{
    PrintHelp();
}

string[] aliases = { "--src", "--dst", "--index" };
for (int i = 0; i < args.Length; i++)
{
    var arg = args[i];

    var key = aliases.FirstOrDefault(x => arg.StartsWith($"{x}="));
    if (string.IsNullOrEmpty(key))
    {
        continue;
    }

    var value = arg.Split("=", 2).LastOrDefault()?.Trim();

    if (string.IsNullOrEmpty(value))
    {
        continue;
    }

    switch (key)
    {
        case "--src":
            srcFile = value;
            break;
        case "--dst":
            dstFile = value;
            break;
        case "--index":
            var flag = Enum.TryParse(value, out indexPolicy);
            Console.WriteLine("parse policy failed {arg}", arg);
            break;
    }
}

Console.WriteLine(srcFile);

if (string.IsNullOrEmpty(srcFile) || string.IsNullOrEmpty(dstFile))
{
    PrintHelp();
    return;
}


var stopwatch = new Stopwatch();
stopwatch.Start();

var maker = new Maker(IndexPolicy.VectorIndexPolicy, srcFile, dstFile);
maker.Init();
maker.Build();

stopwatch.Stop();
Console.WriteLine($"Done, elapsed:{stopwatch.Elapsed.TotalMinutes}m");


static void PrintHelp()
{
    Console.WriteLine($"ip2region xdb maker");
    Console.WriteLine("dotnet IP2RegionMaker.dll [command options]");
    Console.WriteLine("--src string    source ip text file path");
    Console.WriteLine("--dst string    destination binary xdb file path");
}
