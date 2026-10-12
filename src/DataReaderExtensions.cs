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
}