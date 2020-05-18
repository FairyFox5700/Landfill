using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using static Landfill.Common.Enums.EnumsContainer;

namespace Landfill.Models
{
    public class FaqModel
    {
        public int ContentId { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public State State { get; set; }
        public string MainTag { get; set; }
    }
    //public int Id { get; set; }
}
