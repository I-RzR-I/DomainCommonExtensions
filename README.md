> **Note** This repository is developed for net40, net45, .netstandard2.0 and .netstandard2.1.

[![NuGet Version](https://img.shields.io/nuget/v/DomainCommonExtensions.svg?style=flat&logo=nuget)](https://www.nuget.org/packages/DomainCommonExtensions/)
[![Nuget Downloads](https://img.shields.io/nuget/dt/DomainCommonExtensions.svg?style=flat&logo=nuget)](https://www.nuget.org/packages/DomainCommonExtensions)

> **v5 — Namespace reorganisation (BREAKING)**
>
> Starting with **v5**, the library was reorganised  with a new namespace (from `DomainCommonExtensions` to `RzR.Extensions.Domain`) and focused into namespace buckets under the new root `RzR.Extensions.Domain.*`
> (`Async`, `Collections`, `Cryptography`, `Data`, `Diagnostics`, `Internal`, `IO`, `Linq`, `Models`, `Primitives`, `Reflection`, `Text`, `Validation`).
> Class names and method signatures are unchanged — only the namespaces moved.
>
> To soften the upgrade, every old (v4) namespace is preserved as `[Obsolete]` forwarder static classes under
> [`src/DomainCommonExtensions/_Legacy/`](src/DomainCommonExtensions/_Legacy/), so existing v4 code keeps compiling
> while the IDE highlights every call-site with the new location.
>
> See the full mapping in [docs/namespace-migration-v5.md](docs/namespace-migration-v5.md).

This library/repository was created as a way to simplify the development process. It collects the most used extension methods for primitive data types (`int`, `string`, `DateTime`, `Enum`, `bool`, `byte`, `char`, `Guid`, `TimeSpan`, …) and for the common collection interfaces (`ICollection`, `IEnumerable`, `IList`, `HashSet`, `IQueryable`, `IDictionary`, `ConcurrentDictionary`, `Queue`, `DynamicList`, …).

It also bundles `cryptography` helpers for encrypting/decrypting strings by key with `RSA`, `AES`, `TEA`, plus a `Base32` encoder and a password generator.

If you need to extract documentation/comments from an `Assembly`, those extensions are here too. You will also find async lazy-load primitives `AsyncLazy<T>` and `AsyncExpiringLazy<T>`.

Additionally, there are extensions for `System.Data` (`DbDataReader`, `IDataRecord`, `DataTable`), `FileStream`, `MemoryStream`, `Type`, plus `Linq` / `Expression` extensions and a `PredicateBuilder` to keep call-sites clean.

As previously, I said here are collected the most relevant and used extension methods in the life cycle of application development that allow us to improve our code, and writing speed, and use more efficiently dev team time during this period for more complex functionality.
The list of helpful methods and extensions list isn't finished, I think in a short period I'll complete it with more fun things.

I hope I'm on the right way to providing all these things to all who saw or searched for something to make easy life in development, and enjoyable. I think you will find it helpful for all projects where needed.

**You can find a [demo](https://demowebutils.iamrzr.dev/) with some of the extension methods on the [demowebutils](https://demowebutils.iamrzr.dev/) website.**


**In case you wish to use it in your project, you can install the package from <a href="https://www.nuget.org/packages/DomainCommonExtensions" target="_blank">nuget.org</a>** or specify what version you want:

> `Install-Package DomainCommonExtensions -Version x.x.x.x`

## Content
1. [USING](docs/usage.md)
2. [CHANGELOG](docs/CHANGELOG.md)
3. [BRANCH-GUIDE](docs/branch-guide.md)
4. [V5 NAMESPACE MIGRATION](docs/namespace-migration-v5.md)
