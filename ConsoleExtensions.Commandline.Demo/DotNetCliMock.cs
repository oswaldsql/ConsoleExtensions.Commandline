// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DotNetCliMock.cs" company="Lasse Sjørup">
//   Copyright (c) 2019 Lasse Sjørup
//   Licensed under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

// ReSharper disable StyleCop.SA1600
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Global
// ReSharper disable StyleCop.SA1201
// ReSharper disable StyleCop.SA1602
namespace ConsoleExtensions.Commandline.Demo
{
    using System.ComponentModel;

    [Description("MOCK .NET Command Line Tools")]
    public class DotNetCliMock
    {
        [Description("Build the project before packing.")]
        public bool BuildFlag { get; set; }

        [Description("The configuration to use for building the project. The default for most projects is 'Debug'.")]
        public string Configuration { get; set; }

        [Description("Do not build project-to-project references and only build the specified project.")]
        public bool Dependencies { get; set; }

        [Description(
            " Force all dependencies to be resolved even if the last restore was successful. /n This is equivalent to deleting project.assets.json.")]
        public bool Force { get; set; }

        [Description(
            "The target framework to build for. The target framework must also be specified in the project file.")]
        public FrameworkEnum Framework { get; set; }

        [Description(
            " Include PDBs and source files. Source files go into the 'src' folder in the resulting nuget package.")]
        public bool IncludeSource { get; set; }

        [Description("Include packages with symbols in addition to regular packages in output directory.")]
        public bool IncludeSymbols { get; set; }

        [Description("Use incremental building")]
        public bool Incremental { get; set; }

        [Description("The output directory to place built artifacts in.")]
        public string Output { get; set; }

        [Description("Do not restore the project before building.")]
        public bool Restore { get; set; }

        [Description("The target runtime to build for.")]
        public RuntimeEnum Runtime { get; set; }

        [Description(
            "Set the serviceable flag in the package. See https://aka.ms/nupkgservicing for more information.")]
        public bool Serviceable { get; set; }

        [Description(
            " Set the MSBuild verbosity level. Allowed values are q[uiet], m[inimal], n[ormal], d[etailed], and diag[nostic]")]
        public VerbosityEnum Verbosity { get; set; }

        [Description("Set the value of the $(VersionSuffix) property to use when building the project.")]
        public string VersionSuffix { get; set; }

        [Description("Build a .NET project.")]
        public string Build([Description("The project file to operate on. If a file is not specified, the command will search the current directory for one")]string project = "", bool debug = false, int fds = 43)
        {
            return "Mocking a build";
        }

        [Description("Create a NuGet package.")]
        public string Pack(string project = "")
        {
            return "Mocking a pack";
        }
    }

    public enum VerbosityEnum
    {
        Quiet,

        Minimal,

        Normal,

        Detailed,

        Diagnostic
    }

    public enum RuntimeEnum
    {
        Runtime1,

        Runtime2,

        Runtime3,

        Runtime4,

        Runtime5
    }

    public enum FrameworkEnum
    {
        Framework1,

        Framework2,

        Framework3,

        Framework4,

        Framework5
    }
}