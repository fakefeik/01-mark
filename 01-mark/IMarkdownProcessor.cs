using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01_mark
{
    interface IMarkdownProcessor
    {
        string ToHtml(string text);
    }
}
