public static class Tmatrix
{
    public static PointF matrixRotate(float angleDeg, PointF p, PointF pivot)
    {
        float rad = angleDeg * (float)Math.PI / 180f;
        float cos = (float)Math.Cos(rad);
        float sin = (float)Math.Sin(rad);

        float tx = p.X - pivot.X;
        float ty = p.Y - pivot.Y;

        float rx = (tx * cos) - (ty * sin);
        float ry = (tx * sin) + (ty * cos);

        return new PointF(rx + pivot.X, ry + pivot.Y);
    }

    public static PointF[] matrixRotate(float angleDeg, PointF[] points, PointF pivot)
    {
        PointF[] rotated = new PointF[points.Length];
        for (int i = 0; i < points.Length; i++)
            rotated[i] = matrixRotate(angleDeg, points[i], pivot);

        return rotated;
    }
}