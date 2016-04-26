using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolver
{
    class MathJax
    {
        public static string GetFormattedHtml(string innerHtml)
        {
            return string.Format(@"
            <!DOCTYPE html><html><head><meta http-equiv=""Content-Type"" content=""text/html; charset=UTF-8"" /><meta http-equiv=""X-UA-Compatible"" content=""IE=edge"" />
<meta name=""viewport"" content=""width=device-width, initial-scale=1""><script type=""text/x-mathjax-config"">MathJax.Hub.Config({{tex2jax: {{inlineMath: [[""$"",""$""],[""\\("",""\\)""]]}}}});</script><script type=""text/javascript"" src=""http://cdn.mathjax.org/mathjax/latest/MathJax.js?config=TeX-AMS_HTML-full""></script>
</head><body><p>{0}</p></body></html>", innerHtml);
        }
    }
}
