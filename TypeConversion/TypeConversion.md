# Type Conversion System

---
&emsp;The type conversion system is used primarily to convert from database types
to .Net types and back again.  Each type can be handled by it's own class inheriting
from TypeHandlerBase or implementing ITypeHandler.

---

#### Source Files:

- CLRTypeCode.cs
- CLR Type Handlers:
	- BooleanHandler
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
	- EnumHandler<T>
- ITypeHandlerService.cs
- ITypeHandler.cs
- NullConvert.cs
- TypeConversionExtensions.cs
- TypeConversionService.cs
- TypeHandlerBase.cs

---

##### CLRTypeCode.cs:

---

##### CLRTypeHandlers.cs:
##### ITypeHandlerService.cs:
##### ITypeHandler.cs:
##### NullConverter.cs:
##### TypeConversionExtensions.cs:
##### TypeConversionService.cs:
##### TypeHandlerBase.cs:

---
Notes: