using IRSOutput.DAL;
using IRSOutput.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRSOutput {
    /// <summary>
    /// 上下文
    /// </summary>
    /// 
    public class Context {
        private Dictionary<string, string> Input;
        private Dictionary<string, string> Inner;
        private Dictionary<string, string> Output;

        public string Code14 { get; set; }
        public string Code15 { get; set; }

        public static List<HealthbillingKeyValue> HealthbillingList;


        #region 构造函数
        public Context(int year) {
            this.Input = new Dictionary<string, string>();
            this.Inner = new Dictionary<string, string>();
            this.Output = new Dictionary<string, string>();
            Context.HealthbillingList = IRSQueryDB.loadHealthbilling(year);
            Code14 = IRSQueryDB.Code14(5);
            Code15 = IRSQueryDB.Code15(year);
            this.SetInner(IRSQueryDB.ALE1095CSoure());            
        }

        public Context(Dictionary<string, string> input) {
            this.Input = input;
        }
        #endregion


        public void SetInner(List<HealthbillingKeyValue> inner, Action<List<HealthbillingKeyValue>> action) {
            action(inner);
        }
        public void SetInnerText14(List<HealthbillingKeyValue> inner) {
            foreach (var i in inner) {
                if (!this.Inner.ContainsKey(i.TextField14)) {
                    this.Inner.Add(i.TextField14, this.Code14);
                }
            }
        }
        public void SetInnerText15(List<HealthbillingKeyValue> inner) {
            foreach (var i in inner) {
                if (!this.Inner.ContainsKey(i.TextField15)) {
                    this.Inner.Add(i.TextField15, this.Code15);
                }
            }
        }
        public void SetInnerText15Compare(List<HealthbillingKeyValue> inner) {
            foreach (var i in inner) {
                if (!this.Inner.ContainsKey(i.TextField15)) {
                    this.Inner.Add(i.TextField15, i.Value > 0 ? this.Code15 : "0.00");
                }
            }
        }
        public void SetInnerText16A2(List<HealthbillingKeyValue> inner) {
            foreach (var i in inner) {
                if (!this.Inner.ContainsKey(i.TextField16)) {
                    this.Inner.Add(i.TextField16, Code16.A2.GetDescription());
                }
            }
        }
        public void SetInnerText16Compare(List<HealthbillingKeyValue> inner) {
            foreach (var i in inner) {
                if (!this.Inner.ContainsKey(i.TextField16)) {
                    this.Inner.Add(i.TextField16, i.Value > 0 ? Code16.C2.GetDescription() : string.Empty);
                }
            }
        }

        public void SetInner(Dictionary<string, string> inner) {
            foreach (var i in inner) {
                if (!this.Inner.ContainsKey(i.Key)) {
                    this.Inner.Add(i.Key, i.Value);
                }
            }
        }
        public void SetInner(string key, string value) {
            if (!this.Inner.ContainsKey(key)) {
                this.Inner.Add(key, value);
            }
        }

        public Dictionary<string, string> GetInner() {
            return this.Inner;
        }

        public Dictionary<string, string> GetResult() {
            if (Output.Count == 0)
                this.Output = this.Inner;
            return Output;
        }
        public List<OutputXMLResult> getXMLResult() {
            List<OutputXMLResult> listResult = new List<OutputXMLResult>();
            var output = this.GetResult();
            var n4 = Common.Common.findRecordByNum(output, 14, 26);
            var n5 = Common.Common.findRecordByNum(output, 27, 39);
            var n6 = Common.Common.findRecordByNum(output, 40, 52);
            

            listResult.Add(new OutputXMLResult() {
                ColumnName = 14,                
                TwelveMonth = Common.Common.GR(n4, 14),
                Jan = Common.Common.GR(n4, 15),
                Feb = Common.Common.GR(n4, 16),
                Mar = Common.Common.GR(n4, 17),
                Apr = Common.Common.GR(n4, 18),
                May = Common.Common.GR(n4, 19),
                June = Common.Common.GR(n4, 20),
                July = Common.Common.GR(n4, 21),
                Aug = Common.Common.GR(n4, 22),
                Sept = Common.Common.GR(n4, 23),
                Oct = Common.Common.GR(n4, 24),
                Nov = Common.Common.GR(n4, 25),
                Dec = Common.Common.GR(n4, 26)
            });
            listResult.Add(new OutputXMLResult() {
                ColumnName = 15,
                TwelveMonth = Common.Common.GR(n5, 27),
                Jan = Common.Common.GR(n5, 28),
                Feb = Common.Common.GR(n5, 29),
                Mar = Common.Common.GR(n5, 30),
                Apr = Common.Common.GR(n5, 31),
                May = Common.Common.GR(n5, 32),
                June = Common.Common.GR(n5, 33),
                July = Common.Common.GR(n5, 34),
                Aug = Common.Common.GR(n5, 35),
                Sept = Common.Common.GR(n5, 36),
                Oct = Common.Common.GR(n5, 37),
                Nov = Common.Common.GR(n5, 38),
                Dec = Common.Common.GR(n5, 39)
            });
            listResult.Add(new OutputXMLResult() {
                ColumnName = 16,
                TwelveMonth = Common.Common.GR(n6, 40),
                Jan = Common.Common.GR(n6, 41),
                Feb = Common.Common.GR(n6, 42),
                Mar = Common.Common.GR(n6, 43),
                Apr = Common.Common.GR(n6, 44),
                May = Common.Common.GR(n6, 45),
                June = Common.Common.GR(n6, 46),
                July = Common.Common.GR(n6, 47),
                Aug = Common.Common.GR(n6, 48),
                Sept = Common.Common.GR(n6, 49),
                Oct = Common.Common.GR(n6, 50),
                Nov = Common.Common.GR(n6, 51),
                Dec = Common.Common.GR(n6, 52),
            });
            return listResult;
        }
    }
}