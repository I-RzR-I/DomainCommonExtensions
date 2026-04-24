# Namespace migration guide (v5)

The repository was reorganized into 11 stable namespace buckets. This is a
**breaking change**: every user should update its `using` directives. The
class names and method signatures themselves did not change, only the
namespaces they live in. Also, one more important thing is that the namespace was changed from `DomainCommonExtensions` to `RzR.Extensions.Domain`.

## Backward-compatibility shims (`_Legacy/` folder)

To soften the upgrade, every old namespace still exists via auto-generated
forwarder static classes under [`src/DomainCommonExtensions/_Legacy/`](../src/DomainCommonExtensions/_Legacy).
Each shim type is marked `[Obsolete]` with a message telling you the new
location, so existing v4 code keeps compiling but every call site lights up
in your IDE pointing to its v5 replacement.

## Namespace map

Update old `using` directives as follows:

| Old namespace | New namespace(s) | Notes |
|---|---|---|
| `DomainCommonExtensions.ArraysExtensions` | `RzR.Extensions.Domain.Collections`, `RzR.Extensions.Domain.Linq` | LinqExtensions split out to `Linq`; everything else → `Collections` |
| `DomainCommonExtensions.Collections` (supporting types: `IndexableEnumerable`, `MutableIndexableEnumerable`, `ObservableEnumerator`, `DisposableStackCollection`, `InsensitiveCaseHashtable`) | `RzR.Extensions.Domain.Collections.Types` | Containers separated from extension methods |
| `DomainCommonExtensions.CommonExtensions` | `RzR.Extensions.Domain.Async`, `RzR.Extensions.Domain.Cryptography`, `RzR.Extensions.Domain.Data`, `RzR.Extensions.Domain.Diagnostics`, `RzR.Extensions.Domain.IO`, `RzR.Extensions.Domain.Linq`, `RzR.Extensions.Domain.Primitives` | This was the catch-all; split per concern. `FilesExtensions`, `DirectoryInfoExtensions`, `MemoryStreamExtensions` → `IO`; `CryptoExtensions`, `AESEncryptionExtensions` → `Cryptography`; `ExpressionExtensions`, `PredicateBuilderExtensions` → `Linq`; `TaskExtensions`, `FuncExtensions` → `Async`; `ExceptionExtensions`, `DocumentationExtensions` → `Diagnostics`; `ExpandoObjectExtensions` → `Data`; `NumbersExtensions`, `NullExtensions`, `RandomExtensions` → `Primitives` |
| `DomainCommonExtensions.CommonExtensions.Encryption` | `RzR.Extensions.Domain.Cryptography.Rsa`, `RzR.Extensions.Domain.Cryptography.Tea` | AES extensions moved up to `Cryptography` (no `.Aes` sub-namespace because it shadows `System.Security.Cryptography.Aes`) |
| `DomainCommonExtensions.CommonExtensions.Reflection` | `RzR.Extensions.Domain.Reflection` | |
| `DomainCommonExtensions.CommonExtensions.SystemData` | `RzR.Extensions.Domain.Data` | |
| `DomainCommonExtensions.CommonExtensions.TypeParam` | `RzR.Extensions.Domain.Reflection.TypeParam` | |
| `DomainCommonExtensions.ConvertExtensions.DataReader` | `RzR.Extensions.Domain.Data.DataReader` | |
| `DomainCommonExtensions.DataTypeExtensions` | `RzR.Extensions.Domain.Primitives`, `RzR.Extensions.Domain.Text`, `RzR.Extensions.Domain.Diagnostics` | `StringExtensions`, `StringInjectExtension` → `Text`; `SocketExtensions` → `Diagnostics`; everything else → `Primitives` |
| `DomainCommonExtensions.Helpers` | `RzR.Extensions.Domain.IO`, `RzR.Extensions.Domain.IO.Ini`, `RzR.Extensions.Domain.Async`, `RzR.Extensions.Domain.Cryptography`, `RzR.Extensions.Domain.Primitives` | `DirectoryHelper` → `IO`; `IniFileHelper` → `IO.Ini`; `TaskRunnerHelper` → `Async`; `TinyEncryptionAlgorithmHelper` → `Cryptography.Tea`; `RandomHelper` → `Primitives` |
| `DomainCommonExtensions.Helpers.Internal` | `RzR.Extensions.Domain.Internal`, `RzR.Extensions.Domain.Cryptography` | `Base32EncodingHelper` → `Cryptography` (no `.Encoding` sub-namespace because it shadows `System.Text.Encoding`) |
| `DomainCommonExtensions.Helpers.Internal.AnonymousSelect[.Base|.Factory]` | `RzR.Extensions.Domain.Internal.AnonymousSelect[.Base|.Factory]` | |
| `DomainCommonExtensions.Resources` (`RegularExpressions`) | `RzR.Extensions.Domain.Text` | |
| `DomainCommonExtensions.Resources.Enums` | `RzR.Extensions.Domain.Collections` (`OrderType`), `RzR.Extensions.Domain.Validation` (`ExceptionType`) | |
| `DomainCommonExtensions.Utilities` | `RzR.Extensions.Domain.Collections`, `RzR.Extensions.Domain.Cryptography` | `EnumerateUtils`, `GeneralUtils` → `Collections`; `PasswordGenerateUtils` → `Cryptography` |
| `DomainCommonExtensions.Utilities.Ensure` | `RzR.Extensions.Domain.Validation` | `DomainEnsure`, `DomainEnsureExtensions` |
| `DomainCommonExtensions.Utilities.LazyLoad` | `RzR.Extensions.Domain.Async.LazyLoad` | `AsyncLazy<T>`, `AsyncExpiringLazy<T>` |

`RzR.Extensions.Domain.Models` is unchanged.

## How to migrate your code

Use a single project-wide find-and-replace for each row in the table above.
For namespaces that split, you may need to add multiple `using` directives —
your IDE's "Remove and sort usings" / `dotnet format` will trim the unused
ones automatically.

## Folder layout

The folder tree under `src/DomainCommonExtensions/` now mirrors the namespaces
exactly (folder name = last segment of namespace). `.editorconfig` enforces
this with `dotnet_diagnostic.IDE0130 = warning`.

```
DomainCommonExtensions/
├── Async/             (TaskExtensions, FuncExtensions, TaskRunnerHelper)
│   └── LazyLoad/      (AsyncLazy, AsyncExpiringLazy)
├── Collections/       (Array/List/Dictionary/Queue/Hash/Concurrent... + utils)
│   └── Types/         (IndexableEnumerable, ObservableEnumerator, ...)
├── Cryptography/      (AES, Crypto, Base32, Password... )
│   ├── Rsa/
│   └── Tea/           (TEA + TinyEncryptionAlgorithmHelper)
├── Data/              (DataReader/Record/Table, ExpandoObject)
│   └── DataReader/    (To* converters)
├── Diagnostics/       (Exception, Documentation, Socket)
├── Internal/          (private impl details)
│   └── AnonymousSelect/
├── IO/                (Files, Directory, MemoryStream)
│   └── Ini/           (IniFileHelper)
├── Linq/              (LinqExtensions, Expression, PredicateBuilder)
├── Models/            (unchanged)
├── Primitives/        (int/long/bool/byte/char/Guid/TimeSpan/Random/...)
├── Reflection/        (Type/Assembly/PropertyInfo/Reflection)
│   └── TypeParam/     (TypeParam* extensions)
├── Text/              (StringExtensions, StringInject, RegularExpressions)
└── Validation/        (DomainEnsure, ExceptionType)
```
