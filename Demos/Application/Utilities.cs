using Termule.Types.Content;
using Termule.Types.Vectors;

namespace Demos.Application;

internal static class Utilities
{
    public static Vector Perpendicular(this Vector v)
    {
        return (v.Y, -v.X);
    }

    public static Vector ScaleVelocity(Vector v)
    {
        if (v.Magnitude == 0)
        {
            return (0, 0);
        }

        var factor = MathF.Sqrt((v.X * v.X + v.Y * 0.5f * v.Y * 0.5f)
                                / (v.X * v.X + v.Y * v.Y));
        return v * factor;
    }

    public static Vector PointOnRectangle(Random random, Vector corner, Vector size)
    {
        var dist = (float)random.NextDouble() * (size.X * 2 + size.Y * 2);

        if (dist < size.X)
        {
            return (corner.X + dist, corner.Y);
        }
        else if (dist < size.X + size.Y)
        {
            return (corner.X + size.X, corner.Y + (dist - size.X));
        }
        else if (dist < size.X * 2 + size.Y)
        {
            return (corner.X + size.X - (dist - size.X - size.Y), corner.Y + size.Y);
        }
        else
        {
            return(corner.X, corner.Y + size.Y - (dist - 2 * size.X - size.Y));
        }
    }

    extension(Image image)
    {
        public Image Flipped()
        {
            Image flipped = new(image.Size.X, image.Size.Y);
            for (var x = 0; x < image.Size.X; x++)
            for (var y = 0; y < image.Size.Y; y++)
            {
                flipped[x, y] = image[image.Size.X - x - 1, y];
            }

            return flipped;
        }

        public Image WithColorSwapped(Color target, Color value)
        {
            Image swapped = new(image.Size.X, image.Size.Y);
            for (var x = 0; x < image.Size.X; x++)
            for (var y = 0; y < image.Size.Y; y++)
            {
                swapped[x, y] = image[x, y].Color == target ? image[x, y] with { Color = value } : image[x, y];
            }

            return swapped;
        }
    }
}