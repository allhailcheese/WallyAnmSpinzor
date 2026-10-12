using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WallyAnmSpinzor.Internal;

namespace WallyAnmSpinzor;

// Official name: AnimDef
public sealed class AnmClass
{
    public required string Index { get; set; }
    public required string FileName { get; set; }
    public required Dictionary<string, AnmAnimation> Animations { get; set; }

    internal static AnmClass CreateFrom(AnmVersionEnum version, DataReader reader)
    {
        string index = reader.ReadStr();
        string fileName = reader.ReadStr();

        uint animationCount = reader.ReadU32();
        Dictionary<string, AnmAnimation> animations = new((int)animationCount);
        for (uint i = 0; i < animationCount; ++i)
        {
            AnmAnimation animation = AnmAnimation.CreateFrom(version, reader);
            animations[animation.Name] = animation;
        }

        return new()
        {
            Index = index,
            FileName = fileName,
            Animations = animations,
        };
    }

    internal static async Task<AnmClass> CreateFromAsync(AnmVersionEnum version, DataReader reader, CancellationToken ctoken = default)
    {
        string index = await reader.ReadStrAsync(ctoken);
        string fileName = await reader.ReadStrAsync(ctoken);

        uint animationCount = await reader.ReadU32Async(ctoken);
        Dictionary<string, AnmAnimation> animations = new((int)animationCount);
        for (uint i = 0; i < animationCount; ++i)
        {
            AnmAnimation animation = await AnmAnimation.CreateFromAsync(version, reader, ctoken);
            animations[animation.Name] = animation;
        }

        return new()
        {
            Index = index,
            FileName = fileName,
            Animations = animations,
        };
    }

    internal void WriteTo(AnmVersionEnum version, DataWriter writer)
    {
        writer.WriteStr(Index);
        writer.WriteStr(FileName);
        writer.WriteU32((uint)Animations.Count);
        foreach ((_, AnmAnimation animation) in Animations)
        {
            animation.WriteTo(version, writer);
        }
    }

    internal async Task WriteToAsync(AnmVersionEnum version, DataWriter writer, CancellationToken ctoken = default)
    {
        await writer.WriteStrAsync(Index, ctoken);
        await writer.WriteStrAsync(FileName, ctoken);
        await writer.WriteU32Async((uint)Animations.Count, ctoken);
        foreach ((_, AnmAnimation animation) in Animations)
        {
            await animation.WriteToAsync(version, writer, ctoken);
        }
    }
}