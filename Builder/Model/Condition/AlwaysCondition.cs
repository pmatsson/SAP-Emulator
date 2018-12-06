using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Builder.Model.Condition
{
    public class AlwaysCondition : ICondition
    {
        public string DisplayName => "Always";
    }
}
