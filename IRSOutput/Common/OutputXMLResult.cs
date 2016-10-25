using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRSOutput.Models {
    public class OutputXMLResult {

        private string einfoID;
        public string EinfoID {
            get { return einfoID == null ? string.Empty : einfoID; }
            set { einfoID = value; }
        }

        public int ColumnName { get; set; }

        private string twelveMonth;
        public string TwelveMonth {
            get { return twelveMonth == null ? string.Empty : twelveMonth; }
            set { twelveMonth = value; }
        }

        private string jan;
        public string Jan {
            get { return jan == null ? string.Empty : jan; }
            set { jan = value; }
        }

        private string feb;
        public string Feb {
            get { return feb == null ? string.Empty : feb; }
            set { feb = value; }
        }

        private string mar;
        public string Mar {
            get { return mar == null ? string.Empty : mar; }
            set { mar = value; }
        }

        private string apr;
        public string Apr {
            get { return apr == null ? string.Empty : apr; }
            set { apr = value; }
        }

        private string may;
        public string May {
            get { return may == null ? string.Empty : may; }
            set { may = value; }
        }


        private string june;
        public string June {
            get { return june == null ? string.Empty : june; }
            set { june = value; }
        }


        private string july;
        public string July {
            get { return july == null ? string.Empty : july; }
            set { july = value; }
        }


        private string aug;
        public string Aug {
            get { return aug == null ? string.Empty : aug; }
            set { aug = value; }
        }


        private string sept;
        public string Sept {
            get { return sept == null ? string.Empty : sept; }
            set { sept = value; }
        }


        private string oct;
        public string Oct {
            get { return oct == null ? string.Empty : oct; }
            set { oct = value; }
        }



        private string nov;
        public string Nov {
            get { return nov == null ? string.Empty : nov; }
            set { nov = value; }
        }



        private string dec;
        public string Dec {
            get { return dec == null ? string.Empty : dec; }
            set { dec = value; }
        }

        public override bool Equals(object obj) {
            OutputXMLResult xml = obj as OutputXMLResult;
            if (xml == null) {
                return false;
            }
            return  TwelveMonth == xml.TwelveMonth
                && ColumnName == xml.ColumnName
                && Jan == xml.Jan
                && Feb == xml.Feb
                && Mar == xml.Mar
                && Apr == xml.Apr
                && May == xml.May
                && June == xml.June
                && July == xml.July
                && Aug == xml.Aug
                && Sept == xml.Sept
                && Oct == xml.Oct
                && Nov == xml.Nov
                && Dec == xml.Dec;
        }
    }
}