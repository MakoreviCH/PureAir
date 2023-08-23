using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyFinder.Model.Dto
{
    public class AuthInfo
    {
       public string? Token { get; set; }
       public bool Result { get; set; }
       public string[]? Errors { get; set; }
    }
}
