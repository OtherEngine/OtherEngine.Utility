﻿using System.Reflection;
using System;

[assembly: AssemblyProduct("OtherEngine")]
[assembly: AssemblyTitle("OtherEngine.Utility")]
[assembly: AssemblyCopyright("copygirl")]

[assembly: AssemblyVersion("1.0")]

#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif

[assembly: CLSCompliant(true)]

