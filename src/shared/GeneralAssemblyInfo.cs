// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2022-08-10 21:39
// 
//  Last Modified By : RzR
//  Last Modified On : 2022-08-12 23:40
// ***********************************************************************
//  <copyright file="GeneralAssemblyInfo.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System.Reflection;
using System.Resources;

#endregion

#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif

[assembly: AssemblyCompany("RzR ®")]
[assembly: AssemblyProduct("Common data type extensions")]
[assembly: AssemblyCopyright("Copyright © 2022 RzR All rights reserved.")]
[assembly: AssemblyTrademark("® RzR™")]
[assembly:
    AssemblyDescription(
        "The purpose of this repository/library is to provide the most relevant and used extension methods in the life cycle of application development that allow us to improve our code, and writing speed, and use more efficiently dev team time during this period for more complex functionality.")]

[assembly: AssemblyMetadata("TermsOfService", "")]

[assembly: AssemblyMetadata("ContactUrl", "")]
[assembly: AssemblyMetadata("ContactName", "RzR")]
[assembly: AssemblyMetadata("ContactEmail", "ddpRzR@hotmail.com")]

[assembly: NeutralResourcesLanguage("en-US", UltimateResourceFallbackLocation.MainAssembly)]

[assembly: AssemblyVersion("1.0.2.1457")]
[assembly: AssemblyFileVersion("1.0.2.1457")]
[assembly: AssemblyInformationalVersion("1.0.2.1457")]