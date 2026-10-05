# RzR.Extensions.Domain

[![NuGet Version](https://img.shields.io/nuget/v/RzR.Extensions.Domain.svg?style=flat&logo=nuget)](https://www.nuget.org/packages/RzR.Extensions.Domain/)
[![Nuget Downloads](https://img.shields.io/nuget/dt/RzR.Extensions.Domain.svg?style=flat&logo=nuget)](https://www.nuget.org/packages/RzR.Extensions.Domain)

<details>
  <summary>Old version</summary>
  
[![NuGet Version](https://img.shields.io/nuget/v/DomainCommonExtensions.svg?style=flat&logo=nuget)](https://www.nuget.org/packages/DomainCommonExtensions/)
[![Nuget Downloads](https://img.shields.io/nuget/dt/DomainCommonExtensions.svg?style=flat&logo=nuget)](https://www.nuget.org/packages/DomainCommonExtensions)

</details>
<br />

600+ C# extension methods for strings, dates, collections, LINQ and IO. One package, .NET Framework to modern .NET.

It's a toolbox: import the namespace you need and ignore the rest.

## Install

```shell
dotnet add package RzR.Extensions.Domain
```

Or reference it in your project file:

```xml
<PackageReference Include="RzR.Extensions.Domain" Version="7.0.0.8134" />
```

Or from the Package Manager Console in Visual Studio:

```powershell
Install-Package RzR.Extensions.Domain
```

Still on the old `DomainCommonExtensions` package? Read [Upgrading](#upgrading) first.

## Quick start

Each area has its own namespace, so you add a `using` for each one you call.

```csharp
using System;
using RzR.Extensions.Domain.Primitives;
using RzR.Extensions.Domain.Text;

string name   = "  Ada  ".TrimToNull();                         // "Ada"
string blank  = "   ".TrimToNull();                             // null
string[] tags = " api, web,, db ".SplitAndTrim();               // ["api", "web", "db"]
bool missing  = "   ".IsMissing();                              // true
string path   = "api/v1".EnsureEndsWith("/");                   // "api/v1/"
string host   = "ada@example.com".SubstringAfter("@");          // "example.com"
string slug   = "Hello World".ToSlug();                         // "hello-world"
string card   = "4111111111111111".Mask("****-****-****-####"); // "****-****-****-1111"
string uptime = TimeSpan.FromMinutes(135).ToHumanReadable();    // "2h 15m"
bool noId     = Guid.Empty.IsMissing();                         // true
```

The [demo site](https://demowebutils.iamrzr.dev/) lets you try some of the methods in the browser.

## What's inside

Every namespace below starts with `RzR.Extensions.Domain.`

| Namespace | What's there | Examples |
|---|---|---|
| `Async` | Task helpers; lazy async values live in `Async.LazyLoad` | `AsyncLazy<T>`, `AsyncExpiringLazy<T>`, `TaskRunnerHelper.Run` |
| `Collections` | Enumerable, list, dictionary, queue and concurrent-collection helpers; extra collection types in `Collections.Types` | `Chunked`, `HasDuplicates`, `ToDataTable`, `AddOrUpdate`, `IndexableEnumerable<T>` |
| `Cryptography` | AES string encryption, RSA in `Cryptography.Rsa`, TEA/XXTEA in `Cryptography.Tea`, a password generator in `Cryptography.Passwords` (read the note below) | `AesEncryptString`, `AesDecryptString`, `EncryptRSAXmlKey`, `TEAEncrypt` |
| `Data` | DataTable, data reader and data record helpers; typed column readers in `Data.DataReader` | `ToList<T>`, `ReadDbReaderOneRow<T>`, `IsDbNull`, `HasColumn`, `ToInt16` |
| `Diagnostics` | Exception details, XML doc comments read from an assembly, socket keep-alive | `GetFullError`, `WithData`, `GetSummary`, `XmlFromAssembly` |
| `IO` | Temp-file-then-replace file writes, file hashes, directory copy; INI files in `IO.Ini` | `SafeWriteAllText`, `Sha256HexFromFile`, `IsFileInUse`, `DirectoryHelper.CopyDirectory`, `IniFileHelper` |
| `Linq` | Predicate composition and query helpers for IQueryable and expression trees | `PredicateBuilderExtensions`, `WhereIf`, `OrderByDynamic`, `AndAlso` |
| `Models` | Small result structs you can deconstruct, and a model that pairs an item with its index | `TupleResult`, `WithIndexModel<T>` |
| `Primitives` | Numbers, dates, enums, Guid, TimeSpan, bool, byte and char; random values; time-ordered IDs | `StartOfMonth`, `CalculateAge`, `ToHumanReadable`, `GetDescription`, `TimeSeqId.Generate()` |
| `Reflection` | Copying and looking up properties, type checks, attribute lookup; generic helpers in `Reflection.TypeParam` | `CopyProperties`, `GetPropertyValue`, `GetNonNullableType`, `HasAttribute<T>`, `IfIsNull` |
| `Text` | String checks, trimming, slicing, slugs, masking, Base64Url and Base32, placeholder templates | `TrimToNull`, `SubstringBefore`, `ToSlug`, `Mask`, `Inject` |
| `Validation` | Guard clauses that throw on null or empty arguments | `DomainEnsure.IsNotNull`, `ThrowIfArgNull`, `ThrowIfArgNullOrEmpty` |

> The `Cryptography` helpers (AES, RSA, TEA/XXTEA, password generator) are convenience and legacy-interop wrappers and have not been reviewed for security-sensitive use. AES runs in CBC mode without authentication, some RSA overloads depend on Windows key containers or `RSACryptoServiceProvider`, and the password generator (like `RandomHelper.Instance.Token`) uses `System.Random`. For new code that handles secrets, use `System.Security.Cryptography` directly (for example `RandomNumberGenerator`, and `AesGcm` where your target framework has it) or a vetted library.

Also worth a look: `TimeSeqId.Generate()` returns a time-ordered, sortable 48-character string ID.
[docs/usage.md](docs/usage.md) covers it in detail, along with `AsyncLazy<T>` and `AsyncExpiringLazy<T>`.

## Compatibility

The package ships four builds. NuGet picks the closest one for your project.

| TFM | Picked by | Extra dependencies |
|---|---|---|
| `net40` | .NET Framework 4.0 | None |
| `net45` | .NET Framework 4.5–4.8.1 | System.ComponentModel.Annotations, System.ValueTuple |
| `netstandard2.0` | .NET Core 2.x and other .NET Standard 2.0 platforms (not .NET Framework, which gets `net45`) | Same as `net45`, plus Microsoft.Extensions.Hosting.Abstractions, System.Reflection.Emit, System.Text.Encodings.Web, System.Text.Json |
| `netstandard2.1` | .NET Core 3.0+ / .NET 5+ | Same as `netstandard2.0`, without System.Reflection.Emit |

The `net40` build has fewer extension methods: 596, compared with 648 in `netstandard2.1`.

## Upgrading

### 7.0

- 7.0 no longer depends on `RzR.Core.CodeSource`. If your code used its types, add the package yourself: `dotnet add package RzR.Core.CodeSource`.
- The `EmitCodeSource` build switch only matters when you build this repository from source; package consumers don't set it.
- New string helpers in `RzR.Extensions.Domain.Text`: `IsEquals`/`IsEqualsIgnoreCase`/`IsEqualsInvariantCulture`, `IfIsMissing`, `TrimToNull`, `SplitAndTrim`, `EnsureStartsWith`/`EnsureEndsWith`, `SubstringBefore`/`SubstringAfter` and `ReplaceIgnoreCase`.
- Everything else that changed is in [docs/CHANGELOG.md](docs/CHANGELOG.md).

### 6.0 and 5.0

- 6.0 removed the `_Legacy` `[Obsolete]` shims. See the [migration guide](docs/namespace-migration-v5.md).
- 5.0 moved the namespaces from `DomainCommonExtensions.*` to `RzR.Extensions.Domain.*`. See the [migration guide](docs/namespace-migration-v5.md).

### Legacy package id

`DomainCommonExtensions` is the old package id. It stopped at 5.0.0.7637 and is deprecated on NuGet.
New versions ship only as `RzR.Extensions.Domain`.
To move over, point your PackageReference at `RzR.Extensions.Domain`, then use the [migration guide](docs/namespace-migration-v5.md) to update your `using` directives.

## Documentation and links

- [docs/usage.md](docs/usage.md): `AsyncLazy<T>`, `AsyncExpiringLazy<T>` and `TimeSeqId` in depth.
- [docs/CHANGELOG.md](docs/CHANGELOG.md): release history.
- [docs/namespace-migration-v5.md](docs/namespace-migration-v5.md): old-to-new namespace map for the 5.0 move.
- [docs/branch-guide.md](docs/branch-guide.md): how the repository branches are used.
- [Demo site](https://demowebutils.iamrzr.dev/): try some of the methods in the browser.
- [NuGet package](https://www.nuget.org/packages/RzR.Extensions.Domain): versions and per-framework dependencies.
