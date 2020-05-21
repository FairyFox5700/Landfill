using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using static Landfill.Common.Enums.EnumsContainer;

namespace Landfill.BAL.Abstract
{
    public interface IJObjectConverter
    {
        dynamic GetModel(string jObject, ContentType contentType);///need to be derivered type but now dynamic
        dynamic GetModel(JObject jObject, ContentType contentType);

    }

}
