using System.Drawing;
using System.Collections.Generic;
namespace SharpPreloaders.FrameBuilders
{
    abstract class FrameBuilder
    {
        protected List<Image> images;
        protected Size imageSize;
        protected Color backColor;
        protected ImageBoundry boundry;

        public abstract List<Image> GetImages(Size imageSize, Color backColor, ImageBoundry boundry);
    }
}
