### **v3.4.0.7452** [[RzR](mailto:108324929+I-RzR-I@users.noreply.github.com)] 07-10-2025
* [c160a83] (RzR) -> Auto commit uncommited files
* [b417765] (RzR) -> Add new enumeration extnesion: `AdddIfNotExist`.
* [ab7bedc] (RzR) -> Add new dictionary extnesion: `IsNullOrEmpty`.
* [1f20f4a] (RzR) -> Add new dictionary extnesion: `AdddIfNotExist`.
* [0a7846f] (RzR) -> Add new array extnesion: `AppendIfNotExist`.

### **v3.3.0.5249** [[RzR](mailto:108324929+I-RzR-I@users.noreply.github.com)] 29-09-2025
* [325ff42] (RzR) -> Auto commit uncommited files
* [7385809] (RzR) -> Add new Guid extension methods: `NullIfEmpty`, `EmptyIfNull`, `IsMissing`, `HasValidValue`.
* [a37096b] (RzR) -> Add new DateTime extension methods: `IsOnlyDate`, `ForceMillisecondsToZero`.
* [d0f9816] (RzR) -> Commented some tests, but it's fully covered.
* [a01da4d] (RzR) -> Add new string extension `TruncateFromStart`.

### **v3.2.0.4113** [[RzR](mailto:108324929+I-RzR-I@users.noreply.github.com)] 13-08-2025
* [b16c919] (RzR) -> Auto commit uncommited files
* [b9538c3] (RzR) -> Add missign BASE32 methods: `Base32Encode`, `Base32Decode`. Adjust existing `IsBase32String`.

### **v3.1.0.3090** [[RzR](mailto:108324929+I-RzR-I@users.noreply.github.com)] 13-08-2025
* [bc2ef6d] (RzR) -> Auto commit uncommited files
* [90ce1ef] (RzR) -> Add and adjust test methods.
* [e8fafdc] (RzR) -> Add DateTime method: `Epoch`.
* [8bb8672] (RzR) -> Add byte method: `Base32BytesToString`.
* [896be11] (RzR) -> Add string methods `IsBase32String`, `Base32ToBytes` and adjust `TrimAndReduceSpace`, `TrimAndReplaceSpecialCharacters`.

### **v3.0.0.3007** [[RzR](mailto:108324929+I-RzR-I@users.noreply.github.com)] 07-08-2025
* [9e7a5e9] (RzR) -> Auto commit uncommited files.
* [ddc61a0] (RzR) -> Add new reflection extension methods (available from net45 and up); `GetTypes`, `GetSetMethod`, `GetGetMethod`, `GetGenericArguments`, `GetMethod`, `GetMembers`, `GetInterfaces`, `IsGenericType`, `IsValueType`, `IsAbstract`, `IsAssignableFrom`, `ContainsGenericParameters`, `BaseType`, `IsGenericTypeDefinition`, `IsPrimitive`, `IsNestedPublic`, `IsPublic`, `IsSealed`, `GetGenericParameterConstraints`, `IsClass`, `IsInterface`, `IsGenericParameter`, `GetGenericParameterAttributes`, `GetAssembly`, `GetConstructors`, `GetConstructor`, `IsInNamespace`.
* [379d971] (RzR) -> Add `NotNull` (safe) array extension. Add array/enumerable tests.
* [efb7a14] (RzR) -> Small code adjustments and improvements
* [c4ec2f8] (RzR) -> Add tests for `DomainEnsure` methods
* [780f3f5] (RzR) -> Adjust code to use `Ensure`. Adjust namespace.
* [716c5a4] (RzR) -> [**BreakingChanges**] Relocate extension classes `ReflectionExtensions`, `TypeBuilderExtensions` and `TypeExtensions`.
* [ad66c6d] (RzR) -> Adjust code to use defined extensions. Adjust namespace.
* [6e57990] (RzR) -> Add collection extension method `With` (accept array).
* [5db737c] (RzR) -> Add `Ensure` methods and refactor code.

### **v2.3.0.7698** [[RzR](mailto:108324929+I-RzR-I@users.noreply.github.com)] 15-07-2025
* [f300bd2] (RzR) -> Auto commit uncommited files
* [067dd42] (RzR) -> Add alternative extension method for `WithIndex`
* [4839a72] (RzR) -> Add string extension methods `IfStartsWith` and `IfNotStartsWith`.
* [572b664] (RzR) -> Add string extension methods `IfContains` and `IfNotContains`
* [872a4f6] (RzR) -> Add generic equality compare object `IfEquals` and `IfNotEquals`
* [50c5dcc] (RzR) -> Add func extensions sync/async `IsTrue`, `IsFalse`.

### v**2.2.0.8487** [[RzR](mailto:108324929+I-RzR-I@users.noreply.github.com)] 27-06-2025
* [9b9d498] (RzR) -> Auto commit uncommited files
* [e72cb92] (RzR) -> Adjust path to test projects in scripts
* [141087f] (RzR) -> Add changelog and version scripts
* [11e19df] (RzR) -> Add DataTable extensions `ToJson` and refactor anonimous class factory
* [17989a2] (RzR) -> Add new Enum extensions `AreEquals`.
* [3e56754] (RzR) -> Add new enumerable extensions `Chunked`

### **v1.0.1.0823** 
-> Was fixed tests and was added validator for input source.<br />
-> Was added `ToEnum<T>` from the string.<br />
-> Was added check methods `IsTypeOfNullableInt`, `IsTypeOfFloatingPoint`, `IsTypeOfNullableFloatingPoint` from  property type.

### **v1.0.1.1842** 
-> Was added new byte extension: `ToStringFromByteUnicode`, `ToHexByte`.<br />
-> Was added new string extension: `ToBytesUnicode`.

### **v1.0.2.1107** 
-> Was added new DateTime extension: `StartOfWeek`, `EndOfWeek`, `StartOfMonth`, `EndOfMonth`, `StartOfPreviousMonth`, `EndOfPreviousMonth`, `StartOfYear`, `EndOfYear`, `DaysInMonth`, `DaysInYear`, `GetIso8601WeekOfYear`.<br />
-> Was added new string extension: `ReplaceExact`.<br />
-> Was added new Exception extension: `GetFullError`.<br />
-> Was added new ExpandoObject extension: `AddProperty`, `UpdateValue`, `GetValue`.

### **v1.0.2.1457** 
-> Was added new string extension: `IfNullOrWhiteSpace`, `IfNullOrEmpty`.<br />
-> Was added new TExtensions extension: `IfNotNull`.

### **v1.0.3.0** 
-> Was renamed `Utils` to `GeneralUtils`.<br />
-> Was added new `EnumerateUtils` with methods: `FromTo<int>`, `FromTo<DateTime>` and `PowersOf`.<br />
-> Was added new `DirectoryHelper` with methods: `CreateDirectory`, `CopyDirectory` and `DeleteDirectory`.<br />
-> Was added new `DisposablesCollectionHelper` with methods: `Dispose`, `Add`.<br />
-> Was added new `InsensitiveCaseHashtableHelper` with methods: `ContainsKey`, `Add`, `Remove`.<br />
-> Was added new byte extension: `CompareTo`, `GZipCompress`.<br />
-> Was added new datetime extension: `ToEpochTime`, `ToExcelTime`.<br />
-> Was added new `DecimalExtensions` with methods: `IsNullOrZero`, `IsZero`, `IsLessOrEqualZero`, `IsGreaterThanOrEqualZero`, `IsGreaterThanZero`.<br />
-> Was cleaned `IntExtension` and removed unrelated type extension (moved to specific type class).<br />
-> Was added new `LongExtensions` with methods: `IsNullOrZero`, `IsZero`, `IsLessOrEqualZero`, `IsGreaterThanOrEqualZero`, `IsGreaterThanZero`, `SetFlag`, `IsFlagSet`.<br />
-> Was added new object extension: `In`, `ToDictionary`, `ThrowIfNull`, `ToString`, `To`.<br />
-> Was added new string extension: `ToLines`, `Chunked`, `ThrowIfNullOrEmpty`, `ThrowIfNull`, `ThrowIfEmpty`, `IsEmpty`, `XmlEncode`, `XmlDecode`.<br />
-> Was added new `TimeSpanExtension` with methods: `Absolute`.<br />
-> Was added new `LongExtensions` with methods: `IsNullOrZero`, `IsZero`, `IsGreaterThanZero`.<br />
-> Was added new type extension: `GetPropertyPath`, `IsAsType`.<br />
-> Was added new number extension: `IsBetween` (double, int), `GetNumberSuffix`.<br />
-> Was added new memory stream extension: `WriteAll` .<br />
-> Was added new exception extension: `WithData` .<br />
-> Was added new `QueueExtensions` with methods: `DequeueOrDefault`.<br />
-> Was added new `ListExtensions` with methods: `ToDataSet`, `RemoveFirst`, `RemoveLast`.<br />
-> Was added new `HashExtensions` with methods: `AddRange`.<br />
-> Was added new `EnumerableExtensions` with methods: `Replace`, `Join`, `IsLast`, `IsFirst`, `GetDifferences`, `ContainsAny`, `AnyStartWith` (input: IEnumerable<string>, string), `ToObservableCollection`, `Randomize`, `Transpose`, `ToCollection`, `Combinations`, `ToDataTable`, `ToDataTableDynamic`, `IsNullOrEmptyEnumerable`, `WithIndex`, `ListToString`, `CloneCollection`, `NotNull`.<br />
-> Was added new `ArrayExtensions` with methods: `IndexOf`.<br />

### **v1.0.3.1101** 
-> Was added new object extension: `SerializeToString` .<br />
-> Was added new string extension: `DeserializeToObject` .<br />
-> Was added new T extension: `SerializeToXmlDoc` .<br />

### **v1.0.4.1925** 
-> Added support for net framework.<br />

### **v1.0.5.1849** 
-> Added string extension `Contains`.<br />
-> Added int/long extension `IsLessZero`.<br />
-> Was added new Type extension: `GetStringPropertyNames`, `GetStringPropertyInfos`, `GetPropertyInfos`.<br />

### **v1.0.5.2131** 
-> Added string extension `ParseToInt`, `ParseNullableInt`, `TryParseInt`.<br />
-> Added list extension `ActionForEach`.<br />

### **v1.0.6.1341** 
-> Update vulnerable library version.<br />

### **v1.0.7.0535** 
-> Add in directory helper new methods: `FileCount` x3, `DirectoryFileCount`.<br />

### **v1.0.8.0638** 
-> Added string extension `GetHashSha512String`, `FromSpaceSeparatedString`, `IsMissing`, `IsNullOrEmpty`, `AddQueryString`, `AddHashFragment`, `GetOrigin`, `Obfuscate`.<br />
-> Added Enumerable extension `ToSpaceSeparatedString`, `HasDuplicates`, `GetDuplicates`.<br />

### **v1.0.9.2108** 
-> Added object extensions `ThrowIfArgNull`, `ThrowArgIfNull`.<br />
-> Added bool extensions `IsTrue`, `IsFalse`.<br />
-> Added null check extensions `IsNotNull`, `IsDbNull`.<br />
-> Added string extensions `ThrowArgIfNull`, `ThrowArgIfNullOrEmpty`, `ThrowIfArgNull`, `ThrowIfArgNullOrEmpty`.<br />
-> Adjust validation for input params at some methods.
-> Small code refactor.

### **v1.0.10.2315** 
-> Update lib version. Add option to sign the new version of the files.<br />
-> Small code refactor.

### **v1.0.11.1319** 
-> Fix wrong modification.<br />

### **v1.0.12.1447** 
-> Add IDataReader extensions to convert object in specific type.<br />

### **v1.0.13.8399** 
-> Add excel column name generator `GetExcelColumnName`.<br />
-> Adjust method modifier for `GetDuplicates`.<br />
-> Fix tests.

### **v1.0.14.6517** 
-> Fix some enums extensions.<br />
-> Add new methods (`AppendTo`, `GetPropertiesInfoFromSource`) in `TExtensions`.

### **v1.1.0.0** 
-> Remove unused packages.<br />
-> Downgrade some package versions to cover target frameworks.<br />
-> Fix some warnings and disposable objects.<br />

### **v1.1.1.7310** 
-> Adjust and clean up code execution.<br />
-> Reorganize typeparam extensions.<br />
-> Add new typeparam extensions: `IfIsNull`, `IfIsNotNull`, `IfIsNullOrFuncIsTrue`, `IfIsNullAndFuncIsTrue`, `IfFuncIsTrue`, `IfFuncIsFalse`, `IfFunc`, `IfNull`, `IfNotNull`.<br />

### **v1.1.2.3434** 
-> Add new string extensions: `AsRedacted`, `TrimPrefix`, `TrimSuffix`.<br />

### **v1.2.0.0** 
-> Add/adjust input validations in the `DataTypeExtensions` foler with extensions;<br />
-> Add new string extensions: `IfNullThenEmpty`.<br />

### **v1.3.0.0** 
-> Fix test for `CalculateAge`;<br />
-> Add new string extensions: `IsValidJson`, `IsValidJsonObject`, `IsValidJsonArray`;<br />

### **v2.0.0.0** 
-> Fix test for `CalculateAge`;<br />
-> Add DateTime extension method `AsNotNull`;<br />
-> Add new tests for `AsNotNull` methods;<br />
-> Add `EnumerateUtils` enumerable utils some tests;<br />
-> Adjust AES encryption(`AesEncryptString`, `AesDecryptString`) and expose iv as input;<br />
-> Adjust dynamic property/ies select avoid `System.Linq.Dynamic.Core`;<br />

### **v2.0.1.8588** 
-> Remove unused package `Microsoft.CodeAnalysis.Common`;<br />

### **v2.1.0.0** 
-> Add new string extensions: `ToStringArray`, `ArrayToString`;<br />
-> Add new array extensions: `AppendItem`, `AppendIfNotExists`, `RemoveItem`, `RemoveAtIdx`;<br />
-> Add new enumerable extensions: `GetDuplicates`, `ForEach`, `ForEachAndReturn`;<br />
-> Add passcode/password generation util;<br />

### **v2.1.1.6403** 
-> Add new string extension: `FormatWith`;<br />
-> Add new Guid/Guid? extension: `IsEmpty`;<br />
-> Relocate several string extensions: `IsGuid`, `ToGuid`, `FromDoubleQuotesWithBackSlashesToGuid`;<br />
