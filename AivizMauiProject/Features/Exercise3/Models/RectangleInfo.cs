namespace AivizMauiProject.Features.Exercise3.Models
{
    /// <summary>
    /// Information about a rectangle drawn on the image
    /// </summary>
    public class RectangleInfo
    {
        public string? Name { get; set; } // For example, "R1"
        public double X { get; set; }     // X coordinate
        public double Y { get; set; }     // Y coordinate
        public double W { get; set; }     // Width
        public double H { get; set; }     // Height
    }
}
