using System.Reflection;
using System.Runtime.InteropServices;

[assembly: ComVisible(false)]
[assembly: Guid("93ced893-841d-4384-be7b-cd3c4b7b1b2e")]
[assembly: System.CLSCompliant(true)]

#if NETCOREAPP
[assembly: AssemblyMetadata("ProjectUrl", "https://github.com/DKorablin/Plugin.MsSqlUtils")]
#else

[assembly: AssemblyTitle("Plugin.MsSqlUtils")]
[assembly: AssemblyDescription("Microsoft © SQL Server Tools")]
#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif
[assembly: AssemblyCompany("Danila Korablin")]
[assembly: AssemblyProduct("Plugin.MsSqlUtils")]
[assembly: AssemblyCopyright("Copyright © Danila Korablin 2011-2024")]
#endif
