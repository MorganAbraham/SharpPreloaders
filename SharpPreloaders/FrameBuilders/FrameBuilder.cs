using System.Drawing;

namespace SharpPreloaders.FrameBuilders
{
    abstract class FrameBuilder
    {
        public abstract Image[] GetImages(Size imageSize);
    }
}
