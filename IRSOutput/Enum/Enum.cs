using IRSOutput.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRSOutput {    
    public enum Code16 {
        [Description("2A")]
        A2,
        [Description("2D")]
        D2,
        [Description("2C")]
        C2
    }
    /// <summary>
    /// 分别统计Column C 和Text18\20
    /// </summary>
    enum EnumIRSTotalNumber {
        ColumnC,
        Text18_20
    }
}
