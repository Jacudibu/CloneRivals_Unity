using UnityEngine;

namespace GearConfigurator
{
    public class MouseOverData
    {
        public readonly string title;
        public readonly Sprite[] images;
        public readonly string text;

        public MouseOverData(string title, Sprite[] images, string text)
        {
            this.title = title;
            this.images = images;
            this.text = text;
        }
    }
}