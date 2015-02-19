using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WESNet.Utilities.ExpressionEvaluator
{
    public class Constants
    {
        public const int CaptionWidth = 20;

        public static string[] SupportedTypes = {"System.String", "System.DateTime", "System.Int32", "System.Double", "System.Boolean"};

        public static int StringSupportedTypeIndex = Array.IndexOf<string>(SupportedTypes, "System.String");

        public static string[,] TypeMatrix =  {{"System.String",   null,               null,              null,               null                },
                                               {null,              "System.DateTime",  null,              null,               null                },
                                               {null,              null,               "System.Int32",    "System.Double",    null                },
                                               {null,              null,               "System.Double",   "System.Double",    null                },
                                               {null,              null,               null,              null,               "System.Boolean"    }};

        public static string[] Usings = {"System.Int32", "System.String", "System.Double", "System.DateTime", "System.Math"};

    }
}
