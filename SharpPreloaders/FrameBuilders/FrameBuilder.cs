using System.Drawing;
using System.Collections.Generic;
namespace SharpPreloaders.FrameBuilders
{
    abstract class FrameBuilder
    {
        protected List<Image> images;
        protected Size imageSize;

        public abstract List<Image> GetImages(Size imageSize);
    }
}
