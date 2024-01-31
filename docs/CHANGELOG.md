### **v.1.0.1.0823** 
-> Was fixed tests and was added validator for input source.<br />
-> Was added `ToEnum<T>` from the string.<br />
-> Was added check methods `IsTypeOfNullableInt`, `IsTypeOfFloatingPoint`, `IsTypeOfNullableFloatingPoint` from  property type.

### **v.1.0.1.1842** 
-> Was added new byte extension: `ToStringFromByteUnicode`, `ToHexByte`.<br />
-> Was added new string extension: `ToBytesUnicode`.

### **v.1.0.2.1107** 
-> Was added new DateTime extension: `StartOfWeek`, `EndOfWeek`, `StartOfMonth`, `EndOfMonth`, `StartOfPreviousMonth`, `EndOfPreviousMonth`, `StartOfYear`, `EndOfYear`, `DaysInMonth`, `DaysInYear`, `GetIso8601WeekOfYear`.<br />
-> Was added new string extension: `ReplaceExact`.<br />
-> Was added new Exception extension: `GetFullError`.<br />
-> Was added new ExpandoObject extension: `AddProperty`, `UpdateValue`, `GetValue`.

### **v.1.0.2.1457** 
-> Was added new string extension: `IfNullOrWhiteSpace`, `IfNullOrEmpty`.<br />
-> Was added new TExtensions extension: `IfNotNull`.

### **v.1.0.3.0** 
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

### **v.1.0.3.1101** 
-> Was added new object extension: `SerializeToString` .<br />
-> Was added new string extension: `DeserializeToObject` .<br />
-> Was added new T extension: `SerializeToXmlDoc` .<br />

### **v.1.0.4.1925** 
-> Added support for net framework.<br />

### **v.1.0.5.1849** 
-> Added string extension `Contains`.<br />
-> Added int/long extension `IsLessZero`.<br />
-> Was added new Type extension: `GetStringPropertyNames`, `GetStringPropertyInfos`, `GetPropertyInfos`.<br />

### **v.1.0.5.2131** 
-> Added string extension `ParseToInt`, `ParseNullableInt`, `TryParseInt`.<br />
-> Added list extension `ActionForEach`.<br />

### **v.1.0.6.1341** 
-> Update vulnerable library version.<br />

### **v.1.0.7.0535** 
-> Add in directory helper new methods: `FileCount` x3, `DirectoryFileCount`.<br />

### **v.1.0.8.0638** 
-> Added string extension `GetHashSha512String`, `FromSpaceSeparatedString`, `IsMissing`, `IsNullOrEmpty`, `AddQueryString`, `AddHashFragment`, `GetOrigin`, `Obfuscate`.<br />
-> Added Enumerable extension `ToSpaceSeparatedString`, `HasDuplicates`, `GetDuplicates`.<br />

### **v.1.0.9.2108** 
-> Added object extensions `ThrowIfArgNull`, `ThrowArgIfNull`.<br />
-> Added bool extensions `IsTrue`, `IsFalse`.<br />
-> Added null check extensions `IsNotNull`, `IsDbNull`.<br />
-> Added string extensions `ThrowArgIfNull`, `ThrowArgIfNullOrEmpty`, `ThrowIfArgNull`, `ThrowIfArgNullOrEmpty`.<br />
-> Adjust validation for input params at some methods.
-> Small code refactor.

### **v.1.0.10.2315** 
-> Update lib version. Add option to sign the new version of the files.<br />
-> Small code refactor.

### **v.1.0.11.1319** 
-> Fix wrong modification.<br />

### **v.1.0.12.1447** 
-> Add IDataReader extensions to convert object in specific type.<br />

### **v.1.0.13.8399** 
-> Add excel column name generator `GetExcelColumnName`.<br />
-> Adjust method modifier for `GetDuplicates`.<br />
-> Fix tests.

### **v.1.0.14.6517** 
-> Fix some enums extensions.<br />
-> Add new methods (`AppendTo`, `GetPropertiesInfoFromSource`) in 'TExtensions'.