using System.Threading;
using System.Threading.Tasks;
using WallyAnmSpinzor.Internal;

namespace WallyAnmSpinzor;

internal static class DataReaderExtensions
{
    internal static short ReadBoneFrame(this DataReader reader, AnmVersionEnum version)
    {
        if (version >= AnmVersionEnum.From904ToCurrent)
        {
            return reader.ReadI8();
        }
        else
        {
            return reader.ReadI16();
        }
    }

    internal static async ValueTask<short> ReadBoneFrameAsync(this DataReader reader, AnmVersionEnum version, CancellationToken ctoken)
    {
        if (version >= AnmVersionEnum.From904ToCurrent)
        {
            return await reader.ReadI8Async(ctoken);
        }
        else
        {
            return await reader.ReadI16Async(ctoken);
        }
    }

    internal static double ReadBoneTransform(this DataReader reader, AnmVersionEnum version)
    {
        if (version >= AnmVersionEnum.From710To904)
        {
            return reader.ReadF32();
        }
        else
        {
            return reader.ReadF64();
        }
    }

    internal static async ValueTask<double> ReadBoneTransformAsync(this DataReader reader, AnmVersionEnum version, CancellationToken ctoken)
    {
        if (version >= AnmVersionEnum.From710To904)
        {
            return await reader.ReadF32Async(ctoken);
        }
        else
        {
            return await reader.ReadF64Async(ctoken);
        }
    }

    internal static uint ReadFrameId(this DataReader reader, AnmVersionEnum version)
    {
        if (version >= AnmVersionEnum.From703To704)
        {
            return (uint)reader.ReadI16();
        }
        else
        {
            return reader.ReadU32();
        }
    }

    internal static async ValueTask<uint> ReadFrameIdAsync(this DataReader reader, AnmVersionEnum version, CancellationToken ctoken)
    {
        if (version >= AnmVersionEnum.From703To704)
        {
            return (uint)await reader.ReadI16Async(ctoken);
        }
        else
        {
            return await reader.ReadU32Async(ctoken);
        }
    }

    internal static int ReadBoneCount(this DataReader reader, AnmVersionEnum version)
    {
        if (version >= AnmVersionEnum.From703To704)
        {
            return reader.ReadI16();
        }
        else
        {
            return (int)reader.ReadU32();
        }
    }

    internal static async ValueTask<int> ReadBoneCountAsync(this DataReader reader, AnmVersionEnum version, CancellationToken ctoken)
    {
        if (version >= AnmVersionEnum.From703To704)
        {
            return await reader.ReadI16Async(ctoken);
        }
        else
        {
            return (int)await reader.ReadU32Async(ctoken);
        }
    }

    internal static double ReadBoneOpacity(this DataReader reader, AnmVersionEnum version)
    {
        if (version >= AnmVersionEnum.From703To704)
        {
            return reader.ReadU8() / 255.0;
        }
        else
        {
            return reader.ReadF64();
        }
    }

    internal static async ValueTask<double> ReadBoneOpacityAsync(this DataReader reader, AnmVersionEnum version, CancellationToken ctoken)
    {
        if (version >= AnmVersionEnum.From703To704)
        {
            return await reader.ReadU8Async(ctoken) / 255.0;
        }
        else
        {
            return await reader.ReadF64Async(ctoken);
        }
    }
}