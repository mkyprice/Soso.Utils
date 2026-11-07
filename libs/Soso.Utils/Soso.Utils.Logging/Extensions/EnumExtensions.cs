using System;
using System.Runtime.CompilerServices;

namespace Soso.Utils.Logging.Extensions;

internal static class EnumExtensions
{
    public static bool HasFlagNoAlloc<TEnum>(this TEnum value, TEnum flag)
        where TEnum : unmanaged, Enum
    {
        ulong numericValue = value.ToValue();
        ulong numericFlag = flag.ToValue();
        return (numericValue & numericFlag) == numericFlag;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ulong ToValue<TEnum>(this TEnum value)
        where TEnum : unmanaged, Enum
    {
        ulong result = Unsafe.As<TEnum, ulong>(ref value);
        return result;
        // if (BitConverter.IsLittleEndian)
        // {
        //     Span<ulong> spanAlloc = stackalloc ulong[] { 0UL };
        //     Span<TEnum> span = MemoryMarshal.Cast<ulong, TEnum>(spanAlloc);
        //
        //     span[0] = value;
        //
        //     result = spanAlloc[0];
        // }
        // else
        // {
        //     result = Unsafe.As<TEnum, ulong>(ref value);
        // }
        // return result;
    }
}