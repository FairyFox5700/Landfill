﻿using Landfill.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using static Landfill.Common.Enums.EnumsContainer;

namespace Landfill.Models
{
    public class AnnouncementModel
    {
        public int ContentId { get; set; }
        //[JsonConverter(typeof(StringEnumConverter))]
        //public State State { get; set; }
        //public ContentType ContentType { get; set; }
        public string Header { get; set; }
        public DateTime ValiUntil { get; set; }

        public static Expression<Func<AnnouncementModel, Announcement>> ConvertToAnnouncementEntity
        {
            get
            {
                return x => new Announcement()
                {
                    Id = x.ContentId,
                    ContentId = x.ContentId,
                    Header = x.Header,
                    ValiUntil = x.ValiUntil,
                };
            }
        }
    }
}
