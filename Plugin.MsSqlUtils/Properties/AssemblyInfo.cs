using System.Reflection;
using System.Runtime.InteropServices;

[assembly: Guid("93ced893-841d-4384-be7b-cd3c4b7b1b2e")]
[assembly: System.CLSCompliant(true)]

#if NETCOREAPP
[assembly: AssemblyMetadata("ProjectUrl", "https://github.com/DKorablin/Plugin.MsSqlUtils")]
#else

[assembly: AssemblyDescription("Microsoft © SQL Server Tools")]
[assembly: AssemblyCopyright("Copyright © Danila Korablin 2011-2024")]
#endif