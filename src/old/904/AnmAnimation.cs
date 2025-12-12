using System.Threading;
using System.Threading.Tasks;
using WallyAnmSpinzor.Internal;

namespace WallyAnmSpinzor.Version_904;

// Official name: AnimMove
public sealed class AnmAnimation_904
{
    public required string Name { get; set; }
    public required uint LoopStart { get; set; }
    public required uint RecoveryStart { get; set; }
    public required uint FreeStart { get; set; }
    public required uint PreviewFrame { get; set; }
    public required uint BaseStart { get; set; }
    public required uint[] RunEndFrames { get; set; }
    public required AnmFrame_904[] Frames { get; set; }

    internal static AnmAnimation_904 CreateFrom(DataReader reader)
    {
        string name = reader.ReadStr();
        uint frameCount = reader.ReadU32();
        uint loopStart = reader.ReadU32();
        uint recoveryStart = reader.ReadU32();
        uint freeStart = reader.ReadU32();
        uint previewFrame = reader.ReadU32();
        uint baseStart = reader.ReadU32();
        uint runEndFramesCount = reader.ReadU32();
        uint[] runEndFrames = new uint[runEndFramesCount];
        for (int i = 0; i < runEndFramesCount; ++i)
        {
            runEndFrames[i] = reader.ReadU32();
        }
        // this discarded value tells the game what's the size of the frames field.
        // this is used to be able to load the frames on-demand.
        _ = reader.ReadU32();
        AnmFrame_904[] frames = new AnmFrame_904[frameCount];
        for (int i = 0; i < frameCount; ++i)
        {
            AnmFrame_904? prevFrame = i == 0 ? null : frames[i - 1];
            frames[i] = AnmFrame_904.CreateFrom(reader, prevFrame);
        }

        return new()
        {
            Name = name,
            LoopStart = loopStart,
            RecoveryStart = recoveryStart,
            FreeStart = freeStart,
            PreviewFrame = previewFrame,
            BaseStart = baseStart,
            RunEndFrames = runEndFrames,
            Frames = frames,
        };
    }

    internal static async Task<AnmAnimation_904> CreateFromAsync(DataReader reader, CancellationToken ctoken = default)
    {
        string name = await reader.ReadStrAsync(ctoken);
        uint frameCount = await reader.ReadU32Async(ctoken);
        uint loopStart = await reader.ReadU32Async(ctoken);
        uint recoveryStart = await reader.ReadU32Async(ctoken);
        uint freeStart = await reader.ReadU32Async(ctoken);
        uint previewFrame = await reader.ReadU32Async(ctoken);
        uint baseStart = await reader.ReadU32Async(ctoken);
        uint runEndFramesCount = await reader.ReadU32Async(ctoken);
        uint[] runEndFrames = new uint[runEndFramesCount];
        for (int i = 0; i < runEndFramesCount; ++i)
        {
            runEndFrames[i] = await reader.ReadU32Async(ctoken);
        }
        // this discarded value tells the game what's the size of the frames field.
        // this is used to be able to load the frames on-demand.
        _ = await reader.ReadU32Async(ctoken);
        AnmFrame_904[] frames = new AnmFrame_904[frameCount];
        for (int i = 0; i < frameCount; ++i)
        {
            AnmFrame_904? prevFrame = i == 0 ? null : frames[i - 1];
            frames[i] = await AnmFrame_904.CreateFromAsync(reader, prevFrame, ctoken);
        }

        return new()
        {
            Name = name,
            LoopStart = loopStart,
            RecoveryStart = recoveryStart,
            FreeStart = freeStart,
            PreviewFrame = previewFrame,
            BaseStart = baseStart,
            RunEndFrames = runEndFrames,
            Frames = frames,
        };
    }

    internal void WriteTo(DataWriter writer)
    {
        writer.WriteStr(Name);
        writer.WriteU32((uint)Frames.Length);
        writer.WriteU32(LoopStart);
        writer.WriteU32(RecoveryStart);
        writer.WriteU32(FreeStart);
        writer.WriteU32(PreviewFrame);
        writer.WriteU32(BaseStart);
        writer.WriteU32((uint)RunEndFrames.Length);
        foreach (uint datum in RunEndFrames)
        {
            writer.WriteU32(datum);
        }
        writer.WriteU32(GetFramesByteCount());
        for (int i = 0; i < Frames.Length; ++i)
        {
            AnmFrame_904? prevFrame = i == 0 ? null : Frames[i - 1];
            Frames[i].WriteTo(writer, prevFrame);
        }
    }

    internal async Task WriteToAsync(DataWriter writer, CancellationToken ctoken = default)
    {
        await writer.WriteStrAsync(Name, ctoken);
        await writer.WriteU32Async((uint)Frames.Length, ctoken);
        await writer.WriteU32Async(LoopStart, ctoken);
        await writer.WriteU32Async(RecoveryStart, ctoken);
        await writer.WriteU32Async(FreeStart, ctoken);
        await writer.WriteU32Async(PreviewFrame, ctoken);
        await writer.WriteU32Async(BaseStart, ctoken);
        await writer.WriteU32Async((uint)RunEndFrames.Length, ctoken);
        foreach (uint datum in RunEndFrames)
        {
            await writer.WriteU32Async(datum, ctoken);
        }
        await writer.WriteU32Async(GetFramesByteCount(), ctoken);
        for (int i = 0; i < Frames.Length; ++i)
        {
            AnmFrame_904? prevFrame = i == 0 ? null : Frames[i - 1];
            await Frames[i].WriteToAsync(writer, prevFrame, ctoken);
        }
    }

    internal uint GetFramesByteCount()
    {
        uint size = 0;
        for (int i = 0; i < Frames.Length; ++i)
        {
            AnmFrame_904? prevFrame = i == 0 ? null : Frames[i - 1];
            size += Frames[i].GetByteCount(prevFrame);
        }
        return size;
    }
}