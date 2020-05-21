using Landfill.BAL.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lanfill.BAL.Implementation.Mapping
{
    public class ExpressionMapper
    {
        private readonly IMappingModel mappingModel;

        public ExpressionMapper(IMappingModel mappingModel)
        {
            this.mappingModel = mappingModel;
        }
    }
}
