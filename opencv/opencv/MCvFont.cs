using Emgu.CV.CvEnum;

internal class MCvFont
{
    private FontFace hersheyComplex;
    private double v1;
    private double v2;

    public MCvFont(FontFace hersheyComplex, double v1, double v2)
    {
        this.hersheyComplex = hersheyComplex;
        this.v1 = v1;
        this.v2 = v2;
    }
}