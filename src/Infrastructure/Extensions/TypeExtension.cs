using System.Collections;

namespace Infrastructure.Extensions;

internal static class TypeExtension
{
    /// <summary>
    /// Determines whether the specified type represents a <see cref="long"/> type.
    /// </summary>
    /// <param name="targetType">The type to check.</param>
    /// <returns>True if the specified type represents a <see cref="long"/> type; otherwise, false.</returns>
    internal static bool IsLong(this Type targetType) => targetType == typeof(long) || targetType == typeof(ulong);

    /// <summary>
    /// Determines whether the specified <see cref="Type"/> is an enumeration type,
    /// considering nullable types.
    /// </summary>
    /// <param name="targetType">The type to check.</param>
    /// <returns>
    /// <c>true</c> if the specified type is an enumeration type; otherwise, <c>false</c>.
    /// For nullable types, returns <c>true</c> if the underlying type is an enumeration.
    /// </returns>
    internal static bool IsEnum(this Type targetType) => targetType.IsNullableType() ? Nullable.GetUnderlyingType(targetType)?.IsEnum == true : targetType.IsEnum;

    /// <summary>
    /// Determines whether the specified <see cref="Type"/> is a <see cref="Guid"/> type,
    /// considering nullable types.
    /// </summary>
    /// <param name="targetType">The type to check.</param>
    /// <returns>
    /// <c>true</c> if the specified type is a <see cref="Guid"/> type; otherwise, <c>false</c>.
    /// For nullable types, returns <c>true</c> if the underlying type is a <see cref="Guid"/>.
    /// </returns>
    internal static bool IsGuid(this Type targetType) => targetType.IsNullableType() ? Nullable.GetUnderlyingType(targetType) == typeof(Guid) : targetType == typeof(Guid);

    /// <summary>
    /// Determines whether the specified type represents a <see cref="DateTime"/> or <see cref="DateTimeOffset"/> type.
    /// </summary>
    /// <param name="targetType">The type to check.</param>
    /// <returns>True if the specified type represents a <see cref="DateTime"/> or <see cref="DateTimeOffset"/> type; otherwise, false.</returns>
    internal static bool IsDateTime(this Type targetType) => targetType == typeof(DateTime) || targetType == typeof(DateTimeOffset);

    /// <summary>
    /// Determines whether the specified type represents a class type (excluding <see cref="string"/> and <see cref="IEnumerable"/> types).
    /// </summary>
    /// <param name="type">The type to check.</param>
    /// <returns>True if the specified type represents a class type; otherwise, false.</returns>
    internal static bool IsClass(this Type type) => type != typeof(string) && !type.IsAssignableTo(typeof(IEnumerable)) && type.IsClass;

    /// <summary>
    /// Determines whether the specified type represents a collection type (<see cref="Array"/> or <see cref="IEnumerable"/>).
    /// </summary>
    /// <param name="type">The type to check.</param>
    /// <returns>True if the specified type represents a collection type; otherwise, false.</returns>
    internal static bool IsCollection(this Type type) => type.IsArray || type != typeof(string) && type.IsAssignableTo(typeof(IEnumerable));

    /// <summary>
    /// Determines whether the specified type represents a <see cref="Nullable"/> type.
    /// </summary>
    /// <param name="type">The type to check.</param>
    /// <returns>True if the specified type represents a <see cref="Nullable"/> type; otherwise, false.</returns>
    internal static bool IsNullableType(this Type type) => type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);

    /// <summary>
    /// Determines whether the specified type represents a primitive type.
    /// </summary>
    /// <param name="type">The type to check.</param>
    /// <returns>True if the specified type represents a primitive type; otherwise, false.</returns>
    /// <remarks>
    /// This method checks if the specified type is a numeric type, an <see cref="Enum"/>, <see cref="string"/>, <see cref="char"/>, <see cref="Guid"/>, or <see cref="bool"/>.
    /// Additionally, if the type is <see cref="Nullable"/>, it recursively checks if its underlying type is a primitive type.
    /// </remarks>
    internal static bool IsPrimitiveType(this Type type)
    {
        return type.IsNumericType() ||
            type.IsEnum ||
            type == typeof(string) ||
            type == typeof(char) ||
            type == typeof(Guid) ||
            type == typeof(bool) ||

             type.IsNullableType() &&
             Nullable.GetUnderlyingType(type)!.IsPrimitiveType()
            ;
    }

    /// <summary>
    /// Determines whether the specified type represents a numeric type.
    /// </summary>
    /// <param name="targetType">The type to check.</param>
    /// <returns>True if the specified type represents a numeric type; otherwise, false.</returns>
    /// <remarks>
    /// This method checks if the specified type is one of the numeric types: <see cref="int"/>, <see cref="uint"/>, <see cref="float"/>,
    /// <see cref="double"/>, <see cref="decimal"/>, <see cref="short"/>, <see cref="ushort"/>, <see cref="byte"/>, <see cref="sbyte"/>, <see cref="long"/>,
    /// <see cref="DateTime"/>, <see cref="DateTimeOffset"/>.
    /// Additionally, if the type is <see cref="Nullable"/>, it recursively checks if its underlying type is a numeric type.
    /// </remarks>
    internal static bool IsNumericType(this Type targetType)
    {
        return targetType == typeof(int) ||
               targetType == typeof(uint) ||
               targetType == typeof(float) ||
               targetType == typeof(double) ||
               targetType == typeof(decimal) ||
               targetType == typeof(short) ||
               targetType == typeof(ushort) ||
               targetType == typeof(byte) ||
               targetType == typeof(sbyte) ||
               targetType.IsLong() ||
               targetType.IsDateTime() ||

                targetType.IsNullableType() &&
                Nullable.GetUnderlyingType(targetType)!.IsNumericType()
               ;
    }
}
