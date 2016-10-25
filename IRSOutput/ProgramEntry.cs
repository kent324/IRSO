using IRSOutput.DAL;
using IRSOutput.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRSOutput {
    
    public class ProgramEntry {
        private  int Year { get; set; }
        private List<int> CorrectedList = new List<int>();

        public ProgramEntry(int year) {
            this.Year = year;
        }
        public ProgramEntry(int year,List<int> correctedlist) {
            this.Year = year;
            this.CorrectedList = correctedlist;
        }

        public Dictionary<string, string> Handle1094() {
            return C1904Factory.GetResult(this.Year);
        }
        public List<IRSEmployee> loadAllEmployees() {            
            //string Condition = string.Format(" AND a.EInfo_id IN ({0})", EinfoID);
            //var employees = IRSQueryDB.loadAllEmployees(Condition, dtStart, dtEnd);
            return DBSourceFactory.GetDBSource(Year);
            //return IRSQueryDB.loadAllEmployees(string.Empty, dtStart, dtEnd);
        }
        public Dictionary<string, string> Handle1095(IRSEmployee employee, bool isCorrected=false) {
            Context context = new Context(Year);            

            context.SetInner("Text1", employee.EmployeeName);
            context.SetInner("Text2", employee.SSN);
            context.SetInner("Text3", employee.Address);
            context.SetInner("Text4", employee.City);
            context.SetInner("Text5", employee.State);
            context.SetInner("Text6", employee.ZipCode);
            //context.SetInner("DOB", employee.DOB.ToString("yyyy-MM-dd"));
            context.SetInner("DOB", employee.DOB.ToString("yyyyMMdd"));
            context.SetInner("Check2", CorrectedList.Contains(employee.EinfoID) ? "1" : "0");

            BaseEmoloyee bEmployee = null;
            //foreach (var employee in employees) {
            if (!HardCode(employee.EinfoID, context)) {
                //在职
                if (employee.empStatus) {
                    if (employee.workYear) {//大于一年
                        bEmployee = new OnJobGreaterEmoloyee(employee);
                    }
                    else {//小于一年
                        bEmployee = new OnJobLessEmoloyee(employee);
                    }
                }
                else {//离职
                    if (employee.workYear) { //大于一年
                        bEmployee = new UnJobGreaterEmoloyee(employee);
                    }
                    else {//小于一年                        
                        bEmployee = new UnJobLessEmoloyee(employee);
                    }
                }
                bEmployee.handler14(context);
                bEmployee.handler15(context);
                bEmployee.handlerExtend15(context);
                bEmployee.handler16(context);
                bEmployee.handlerExtend14(context);
            }
            return context.GetResult();
        }

        public List<OutputXMLResult> Main(string EinfoID) {
            //DateTime dtStart = new DateTime(Year + 1, 1, 1).AddDays(-1);
            //DateTime dtEnd = new DateTime(Year, 1, 1);
            //string Condition = string.Format(" AND a.EInfo_id IN ({0})", EinfoID);
            //var employees = IRSQueryDB.loadAllEmployees(Condition, dtStart, dtEnd);

            //var employees = IRSQueryDB.loadAllEmployees(string.Empty, dtStart, dtEnd);
            var eid = EinfoID.ToInt();
            var all = this.loadAllEmployees();
            var employees = all.Where(p => p.EinfoID == eid).ToList();
            Context context = new Context(Year);
            BaseEmoloyee bEmployee = null;

            foreach (var employee in employees) {
                if (HardCode(employee.EinfoID, context)) {
                    continue;
                }
                //在职
                if (employee.empStatus) {
                    if (employee.workYear) {//大于一年
                        bEmployee = new OnJobGreaterEmoloyee(employee);
                    }
                    else {//小于一年
                        bEmployee = new OnJobLessEmoloyee(employee);
                    }
                }
                else {//离职
                    if (employee.workYear) { //大于一年
                        bEmployee = new UnJobGreaterEmoloyee(employee);
                    }
                    else {//小于一年                        
                        bEmployee = new UnJobLessEmoloyee(employee);
                    }
                }
                bEmployee.handler14(context);
                bEmployee.handler15( context);
                bEmployee.handlerExtend15(context);
                bEmployee.handler16( context);
                bEmployee.handlerExtend14(context);
            }
            return context.getXMLResult();
        }

        static bool HardCode(int EinfoID, Context context) {
            bool val = false;
            switch (EinfoID) {
                case 5626:
                    HardCodeHandle(15, 17, context.Code14, context);
                    HardCodeHandle(28, 30, context.Code15, context);
                    HardCodeHandle(41, 43, "2C", context);
                    HardCodeHandle(44, 52, "2A", context);
                    val = true;
                    break;
                case 5666:
                    HardCodeHandle(15, 18, context.Code14, context);
                    HardCodeHandle(27, 27, "0.00", context);
                    HardCodeHandle(45, 52, "2A", context);
                    val = true;
                    break;
                    //case 
            }
            return val;
        }
        static void HardCodeHandle(int start, int end, string code, Context context) {
            for (int i = start; i <= end; i++) {                
                context.SetInner("Text" + i, code);
            }
        }
    }
}