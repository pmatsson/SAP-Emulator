using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using MQChatter.Common;
using MQChatter.ViewModel.RuleGroup.Action;

namespace MQChatter.Model.Action
{
    public class NoAction : ActionBase
    {
        public override string DisplayName => "No action";

        protected override bool ProcessAction(XmlDocument doc, int ruleProcessCount, ActionGroup actionGroup)
        {
            return true;
        }
    }
}
