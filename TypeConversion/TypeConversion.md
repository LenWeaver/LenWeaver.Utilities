## Type Conversion System

---
&emsp;The type conversion system is used primarily to convert from database types
to .Net types and back again.  Each type can be handled by it's own class inheriting
from TypeHandlerBase or implementing ITypeHandler.

---

#### Source Files:

- CLRTypeCode.cs
- CLRTypeHandlers.cs
	- BooleanHandler
	- CharHandler
	- ByteHandler
	- Int32Handler
	- Int64Handler
	- DoubleHandler
	- DecimalHandler
	- StringHandler
	- DateTimeHandler
	- DateOnlyHandler
	- TimeOnlyHandler
	- GuidHandler
	- ByteArrayHandler
	- EnumHandler&lt;T&gt;
	- ValueTupleHandler&lt;T&gt;
	- FontDescriptorHandler
- ITypeHandlerService.cs
- ITypeHandler.cs
- NullableConvert.cs
- TypeConversionExtensions.cs
- TypeConversionService.cs
- TypeHandlerBase.cs

---

##### [CLRTypeCode.cs:](CLRTypeCode.cs)

&emsp;The CLRTypeCode enumerated type is similar to the TypeCode enumerated type
from earlier versions of .Net.  CLRTypeCode adds members for more recent types
such as DateOnly and TimeOnly.  The [TypeConversionExtensions.cs](TypeConversionExtensions.cs)
file contains an extension method that provides a **GetCLRTypeCode()** method to
all types.

---

##### [CLRTypeHandlers.cs:](CLRTypeHandlers.cs)

&emsp;The CLRTypeHandlers.cs source file contains Type Handlers for the following
data types: Boolean, Byte, Int32, Int64, Double, Decimal, String, DateTime,
DateOnly, TimeOnly, Guid, Byte[], Enum and Json.  Others to be added soon.

---

##### ITypeHandlerService.cs:
##### ITypeHandler.cs:
##### NullableConvert.cs:
##### TypeConversionExtensions.cs:
##### TypeConversionService.cs:
##### TypeHandlerBase.cs:

---
Notes: &emsp;An [UnderConstruction](..\Miscellaneous\UnderConstructionAttribute.cs) 
attribute exists that can <em>hide</em> an ITypeHandler implementation from the
[ObjectRegistry](..\ObjectRegistry\ObjectRegistry.cs).