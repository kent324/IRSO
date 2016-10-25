using IRSOutput.DAL;
using IRSOutput.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRSOutput {
    /// <summary>
    /// 1094C处理的上下文
    /// </summary>
    /// 
    public class C1904Factory {
        private Dictionary<string, string> Input;
        private static Dictionary<string, string> Output;

        #region 构造函数
        private static void Init(int Year) {
            if (Output == null) {
                Output = new Dictionary<string, string>();
                Dictionary<string, string> input = new Dictionary<string, string>();

                SetInner(IRSQueryDB.ALE1094CSoure());

                input.Add("Check1", "false");
                input.Add("Check2", "false");

                DateTime dtStart = new DateTime(Year + 1, 1, 1).AddDays(-1);
                DateTime dtEnd = new DateTime(Year, 1, 1);

                /*Please double check your formula for calculating 1094C items 18 and 20 to make sure it captures/include all employees in active status for at least one day of the selected year (e.g. 2015).*/
                var employees = DBSourceFactory.GetDBSource(Year);

                string sigDate = IRSQueryDB.Code15Date(Year);
                if (!string.IsNullOrEmpty(sigDate)) {
                    input.Add("Text21", sigDate);
                }

                input.Add("Text17", employees.Count.ToString());
                input.Add("Check3", "true");
                input.Add("Check4", "false");
                input.Add("Text18", employees.Count.ToString());
                input.Add("Check5", "true");
                input.Add("Check9", "true");//计算B列             
                input.Add("Check10", "true");


                /*如果当前月为7月,则往前推一年再+1个月, 再-1天可以得到2014/7/1 - 2014/7/31 */
                //DateTime startDate = new DateTime(DateTime.Now.AddYears(-1).Year, DateTime.Now.AddYears(-1).Month + 1, 1);
                //DateTime endDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

                //DateTime startDate = new DateTime()

                DateTime dtTemp = dtEnd;
                int hire = 0;
                //int terminate = 0;
                Dictionary<int, int> ColumnB = new Dictionary<int, int>();
                Dictionary<int, int> ColumnC = new Dictionary<int, int>();
                while (dtTemp <= dtStart) {
                    /*Calculate total number of Regular Full Time”, “Regular Part Time” and “Part Time 30 HRs for each month of the selected Year and enter values*/
                    string strSQL = string.Format("SELECT count(*) FROM Employment_Info WHERE employee_type IN ('001','002','003')  AND date_entry_to_agency < '{0}' AND employment_status=1", dtTemp);
                    hire = int.Parse(SqlHelperIRS.ExecuteScalarToStr(System.Data.CommandType.Text, strSQL));
                    ColumnB.Add(dtTemp.Month, hire);

                    /*Count ALL Employees including those terminated for the selected year. (count Regular Full Time”, “Regular Part Time” , “Part Time 30 HRs, Part time 20 hrs and substitutes)*/
                    //strSQL = string.Format("SELECT count(*) FROM (SELECT DISTINCT Employee_ID FROM Employment_Info WHERE date_terminate_from_agency BETWEEN '{0}' AND  '{1}' )A", dtTemp.AddMonths(-1), dtTemp.AddDays(-1));
                    //terminate = int.Parse(SqlHelper.ExecuteScalarToStr(CommandType.Text, strSQL));
                    //ColumnC.Add(dtTemp.Month, terminate);
                    var tmp = IRSQueryDB.loadAllEmployees("", dtTemp.AddMonths(1).AddDays(-1), dtTemp, EnumIRSTotalNumber.ColumnC);
                    ColumnC.Add(dtTemp.Month, tmp.Count);

                    dtTemp = dtTemp.AddMonths(1);
                }

                /*Since box D is checked on line 22: it is not required to complete column (b).  Please remove your calculations for column B.*/
                var cb = input.FirstOrDefault(p => p.Key == "Check9" && p.Value == "true").Value;
                if (string.IsNullOrEmpty(cb)) {
                    //Column B
                   input.Add("Text25", ColumnB.FirstOrDefault(p => p.Key == 1).Value.ToString());
                   input.Add("Text28", ColumnB.FirstOrDefault(p => p.Key == 2).Value.ToString());
                   input.Add("Text31", ColumnB.FirstOrDefault(p => p.Key == 3).Value.ToString());
                   input.Add("Text34", ColumnB.FirstOrDefault(p => p.Key == 4).Value.ToString());
                   input.Add("Text37", ColumnB.FirstOrDefault(p => p.Key == 5).Value.ToString());
                   input.Add("Text40", ColumnB.FirstOrDefault(p => p.Key == 6).Value.ToString());
                   input.Add("Text43", ColumnB.FirstOrDefault(p => p.Key == 7).Value.ToString());
                   input.Add("Text46", ColumnB.FirstOrDefault(p => p.Key == 8).Value.ToString());
                   input.Add("Text49", ColumnB.FirstOrDefault(p => p.Key == 9).Value.ToString());
                   input.Add("Text52", ColumnB.FirstOrDefault(p => p.Key == 10).Value.ToString());
                   input.Add("Text55", ColumnB.FirstOrDefault(p => p.Key == 11).Value.ToString());
                   input.Add("Text58", ColumnB.FirstOrDefault(p => p.Key == 12).Value.ToString());
                }

                //Column C
                input.Add("Text26", ColumnC.FirstOrDefault(p => p.Key == 1).Value.ToString());
                input.Add("Text29", ColumnC.FirstOrDefault(p => p.Key == 2).Value.ToString());
                input.Add("Text32", ColumnC.FirstOrDefault(p => p.Key == 3).Value.ToString());
                input.Add("Text35", ColumnC.FirstOrDefault(p => p.Key == 4).Value.ToString());
                input.Add("Text38", ColumnC.FirstOrDefault(p => p.Key == 5).Value.ToString());
                input.Add("Text41", ColumnC.FirstOrDefault(p => p.Key == 6).Value.ToString());
                input.Add("Text44", ColumnC.FirstOrDefault(p => p.Key == 7).Value.ToString());
                input.Add("Text47", ColumnC.FirstOrDefault(p => p.Key == 8).Value.ToString());
                input.Add("Text50", ColumnC.FirstOrDefault(p => p.Key == 9).Value.ToString());
                input.Add("Text53", ColumnC.FirstOrDefault(p => p.Key == 10).Value.ToString());
                input.Add("Text56", ColumnC.FirstOrDefault(p => p.Key == 11).Value.ToString());
                input.Add("Text59", ColumnC.FirstOrDefault(p => p.Key == 12).Value.ToString());

                SetInner(input);
            }
        }

        #endregion



        static void SetInner(Dictionary<string, string> inner) {
            foreach (var i in inner) {
                if (!Output.ContainsKey(i.Key)) {
                    Output.Add(i.Key, i.Value);
                }
            }
        }
        static void SetInner(string key, string value) {
            if (!Output.ContainsKey(key)) {
                Output.Add(key, value);
            }
        }

        public static Dictionary<string, string> GetResult(int year) {            
            Init(year);
            return Output;
        }
    }
}