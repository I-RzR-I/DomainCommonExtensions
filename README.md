> **Note** This repository is developed for net40, net45, .netstandard2.0 and .netstandard2.1.

[![NuGet Version](https://img.shields.io/nuget/v/RzR.Extensions.Domain.svg?style=flat&logo=nuget)](https://www.nuget.org/packages/RzR.Extensions.Domain/)
[![Nuget Downloads](https://img.shields.io/nuget/dt/RzR.Extensions.Domain.svg?style=flat&logo=nuget)](https://www.nuget.org/packages/RzR.Extensions.Domain)

<details>

  <summary>Old version</summary>
  
[![NuGet Version](https://img.shields.io/nuget/v/DomainCommonExtensions.svg?style=flat&logo=nuget)](https://www.nuget.org/packages/DomainCommonExtensions/)
[![Nuget Downloads](https://img.shields.io/nuget/dt/DomainCommonExtensions.svg?style=flat&logo=nuget)](https://www.nuget.org/packages/DomainCommonExtensions)

</details>

<br />


This library/repository was created as a way to simplify the development process. It collects the most used extension methods for primitive data types (`int`, `string`, `DateTime`, `Enum`, `bool`, `byte`, `char`, `Guid`, `TimeSpan`, …) and for the common collection interfaces (`ICollection`, `IEnumerable`, `IList`, `HashSet`, `IQueryable`, `IDictionary`, `ConcurrentDictionary`, `Queue`, `DynamicList`, …).

It also bundles `cryptography` helpers for encrypting/decrypting strings by key with `RSA`, `AES`, `TEA`, plus a `Base32` encoder and a password generator.

A `TimeSeqId` generator in `Primitives` produces time-ordered, lexicographically sortable 48-character unique identifiers with 64-bit CSPRNG entropy and a SHA-256 integrity checksum — a structured, sortable alternative to `Guid`.

If you need to extract documentation/comments from an `Assembly`, those extensions are here too. You will also find async lazy-load primitives `AsyncLazy<T>` and `AsyncExpiringLazy<T>`.

Additionally, there are extensions for `System.Data` (`DbDataReader`, `IDataRecord`, `DataTable`), `FileStream`, `MemoryStream`, `Type`, plus `Linq` / `Expression` extensions and a `PredicateBuilder` to keep call-sites clean.

As previously, I said here are collected the most relevant and used extension methods in the life cycle of application development that allow us to improve our code, and writing speed, and use more efficiently dev team time during this period for more complex functionality.
The list of helpful methods and extensions list isn't finished, I think in a short period I'll complete it with more fun things.

I hope I'm on the right way to providing all these things to all who saw or searched for something to make easy life in development, and enjoyable. I think you will find it helpful for all projects where needed.

**You can find a [demo](https://demowebutils.iamrzr.dev/) with some of the extension methods on the [demowebutils](https://demowebutils.iamrzr.dev/) website.**


**In case you wish to use it in your project, you can install the package from <a href="https://www.nuget.org/packages/RzR.Extensions.Domain" target="_blank">nuget.org</a>** or specify what version you want:

> `Install-Package RzR.Extensions.Domain -Version x.x.x.x`

## Content
1. [USING](docs/usage.md)
2. [CHANGELOG](docs/CHANGELOG.md)
3. [BRANCH-GUIDE](docs/branch-guide.md)
