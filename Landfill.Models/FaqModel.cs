﻿using Landfill.Entities;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json.Serialization;
using static Landfill.Common.Enums.EnumsContainer;

namespace Landfill.Models
{
    public class FaqModel
    {
        public int ContentId { get; set; }
        //[JsonConverter(typeof(StringEnumConverter))]
        //public ContentType ContentType { get; set; }
        //[JsonConverter(typeof(StringEnumConverter))]
        //public State State { get; set; }
        public string MainTag { get; set; }

        public static Expression<Func<FaqModel, FAQ>> ConvertToFaqEntity
        {
            get
            {
                return x => new FAQ()
                {
                    Id = x.ContentId,
                    ContentId = x.ContentId,
                    Tag = x.MainTag,
                };
            }
        }
    }
    //public int Id { get; set; }
}
