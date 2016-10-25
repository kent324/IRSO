using IRSOutput.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRSOutput.DAL {

    public class IRSQueryDB {
        private static string strSQL = string.Empty;
        internal static List<IRSEmployee> loadAllEmployees(string Condition, DateTime startDate, DateTime endDate, EnumIRSTotalNumber tn = EnumIRSTotalNumber.Text18_20) {
            List<IRSEmployee> Employees = new List<IRSEmployee>();
            string employeeType = "AND employee_type in('001','002','003')";

            if (tn == EnumIRSTotalNumber.ColumnC) {
                employeeType = "AND employee_type in('001','002','003','004','005')";
            }
            #region T-SQL
            strSQL = string.Format(@"SELECT 
a.EInfo_id
,h.dept_id AS DepartId
,h.name AS Department
,b.Name
,b.Status
,b.[1C]
,b.[1D]
,b.[1E]
,b.[1F]
,b.[Month]
,b.[Year]
,REPLACE(a.SSN,'-','') AS SSN
,CASE WHEN isnull(a.Name_Middle,'') ='' THEN isnull(a.Name_First,'') +' , ' + isnull(a.Name_Last,'') ELSE  isnull(a.Name_First,'') +' , '+ isnull(a.Name_Middle,'') + ' , ' + isnull(a.Name_Last,'') END AS EmployeeName
,a.Name_First
,a.Name_Middle
,a.Name_Last
,a.Date_Birth
, isnull(e.street1,'')+isnull(e.street2,'') AS Address	 
, Replace(Replace(e.city, ',', ''),'''','') City	 
,e.State
, Replace(e.ZipCode, ',', '') ZipCode
, cast(d.date_entry_to_agency AS DATE) HireDate
,cast(d.date_terminate_from_agency AS DATE) terminateDate
,CASE WHEN ISNULL((SELECT TOP 1 EInfo_id FROM Position_Info aa WHERE aa.EInfo_id IS NOT NULL AND aa.EInfo_id=a.EInfo_id  GROUP BY aa.EInfo_id HAVING COUNT(*) >= 2),'')='' THEN '0' ELSE '1' END AS isRehire
 FROM (
    SELECT * FROM Person WHERE EInfo_id IN (
		SELECT DISTINCT b.EInfo_id FROM (
			SELECT DISTINCT a.EInfo_id  FROM person a 
			INNER JOIN Employment_Control b ON b.Person_System_ID = a.Person_System_ID
			INNER JOIN Employment_Info c ON c.Employment_System_ID = b.Employment_System_ID 
			INNER JOIN Position_Info d ON b.Position_System_ID=d.Position_System_ID
			WHERE 
			CAST(position_start_date AS DATE)<'{2}' AND CAST(isnull(position_end_date,'12/31/2099') AS DATE)>'{3}' 
			AND d.EInfo_id IN (       
				SELECT EInfo_id FROM Position_Info 
				WHERE EInfo_id IS NOT NULL
				GROUP BY EInfo_id 
				HAVING COUNT(*)>=2
			) {4}
            AND (c.employment_status=1 OR year(c.date_terminate_from_agency)>='{1}')
			
			UNION ALL
			
			SELECT aa.EInfo_id FROM (
				SELECT  a.*  FROM person a 
				INNER JOIN Employment_Control b ON b.Person_System_ID = a.Person_System_ID 
					AND b.Update_Date = (SELECT Max(Update_Date) FROM Employment_Control WHERE Person_System_ID = a.Person_System_ID) 
				INNER JOIN Employment_Info c ON c.Employment_System_ID = b.Employment_System_ID 
				INNER JOIN Position_Info d ON b.Position_System_ID=d.Position_System_ID
				WHERE CAST(position_start_date AS DATE)<'{2}' {4}
                AND (c.employment_status=1 OR year(c.date_terminate_from_agency)>='{1}')
				
				EXCEPT
				SELECT  a.*  FROM person a 
				INNER JOIN Employment_Control b ON b.Person_System_ID = a.Person_System_ID 
					AND b.Update_Date = (SELECT Max(Update_Date) FROM Employment_Control WHERE Person_System_ID = a.Person_System_ID) 
				INNER JOIN Employment_Info c ON c.Employment_System_ID = b.Employment_System_ID 
				INNER JOIN Position_Info d ON b.Position_System_ID=d.Position_System_ID
				WHERE CAST(position_end_date AS DATE)<'{3}' {4}
				AND (c.employment_status=1 OR year(c.date_terminate_from_agency)>='{1}')
				) aa 
		)b
	)
)a
LEFT JOIN emp_Healthbilling b ON a.EInfo_id=b.EInfo_id AND b.Year = '{1}' AND b.id = (SELECT TOP 1 aa.id FROM emp_Healthbilling aa WHERE aa.EInfo_id = a.EInfo_id ORDER BY aa.Month DESC)
INNER JOIN Employment_Control c ON a.Person_System_ID = c.Person_System_ID AND c.Update_Date = 
	(SELECT Max(Update_Date) FROM Employment_Control WHERE Person_System_ID = a.Person_System_ID)
INNER JOIN Employment_Info d ON c.Employment_System_ID = d.Employment_System_ID 
INNER JOIN Position_Info g ON c.Position_System_ID = g.Position_System_ID 
LEFT JOIN person_address e ON e.Person_System_ID = a.Person_System_ID  AND e.address_id =
	(SELECT max(address_id) FROM  person_address where Person_System_ID=a.Person_System_ID)
LEFT JOIN person_phone f ON f.Person_System_ID = a.Person_System_ID  AND f.phone_id=
	(SELECT max(phone_id) FROM  person_phone where Person_System_ID=a.Person_System_ID)
LEFT JOIN ref_department h ON h.dept_id=g.dept_id  
WHERE a.EInfo_id NOT IN(7840,7940,7950,7960)
{0}
ORDER BY a.EInfo_id", Condition, startDate.Year, startDate, endDate, employeeType);

            #endregion
            using (SqlDataReader dr = SqlHelperIRS.ExecuteReader(CommandType.Text, strSQL)) {
                while (dr.Read()) {
                    Employees.Add(new IRSEmployee() {
                        Department = dr["Department"].ToString(),
                        DepartId = dr["DepartId"].ToInt(),
                        Name_First = dr["Name_First"].ToString(),
                        Name_Middle = dr["Name_Middle"].ToString(),
                        Name_Last = dr["Name_Last"].ToString(),
                        Address = dr["Address"].ToString(),
                        C1 = dr["1C"].ToString(),
                        City = dr["City"].ToString(),
                        D1 = dr["1D"].ToString(),
                        E1 = dr["1E"].ToString(),
                        EinfoID = dr["EInfo_id"].ToInt(),
                        EmployeeName = dr["EmployeeName"].ToString(),
                        F1 = dr["1F"].ToString(),
                        HireDate = Convert.ToDateTime(dr["HireDate"].ToString()),
                        terminateDate = !dr.IsDBNull(dr.GetOrdinal("terminateDate")) && !string.IsNullOrEmpty(dr["terminateDate"].ToString())
                        ? Convert.ToDateTime(dr["terminateDate"].ToString())
                        : DateTime.MinValue,
                        DOB = !dr.IsDBNull(dr.GetOrdinal("Date_Birth")) && !string.IsNullOrEmpty(dr["Date_Birth"].ToString())
                        ? Convert.ToDateTime(dr["Date_Birth"].ToString())
                        : DateTime.MinValue,
                        Month = dr["Month"].ToString(),
                        Name = dr["Name"].ToString(),
                        SSN = dr["SSN"].ToString(),
                        State = dr["State"].ToString(),
                        Status = dr["Status"].ToString(),
                        Year = dr["Year"].ToString(),
                        ZipCode = dr["ZipCode"].ToString(),
                        isRehire = Convert.ToBoolean(Convert.ToInt16(dr["isRehire"])),
                        ClosingDate = startDate
                    });
                }
            }
            return Employees;
        }

        /// <summary>
        /// 获取Column14 Code
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        internal static string Code14(int num) {

            try {
                strSQL = string.Format("SELECT Code FROM ref_line14code WHERE id ='{0}'", num);
                return SqlHelperIRS.ExecuteScalarToStr(CommandType.Text, strSQL);
            }
            catch (Exception) {
                throw;
            }
        }

        /// <summary>
        /// 根据Year获取Column15 Code
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        internal static string Code15(int year) {
            try {
                strSQL = string.Format("SELECT Amount FROM ref_line15amount WHERE Year='{0}'", year);
                return SqlHelperIRS.ExecuteScalarToStr(CommandType.Text, strSQL);
            }
            catch (Exception) {
                throw;
            }
        }
        internal static string Code15Date(int year) {
            try {
                strSQL = string.Format("SELECT Date FROM ref_line15amount WHERE Year ='{0}'", year);
                string result = SqlHelperIRS.ExecuteScalarToStr(CommandType.Text, strSQL);
                DateTime signatureDate = DateTime.MinValue;
                if (DateTime.TryParse(result, out signatureDate) && signatureDate > DateTime.MinValue) {
                    return signatureDate.ToString("MM/dd/yyyy");
                }
            }
            catch (Exception) {
                throw;
            }
            return string.Empty;
        }

        internal static List<HealthbillingKeyValue> loadHealthbilling(int year) {
            ReadProperty rp = new ReadProperty();
            try {
                strSQL = string.Format(@"SELECT EInfo_id,Month,SUM([1C]+[1D]+[1E]+[1F]) AS RESULT FROM emp_Healthbilling 
    WHERE Month BETWEEN 1 AND 12 AND Year = '{0}'GROUP BY EInfo_id, Month ORDER BY EInfo_id, CAST(Month AS INT)", year);


                List<HealthbillingKeyValue> obj = new List<HealthbillingKeyValue>();

                int month = 0;

                using (SqlDataReader dr = SqlHelperIRS.ExecuteReader(CommandType.Text, strSQL)) {
                    while (dr.Read()) {
                        month = dr["Month"].ToInt();
                        obj.Add(new HealthbillingKeyValue() {
                            EinfoID = dr["EInfo_id"].ToInt(),
                            Month = dr["Month"].ToInt(),
                            Value = dr["RESULT"].ToFloat(),
                            TextField14 = string.Format("Text{0}", rp.code14[month - 1].ToString()),
                            TextField15 = string.Format("Text{0}", rp.code15[month - 1].ToString()),
                            TextField16 = string.Format("Text{0}", rp.code16[month - 1].ToString()),
                        });
                    }
                }
                return obj;
            }
            catch (Exception) {
                throw;
            }
        }

        internal static bool getRehireEmployeeDate(int EinfoID, int Year, ref int reHire, ref int end) {
            bool result = false;
            strSQL = string.Format("EXEC sp_IRS_rehireEmplyee '{0}',{1}", EinfoID, Year);
            using (SqlDataReader dr = SqlHelperIRS.ExecuteReader(CommandType.Text, strSQL)) {
                while (dr.Read()) {
                    reHire = dr["mdtRehire"].ToInt();
                    end = dr["mdtEnd"].ToInt();
                    result = reHire > 0 || end > 0;
                }
            }
            return result;
        }

        /// <summary>
        /// Q: Should be 2D, because the employee's TD is 1/9/2015 years, he worked in January. Right?
        /// A: You are correct.The employee worked in January.As a result, it cannot be 2A or 2D.  If there is no record on emp_helathbilling for January, it has to be left blank
        /// </summary>
        /// <param name="query"></param>
        /// <param name="employee"></param>
        /// <returns></returns>
        internal static bool TerminationBorder(List<HealthbillingKeyValue> query, IRSEmployee employee) {
            if (query.Count > 0) {
                var last = query.OrderByDescending(p => p.Month).Take(1).FirstOrDefault();
                if (last.Value > 0) {
                    //处理离职最后一个月
                    string start = employee.terminateDate.Month == last.Month
                        ? employee.terminateDate.ToString("MM/01/yyyy")
                        : string.Format("{0}/1/{1}", last.Month, employee.Year);
                    string end = employee.terminateDate.Month == last.Month
                        ? employee.terminateDate.ToShortDateString()
                        : DateTime.Parse(start).AddMonths(1).AddDays(-1).ToShortDateString();
                    
                    strSQL = string.Format("SELECT COUNT(*) FROM emp_LeaveRecord WHERE EInfo_id={0} AND leaveType IN('SWOP','LWOP','SWP') AND leaveDate BETWEEN '{1}' AND '{2}' ",
                        employee.EinfoID, start, end);
                    int result = SqlHelperIRS.ExecuteScalarToStr(CommandType.Text, strSQL).ToInt();
                    return result == 0;
                }
            }
            return false;
        }

        internal static Dictionary<string, string> ALE1095CSoure() {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            strSQL = string.Format("SELECT NameALE ,EmployerEIN ,[Address] ,City,ST,Country,ZIP,NamePersonContact,ContactPhone,NameGovrnEntity,Entity_EmployeEIN,StreetEntity_address,Entity_city,Entity_state,Entity_countryandZip,Entity_contact,Entity_phone,Einfo_sig,Einfo_title,plan_start_month FROM ref_ALE");
            using (SqlDataReader dr = SqlHelperIRS.ExecuteReader(CommandType.Text, strSQL)) {
                while (dr.Read()) {
                    dic.Add("Text7", dr["NameALE"].ToString());
                    dic.Add("Text8", dr["EmployerEIN"].ToString());
                    dic.Add("Text9", dr["Address"].ToString());
                    dic.Add("Text10", dr["ContactPhone"].ToString());
                    dic.Add("Text11", dr["City"].ToString());
                    dic.Add("Text12", dr["ST"].ToString());
                    dic.Add("Text13", dr["ZIP"].ToString());
                    dic.Add("Text77", dr["plan_start_month"].ToString());

                }
            }
            return dic;
        }
        internal static Dictionary<string, string> ALE1094CSoure() {
            string sig = string.Empty;
            Dictionary<string, string> dic = new Dictionary<string, string>();
            strSQL = string.Format("SELECT NameALE ,EmployerEIN ,[Address] ,City,ST,ZIP,NamePersonContact,ContactPhone,NameGovrnEntity,Entity_EmployeEIN,StreetEntity_address,Entity_city,Entity_state,Entity_countryandZip,Entity_contact,Entity_phone,Einfo_sig,Einfo_title FROM ref_ALE");
            using (SqlDataReader dr = SqlHelperIRS.ExecuteReader(CommandType.Text, strSQL)) {
                while (dr.Read()) {
                    dic.Add("Text1", dr["NameALE"].ToString());
                    dic.Add("Text2", dr["EmployerEIN"].ToString());
                    dic.Add("Text3", dr["Address"].ToString());
                    dic.Add("Text4", dr["City"].ToString());
                    dic.Add("Text5", dr["ST"].ToString());
                    dic.Add("Text6", dr["ZIP"].ToString());
                    dic.Add("Text7", dr["NamePersonContact"].ToString());
                    dic.Add("Text8", dr["ContactPhone"].ToString());
                    dic.Add("Text9", dr["NameGovrnEntity"].ToString());
                    dic.Add("Text10", dr["Entity_EmployeEIN"].ToString());
                    dic.Add("Text11", dr["StreetEntity_address"].ToString());
                    dic.Add("Text12", dr["Entity_city"].ToString());
                    dic.Add("Text13", dr["Entity_state"].ToString());
                    dic.Add("Text14", dr["Entity_countryandZip"].ToString());
                    dic.Add("Text15", dr["Entity_contact"].ToString());
                    dic.Add("Text16", dr["Entity_phone"].ToString());

                    sig = dr["Einfo_sig"].ToString();
                    if (!string.IsNullOrEmpty(sig)) {
                        dic.Add("Text19", String.Format("https://dbapp.hsgd.org/HRIS_HSGD/wupd/Signature.aspx?id={0}&ac=eid", sig));
                    }
                    dic.Add("Text20", dr["Einfo_title"].ToString());
                }
            }
            return dic;
        }

        public static List<OutputXMLResult> QueryTestSoure() {
            List<OutputXMLResult> soure = new List<OutputXMLResult>();
            strSQL = string.Format("SELECT EinfoID,ColumnName,TwelveMonth,Jan,Feb,Mar,Apr,May,June,July,Aug,Sept,Oct,Nov,Dec FROM A_IRSTest");
            using (SqlDataReader dr = SqlHelperIRS.ExecuteReader(CommandType.Text, strSQL)) {
                while (dr.Read()) {
                    soure.Add(new OutputXMLResult() {
                        EinfoID = dr["EinfoID"].ToString(),
                        ColumnName = dr["ColumnName"].ToInt(),
                        TwelveMonth = dr["TwelveMonth"].ToString(),
                        Jan = dr["Jan"].ToString(),
                        Feb = dr["Feb"].ToString(),
                        Mar = dr["Mar"].ToString(),
                        Apr = dr["Apr"].ToString(),
                        May = dr["May"].ToString(),
                        June = dr["June"].ToString(),
                        July = dr["July"].ToString(),
                        Aug = dr["Aug"].ToString(),
                        Sept = dr["Sept"].ToString(),
                        Oct = dr["Oct"].ToString(),
                        Nov = dr["Nov"].ToString(),
                        Dec = dr["Dec"].ToString(),
                    });
                }
            }
            return soure;
        }
    }
}