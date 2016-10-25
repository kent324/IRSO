using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRSOutput.Common {

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public class DescriptionAttribute : Attribute {


        private string description;
        public string Description {
            get { return description; }
        }

        public DescriptionAttribute(String description) {
            this.description = description;
        }
    }
}

