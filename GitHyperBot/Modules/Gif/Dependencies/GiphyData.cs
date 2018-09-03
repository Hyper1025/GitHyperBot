using System.Collections.Generic;

namespace GitHyperBot.Modules.Gif.Dependencies
{
    public class GiphyData
    {
        public List<Data> Data { get; set; }
    }

    public class Data
    {
        public Images Images { get; set; }
    }

    public class Images
    {
        public Original Original { get; set; }
    }

    public class Original
    {
        public string Url { get; set; }
    }
}