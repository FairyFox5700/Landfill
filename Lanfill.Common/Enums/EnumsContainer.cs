using System;
using System.Collections.Generic;
using System.Text;

namespace Landfill.Common.Enums
{
    public class EnumsContainer
    {
        public enum ContentType
        {
            FAQ,
            Announcement
        }

        public enum Language
        {
            UA,
            EN
        }
        public enum State
        {
            Modified,
            Published,
            Deleted
        }

    }
}
