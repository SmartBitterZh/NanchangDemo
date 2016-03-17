using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YYT.Validation
{
  internal interface IAsyncRuleMethod : IRuleMethod
  {
    AsyncRuleArgs AsyncRuleArgs { get; }

    RuleSeverity Severity { get; }

    void Invoke(object target, AsyncRuleCompleteHandler complete);
  }
}
