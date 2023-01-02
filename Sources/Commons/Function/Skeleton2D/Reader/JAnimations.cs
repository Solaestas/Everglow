using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Sources.Commons.Function.Skeleton2D.Reader
{
    /// <summary>
    /// Json: 一个Animation包含一个时间轴列表. 每条时间轴均存储了一个关键帧列表, 这些时间轴描述了骨骼或槽位的值如何随时间变化.
    /// </summary>
    internal class J_Animation
    {
        [JsonProperty(PropertyName = "bones", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public Dictionary<string, JObject> Bones;

        [JsonProperty(PropertyName = "slots", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public Dictionary<string, JObject> Slots;
    }

}
