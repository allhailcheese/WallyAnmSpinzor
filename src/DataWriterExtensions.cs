using System.Threading;
using System.Threading.Tasks;
using WallyAnmSpinzor.Internal;

namespace WallyAnmSpinzor;

internal static class DataWriterExtensions
{
    internal static void WriteBoneFrame(this DataWriter writer, AnmVersionEnum version, short value)
    {
        if (version >= AnmVersionEnum.From904ToCurrent)
        {
            writer.WriteI8((sbyte)value);
        }
        else
        {
            writer.WriteI16(value);
        }
    }

    internal static async ValueTask WriteBoneFrameAsync(this DataWriter writer, AnmVersionEnum version, short value, CancellationToken ctoken = default)
    {
        if (version >= AnmVersionEnum.From904ToCurrent)
        {
            await writer.WriteI8Async((sbyte)value, ctoken);
        }
        else
        {
            await writer.WriteI16Async(value, ctoken);
        }
    }

    internal static void WriteBoneTransform(this DataWriter writer, AnmVersionEnum version, double value)
    {
        if (version >= AnmVersionEnum.From710To904)
        {
            writer.WriteF32((float)value);
        }
        else
        {
            writer.WriteF64(value);
        }
    }

    internal static async ValueTask WriteBoneTransformAsync(this DataWriter writer, AnmVersionEnum version, double value, CancellationToken ctoken = default)
    {
        if (version >= AnmVersionEnum.From710To904)
        {
            await writer.WriteF32Async((float)value, ctoken);
        }
        else
        {
            await writer.WriteF64Async(value, ctoken);
        }
    }

    internal static void WriteFrameId(this DataWriter writer, AnmVersionEnum version, uint value)
    {
        if (version >= AnmVersionEnum.From703To704)
        {
            writer.WriteI16((short)value);
        }
        else
        {
            writer.WriteU32(value);
        }
    }

    internal static async ValueTask WriteFrameIdAsync(this DataWriter writer, AnmVersionEnum version, uint value, CancellationToken ctoken = default)
    {
        if (version >= AnmVersionEnum.From703To704)
        {
            await writer.WriteI16Async((short)value, ctoken);
        }
        else
        {
            await writer.WriteU32Async(value, ctoken);
        }
    }

    internal static void WriteBoneCount(this DataWriter writer, AnmVersionEnum version, int value)
    {
        if (version >= AnmVersionEnum.From703To704)
        {
            writer.WriteI16((short)value);
        }
        else
        {
            writer.WriteU32((uint)value);
        }
    }

    internal static async ValueTask WriteBoneCountAsync(this DataWriter writer, AnmVersionEnum version, int value, CancellationToken ctoken = default)
    {
        if (version >= AnmVersionEnum.From703To704)
        {
            await writer.WriteI16Async((short)value, ctoken);
        }
        else
        {
            await writer.WriteU32Async((uint)value, ctoken);
        }
    }

    internal static void WriteBoneOpacity(this DataWriter writer, AnmVersionEnum version, double value)
    {
        if (version >= AnmVersionEnum.From703To704)
        {
            writer.WriteU8((byte)(value * 255));
        }
        else
        {
            writer.WriteF64(value);
        }
    }

    internal static async ValueTask WriteBoneOpacityAsync(this DataWriter writer, AnmVersionEnum version, double value, CancellationToken ctoken = default)
    {
        if (version >= AnmVersionEnum.From703To704)
        {
            await writer.WriteU8Async((byte)(value * 255), ctoken);
        }
        else
        {
            await writer.WriteF64Async(value, ctoken);
        }
    }
}