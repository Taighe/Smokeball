using System;

namespace Project.Models
{
    public class UrlModel
    {
        public readonly int Index;
        public readonly string Url;

        public UrlModel(int index, string url)
        {
            Index = index;
            Url = url;
        }
    }
}
