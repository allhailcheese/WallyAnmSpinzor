using System;
using System.Threading;
using System.Threading.Tasks;
using WallyAnmSpinzor.Internal;

namespace WallyAnmSpinzor;

// Official name: BoneDef
public sealed class AnmBone
{
    public required uint Id { get; set; }
    public required double ScaleX { get; set; }
    public required double RotateSkew0 { get; set; }
    public required double RotateSkew1 { get; set; }
    public required double ScaleY { get; set; }
    public required double X { get; set; }
    public required double Y { get; set; }
    public required double Opacity { get; set; }
    public required short Frame { get; set; }

    internal static AnmBone CreateFrom(AnmVersionEnum version, DataReader reader, AnmBone? prev)
    {
        uint id = (uint)reader.ReadI16();
        bool opaque = version >= AnmVersionEnum.From703To704 && reader.ReadBool();

        double scaleX, rotateSkew0, rotateSkew1, scaleY, x, y;
        // bmg why did you swap the order
        if (version >= AnmVersionEnum.From703To704)
        {
            (scaleX, rotateSkew0, rotateSkew1, scaleY) = ReadTransform(version, reader, prev);
            (x, y) = ReadPosition(version, reader, prev);
        }
        else
        {
            (x, y) = ReadPosition(version, reader, prev);
            (scaleX, rotateSkew0, rotateSkew1, scaleY) = ReadTransform(version, reader, prev);
        }

        short frame = 1;
        if (version < AnmVersionEnum.From904ToCurrent || reader.ReadBool())
            frame = reader.ReadBoneFrame(version);

        double opacity = 1.0;
        if (!opaque)
            opacity = reader.ReadBoneOpacity(version);

        return new()
        {
            Id = id,
            ScaleX = scaleX,
            RotateSkew0 = rotateSkew0,
            RotateSkew1 = rotateSkew1,
            ScaleY = scaleY,
            X = x,
            Y = y,
            Opacity = opacity,
            Frame = frame,
        };
    }

    internal static (double scaleX, double rotateSkew0, double rotateSkew1, double scaleY) ReadTransform(AnmVersionEnum version, DataReader reader, AnmBone? prev)
    {
        double scaleX, rotateSkew0, rotateSkew1, scaleY;
        // copy transform from prev
        if (version >= AnmVersionEnum.From904ToCurrent && reader.ReadBool())
        {
            if (prev is null)
                throw new Exception("Bone copies transform from previous, but there is no previous bone");
            scaleX = prev.ScaleX;
            rotateSkew0 = prev.RotateSkew0;
            rotateSkew1 = prev.RotateSkew1;
            scaleY = prev.ScaleY;
        }
        else
        {
            bool identity = false;
            bool symmetric = false;
            if (version >= AnmVersionEnum.From703To704 && reader.ReadBool())
            {
                if (reader.ReadBool()) identity = true;
                else symmetric = true;
            }

            if (identity)
            {
                scaleX = scaleY = 1;
                rotateSkew0 = rotateSkew1 = 0;
            }
            else
            {
                scaleX = reader.ReadBoneTransform(version);
                rotateSkew0 = reader.ReadBoneTransform(version);
                if (symmetric)
                {
                    rotateSkew1 = rotateSkew0;
                    scaleY = -scaleX;
                }
                else
                {
                    rotateSkew1 = reader.ReadBoneTransform(version);
                    scaleY = reader.ReadBoneTransform(version);
                }
            }
        }
        return (scaleX, rotateSkew0, rotateSkew1, scaleY);
    }

    internal static (double x, double y) ReadPosition(AnmVersionEnum version, DataReader reader, AnmBone? prev)
    {
        double x, y;
        // copy position from prev
        if (version >= AnmVersionEnum.From904ToCurrent && reader.ReadBool())
        {
            if (prev is null)
                throw new Exception("Bone copies position from previous, but there is no previous bone");
            x = prev.X;
            y = prev.Y;
        }
        else
        {
            x = reader.ReadBoneTransform(version);
            y = reader.ReadBoneTransform(version);
        }
        return (x, y);
    }

    internal static async Task<AnmBone> CreateFromAsync(AnmVersionEnum version, DataReader reader, AnmBone? prev, CancellationToken ctoken = default)
    {
        uint id = (uint)await reader.ReadI16Async(ctoken);
        bool opaque = version >= AnmVersionEnum.From703To704 && await reader.ReadBoolAsync(ctoken);

        double scaleX, rotateSkew0, rotateSkew1, scaleY, x, y;
        // bmg why did you swap the order
        if (version >= AnmVersionEnum.From703To704)
        {
            (scaleX, rotateSkew0, rotateSkew1, scaleY) = await ReadTransformAsync(version, reader, prev, ctoken);
            (x, y) = await ReadPositionAsync(version, reader, prev, ctoken);
        }
        else
        {
            (x, y) = await ReadPositionAsync(version, reader, prev, ctoken);
            (scaleX, rotateSkew0, rotateSkew1, scaleY) = await ReadTransformAsync(version, reader, prev, ctoken);
        }

        short frame = 1;
        if (version < AnmVersionEnum.From904ToCurrent || await reader.ReadBoolAsync(ctoken))
            frame = await reader.ReadBoneFrameAsync(version, ctoken);

        double opacity = 1.0;
        if (!opaque)
            opacity = await reader.ReadBoneOpacityAsync(version, ctoken);

        return new()
        {
            Id = id,
            ScaleX = scaleX,
            RotateSkew0 = rotateSkew0,
            RotateSkew1 = rotateSkew1,
            ScaleY = scaleY,
            X = x,
            Y = y,
            Opacity = opacity,
            Frame = frame,
        };
    }

    internal static async Task<(double scaleX, double rotateSkew0, double rotateSkew1, double scaleY)> ReadTransformAsync(AnmVersionEnum version, DataReader reader, AnmBone? prev, CancellationToken ctoken = default)
    {
        double scaleX, rotateSkew0, rotateSkew1, scaleY;
        // copy transform from prev
        if (version >= AnmVersionEnum.From904ToCurrent && await reader.ReadBoolAsync(ctoken))
        {
            if (prev is null)
                throw new Exception("Bone copies transform from previous, but there is no previous bone");
            scaleX = prev.ScaleX;
            rotateSkew0 = prev.RotateSkew0;
            rotateSkew1 = prev.RotateSkew1;
            scaleY = prev.ScaleY;
        }
        else
        {
            bool identity = false;
            bool symmetric = false;
            if (version >= AnmVersionEnum.From703To704 && await reader.ReadBoolAsync(ctoken))
            {
                if (await reader.ReadBoolAsync(ctoken)) identity = true;
                else symmetric = true;
            }

            if (identity)
            {
                scaleX = scaleY = 1;
                rotateSkew0 = rotateSkew1 = 0;
            }
            else
            {
                scaleX = await reader.ReadBoneTransformAsync(version, ctoken);
                rotateSkew0 = await reader.ReadBoneTransformAsync(version, ctoken);
                if (symmetric)
                {
                    rotateSkew1 = rotateSkew0;
                    scaleY = -scaleX;
                }
                else
                {
                    rotateSkew1 = await reader.ReadBoneTransformAsync(version, ctoken);
                    scaleY = await reader.ReadBoneTransformAsync(version, ctoken);
                }
            }
        }
        return (scaleX, rotateSkew0, rotateSkew1, scaleY);
    }

    internal static async Task<(double x, double y)> ReadPositionAsync(AnmVersionEnum version, DataReader reader, AnmBone? prev, CancellationToken ctoken = default)
    {
        double x, y;
        // copy position from prev
        if (version >= AnmVersionEnum.From904ToCurrent && await reader.ReadBoolAsync(ctoken))
        {
            if (prev is null)
                throw new Exception("Bone copies position from previous, but there is no previous bone");
            x = prev.X;
            y = prev.Y;
        }
        else
        {
            x = await reader.ReadBoneTransformAsync(version, ctoken);
            y = await reader.ReadBoneTransformAsync(version, ctoken);
        }
        return (x, y);
    }

    internal void WriteTo(AnmVersionEnum version, DataWriter writer, AnmBone? prev)
    {
        writer.WriteI16((short)Id);
        if (version >= AnmVersionEnum.From703To704)
            writer.WriteBool(Opacity == 1);

        // bmg why did you swap the order
        if (version >= AnmVersionEnum.From703To704)
        {
            WriteTransform(version, writer, prev);
            WritePosition(version, writer, prev);
        }
        else
        {
            WritePosition(version, writer, prev);
            WriteTransform(version, writer, prev);
        }

        bool canDefaultFrame = Frame == 1;
        if (version >= AnmVersionEnum.From904ToCurrent)
            writer.WriteBool(canDefaultFrame);
        if (version < AnmVersionEnum.From904ToCurrent || !canDefaultFrame)
            writer.WriteBoneFrame(version, Frame);

        if (version < AnmVersionEnum.From703To704 || Opacity != 1)
            writer.WriteBoneOpacity(version, Opacity);
    }

    internal void WriteTransform(AnmVersionEnum version, DataWriter writer, AnmBone? prev)
    {
        bool canCopyTransform = prev is not null && HasSameTransformAs(prev);
        if (version >= AnmVersionEnum.From904ToCurrent)
            writer.WriteBool(canCopyTransform);
        if (version < AnmVersionEnum.From904ToCurrent || !canCopyTransform)
        {
            bool identity = version >= AnmVersionEnum.From703To704 && IsIdentity;
            bool symmetric = version >= AnmVersionEnum.From703To704 && IsSymmetric;
            if (version >= AnmVersionEnum.From703To704)
            {
                writer.WriteBool(identity || symmetric);
                if (identity || symmetric)
                    writer.WriteBool(identity);
            }

            if (!identity)
            {
                writer.WriteBoneTransform(version, ScaleX);
                writer.WriteBoneTransform(version, RotateSkew0);
                if (!symmetric)
                {
                    writer.WriteBoneTransform(version, RotateSkew1);
                    writer.WriteBoneTransform(version, ScaleY);
                }
            }
        }
    }

    internal void WritePosition(AnmVersionEnum version, DataWriter writer, AnmBone? prev)
    {
        bool canCopyPosition = prev is not null && HasSamePositionAs(prev);
        if (version >= AnmVersionEnum.From904ToCurrent)
            writer.WriteBool(canCopyPosition);
        if (version < AnmVersionEnum.From904ToCurrent || !canCopyPosition)
        {
            writer.WriteBoneTransform(version, X);
            writer.WriteBoneTransform(version, Y);
        }
    }

    internal async Task WriteToAsync(AnmVersionEnum version, DataWriter writer, AnmBone? prev, CancellationToken ctoken = default)
    {
        await writer.WriteI16Async((short)Id, ctoken);
        if (version >= AnmVersionEnum.From703To704)
            await writer.WriteBoolAsync(Opacity == 1, ctoken);

        // bmg why did you swap the order
        if (version >= AnmVersionEnum.From703To704)
        {
            await WriteTransformAsync(version, writer, prev, ctoken);
            await WritePositionAsync(version, writer, prev, ctoken);
        }
        else
        {
            await WritePositionAsync(version, writer, prev, ctoken);
            await WriteTransformAsync(version, writer, prev, ctoken);
        }

        bool canDefaultFrame = Frame == 1;
        if (version >= AnmVersionEnum.From904ToCurrent)
            await writer.WriteBoolAsync(canDefaultFrame, ctoken);
        if (version < AnmVersionEnum.From904ToCurrent || !canDefaultFrame)
            await writer.WriteBoneFrameAsync(version, Frame, ctoken);

        if (version < AnmVersionEnum.From703To704 || Opacity != 1)
            await writer.WriteBoneOpacityAsync(version, Opacity, ctoken);
    }


    internal async Task WriteTransformAsync(AnmVersionEnum version, DataWriter writer, AnmBone? prev, CancellationToken ctoken = default)
    {
        bool canCopyTransform = prev is not null && HasSameTransformAs(prev);
        if (version >= AnmVersionEnum.From904ToCurrent)
            await writer.WriteBoolAsync(canCopyTransform, ctoken);
        if (version < AnmVersionEnum.From904ToCurrent || !canCopyTransform)
        {
            bool identity = version >= AnmVersionEnum.From703To704 && IsIdentity;
            bool symmetric = version >= AnmVersionEnum.From703To704 && IsSymmetric;
            if (version >= AnmVersionEnum.From703To704)
            {
                await writer.WriteBoolAsync(identity || symmetric, ctoken);
                if (identity || symmetric)
                    await writer.WriteBoolAsync(identity, ctoken);
            }

            if (!identity)
            {
                await writer.WriteBoneTransformAsync(version, ScaleX, ctoken);
                await writer.WriteBoneTransformAsync(version, RotateSkew0, ctoken);
                if (!symmetric)
                {
                    await writer.WriteBoneTransformAsync(version, RotateSkew1, ctoken);
                    await writer.WriteBoneTransformAsync(version, ScaleY, ctoken);
                }
            }
        }
    }

    internal async Task WritePositionAsync(AnmVersionEnum version, DataWriter writer, AnmBone? prev, CancellationToken ctoken = default)
    {
        bool canCopyPosition = prev is not null && HasSamePositionAs(prev);
        if (version >= AnmVersionEnum.From904ToCurrent)
            await writer.WriteBoolAsync(canCopyPosition, ctoken);
        if (version < AnmVersionEnum.From904ToCurrent || !canCopyPosition)
        {
            await writer.WriteBoneTransformAsync(version, X, ctoken);
            await writer.WriteBoneTransformAsync(version, Y, ctoken);
        }
    }

    internal uint GetByteCount(AnmVersionEnum version, AnmBone? prev)
    {
        uint size = 0;
        size += sizeof(ushort); // id
        if (version >= AnmVersionEnum.From703To704)
            size += sizeof(byte); // opaque

        // copy transform indicator
        if (version >= AnmVersionEnum.From904ToCurrent)
            size += sizeof(byte);
        // can't copy transform
        if (version < AnmVersionEnum.From904ToCurrent || prev is null || !HasSameTransformAs(prev))
        {
            if (version >= AnmVersionEnum.From703To704)
            {
                // identity/symmetric indicator
                size += sizeof(byte);
                // 2nd indicator
                if (IsIdentity || IsSymmetric)
                    size += sizeof(byte);
            }
            if (version < AnmVersionEnum.From703To704 || !IsIdentity)
            {
                size += GetBoneTransformByteSize(version); // scaleX
                size += GetBoneTransformByteSize(version); // rotateSkew0
                if (version < AnmVersionEnum.From703To704 || !IsSymmetric)
                {
                    size += GetBoneTransformByteSize(version); // rotateSkew1
                    size += GetBoneTransformByteSize(version); // scaleY
                }
            }
        }

        // copy position indicator
        if (version >= AnmVersionEnum.From904ToCurrent)
            size += sizeof(byte);
        // can't copy position
        if (version < AnmVersionEnum.From904ToCurrent || prev is null || !HasSamePositionAs(prev))
        {
            size += GetBoneTransformByteSize(version); // x
            size += GetBoneTransformByteSize(version); // y
        }

        // default frame indicator
        if (version >= AnmVersionEnum.From904ToCurrent)
            size += sizeof(byte);
        if (version < AnmVersionEnum.From904ToCurrent || Frame != 1)
            size += GetBoneFrameByteSize(version); // frame

        if (version < AnmVersionEnum.From703To704 || Opacity != 1)
            size += GetBoneOpacityByteSize(version); // opacity

        return size;
    }

    internal static uint GetBoneFrameByteSize(AnmVersionEnum version)
    {
        if (version >= AnmVersionEnum.From904ToCurrent)
        {
            return sizeof(sbyte);
        }
        else
        {
            return sizeof(short);
        }
    }

    internal static uint GetBoneTransformByteSize(AnmVersionEnum version)
    {
        if (version >= AnmVersionEnum.From710To904)
        {
            return sizeof(float);
        }
        else
        {
            return sizeof(double);
        }
    }

    internal static uint GetBoneOpacityByteSize(AnmVersionEnum version)
    {
        if (version >= AnmVersionEnum.From703To704)
        {
            return sizeof(byte);
        }
        else
        {
            return sizeof(double);
        }
    }

    internal AnmBone Clone() => new()
    {
        Id = Id,
        ScaleX = ScaleX,
        RotateSkew0 = RotateSkew0,
        RotateSkew1 = RotateSkew1,
        ScaleY = ScaleY,
        X = X,
        Y = Y,
        Opacity = Opacity,
        Frame = Frame,
    };

    internal bool IsPartialCloneOf(AnmBone bone) =>
        Id == bone.Id &&
        ScaleX == bone.ScaleX &&
        RotateSkew0 == bone.RotateSkew0 &&
        RotateSkew1 == bone.RotateSkew1 &&
        ScaleY == bone.ScaleY &&
        X == bone.X &&
        Y == bone.Y &&
        Opacity == bone.Opacity;

    internal bool HasSameTransformAs(AnmBone bone) =>
        ScaleX == bone.ScaleX &&
        RotateSkew0 == bone.RotateSkew0 &&
        RotateSkew1 == bone.RotateSkew1 &&
        ScaleY == bone.ScaleY;

    internal bool HasSamePositionAs(AnmBone bone) =>
        X == bone.X && Y == bone.Y;

    internal bool IsIdentity => ScaleX == 1 && RotateSkew0 == 0 && RotateSkew1 == 0 && ScaleY == 1;
    internal bool IsSymmetric => ScaleY == -ScaleX && RotateSkew0 == RotateSkew1;
}