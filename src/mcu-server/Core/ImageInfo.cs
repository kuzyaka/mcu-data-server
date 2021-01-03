using System;

namespace McuServerApi.core
{
    public class ImageInfo
    {
        public string Base64Data { get; set; }
        public DateTime DateTime { get; set; }
        public string ClientName { get; set; }
    }
}