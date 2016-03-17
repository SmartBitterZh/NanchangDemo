using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace SpringIoc
{
    public class SecurityAdvice : Spring.Aop.IMethodBeforeAdvice
    {
        #region IMethodBeforeAdvice 成员

        public void Before(System.Reflection.MethodInfo method, object[] args, object target)
        {
            if (HttpContext.Current.Session["customId"] == null)
            {
                HttpContext.Current.Response.Redirect("/");
            }
        }

        #endregion
    }
}
