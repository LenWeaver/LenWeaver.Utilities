using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace LenWeaver.Utilities {


    public static class CompareFunctionBuilder {

        // Generates a single compiled Func<T, T, int> for multi-column sorting
        public static Func<T?,T?,int> Build<T>( IEnumerable<SortDescriptor> sortDefinitions ) {

            Expression              combination;

            ParameterExpression?    paramX;
            ParameterExpression?    paramY;

            Type                    type;


            type        = typeof(T);
            paramX      = Expression.Parameter( type, "x" );
            paramY      = Expression.Parameter( type, "y" );

            // Start with a fallback result of 0 (equal)
            combination = Expression.Constant( 0 );

            // Process columns in reverse order (bottom-up) to nest secondary sorts inside primary sorts
            List<SortDescriptor> definitionsList = new List<SortDescriptor>( sortDefinitions );
            for( int i = definitionsList.Count - 1; i >= 0; i-- ) {
                SortDescriptor sd = definitionsList[i];
            
                // Get property info
                var propInfo = type.GetProperty( sd.ColumnName, BindingFlags.Public | BindingFlags.Instance )
                    ?? throw new ArgumentException( $"Property '{sd.ColumnName}' not found on type '{type.Name}'." );

                // Build x.Property and y.Property
                var propX = Expression.Property( paramX, propInfo );
                var propY = Expression.Property( paramY, propInfo );

                // Determine the correct Comparer<TProperty>.Default
                var defaultComparerType = typeof(Comparer<>).MakeGenericType( propInfo.PropertyType );
                var defaultProperty = defaultComparerType.GetProperty( "Default", BindingFlags.Public | BindingFlags.Static )!;
                var defaultComparerInstance = Expression.Property( null, defaultProperty );

                // Find the Compare method: int Compare(TProperty, TProperty)
                var compareMethod = defaultComparerType.GetMethod( "Compare", [propInfo.PropertyType, propInfo.PropertyType] )!;

                // Call Comparer<TProperty>.Default.Compare(x.Prop, y.Prop)
                Expression compareCall = sd.SortAscending
                    ? Expression.Call( defaultComparerInstance, compareMethod, propX, propY ) // Swap X and Y for descending
                    : Expression.Call( defaultComparerInstance, compareMethod, propY, propX );

                // Variable to hold the result of the current column comparison
                var resultVar = Expression.Variable( typeof(int), $"result_{sd.ColumnName}" );

                // If the current column result != 0, return it. Otherwise, evaluate the next/nested sorting columns.
                var block = Expression.Block(
                    [resultVar],
                    Expression.Assign( resultVar, compareCall ),
                    Expression.Condition(
                        Expression.NotEqual( resultVar, Expression.Constant( 0 ) ),
                        resultVar,
                        combination
                    )
                );

                combination = block;
            }

            // Wrap the final expression tree into a executable delegate
            var lambda = Expression.Lambda<Func<T?,T?,int>>( combination, paramX, paramY );
            return lambda.Compile();
        }

        // Overload for when you only have the System.Type object at runtime
        public static object Build( Type objectType, IEnumerable<SortDescriptor> sortDefinitions ) {

            var method = typeof(CompareFunctionBuilder)
                .GetMethod( nameof(Build), BindingFlags.Public | BindingFlags.Static, [typeof(SortDescriptor)] )!;
        
            var genericMethod = method.MakeGenericMethod( objectType );
            return genericMethod.Invoke( null, [sortDefinitions] )!;
        }


        public static Func<object,T?>       CreateGetter<T>         ( Type type, string propertyName ) {

            Expression                      body;
            Expression<Func<object,T?>>     lambda;
            MemberExpression?               prop;
            MethodCallExpression?           toString;
            MethodInfo?                     parse;
            ParameterExpression?            param;
            PropertyInfo?                   pi;
            UnaryExpression?                cast;


            param               = Expression.Parameter( typeof(object), "obj" );
            cast                = Expression.Convert( param, type );

            pi                  = type.GetProperty( propertyName );
            if( pi == null ) throw new InvalidOperationException( $"Property {propertyName} not found on {type.Name}." );

            prop                = Expression.Property( cast, pi );

            if( pi.PropertyType == typeof(T) ) {
                body            = prop;
            }
            else if( typeof(T).IsAssignableFrom( pi.PropertyType ) ) {
                body            = Expression.Convert( prop, typeof(T) );
            }
            else {
                toString        = Expression.Call( prop, nameof(ToString), Type.EmptyTypes );

                parse           = typeof(T).GetMethod( "Parse", new[] { typeof(string) } );
                if( parse != null ) {
                    body        = Expression.Call( parse, toString );
                }
                else {
                    throw new InvalidOperationException( $"Cannot convert property {propertyName} ({pi.PropertyType.Name}) to {typeof(T).Name}." );
                }
            }

            lambda              = Expression.Lambda<Func<object,T?>>( body, param );

            return lambda.Compile();
        }

        public static Func<object,bool>     CreateBooleanGetter     ( Type type, string propertyName ) => CreateGetter<bool>       ( type, propertyName );
        public static Func<object,Char>     CreateCharGetter        ( Type type, string propertyName ) => CreateGetter<Char>       ( type, propertyName );
        public static Func<object,SByte>    CreateSByteGetter       ( Type type, string propertyName ) => CreateGetter<SByte>      ( type, propertyName );
        public static Func<object,Byte>     CreateByteGetter        ( Type type, string propertyName ) => CreateGetter<Byte>       ( type, propertyName );
        public static Func<object,Int16>    CreateInt16Getter       ( Type type, string propertyName ) => CreateGetter<Int16>      ( type, propertyName );
        public static Func<object,UInt16>   CreateUInt16Getter      ( Type type, string propertyName ) => CreateGetter<UInt16>     ( type, propertyName );
        public static Func<object,Int32>    CreateInt32Getter       ( Type type, string propertyName ) => CreateGetter<Int32>      ( type, propertyName );
        public static Func<object,UInt32>   CreateUInt32Getter      ( Type type, string propertyName ) => CreateGetter<UInt32>     ( type, propertyName );
        public static Func<object,Int64>    CreateInt64Getter       ( Type type, string propertyName ) => CreateGetter<Int64>      ( type, propertyName );
        public static Func<object,UInt64>   CreateUInt64Getter      ( Type type, string propertyName ) => CreateGetter<UInt64>     ( type, propertyName );
        public static Func<object,Single>   CreateSingleGetter      ( Type type, string propertyName ) => CreateGetter<Single>     ( type, propertyName );
        public static Func<object,Double>   CreateDoubleGetter      ( Type type, string propertyName ) => CreateGetter<Double>     ( type, propertyName );
        public static Func<object,Decimal>  CreateDecimalGetter     ( Type type, string propertyName ) => CreateGetter<Decimal>    ( type, propertyName );
        public static Func<object,DateTime> CreateDateTimeGetter    ( Type type, string propertyName ) => CreateGetter<DateTime>   ( type, propertyName );
        public static Func<object,DateOnly> CreateDateOnlyGetter    ( Type type, string propertyName ) => CreateGetter<DateOnly>   ( type, propertyName );
        public static Func<object,TimeOnly> CreateTimeOnlyGetter    ( Type type, string propertyName ) => CreateGetter<TimeOnly>   ( type, propertyName );
        public static Func<object,TimeSpan> CreateTimeSpanGetter    ( Type type, string propertyName ) => CreateGetter<TimeSpan>   ( type, propertyName );
        public static Func<object,Guid>     CreateGuidGetter        ( Type type, string propertyName ) => CreateGetter<Guid>       ( type, propertyName );
        public static Func<object,String>   CreateStringGetter      ( Type type, string propertyName ) => CreateGetter<String>     ( type, propertyName );
    }
}