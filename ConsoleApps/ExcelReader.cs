using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace ConsoleApps
{
    public class ExcelReader
    {
        public List<string> Regex()
        {
            StreamReader sr = new StreamReader(@"C:\Users\DATIA-SOFT.DEV02\Desktop\datas.txt");
            var str = sr.ReadToEnd();
            var noExist = str.Split("||||");
            var exp = new Regex(@"\d{1,2}/\d{1,2}/\d{4}\s\d{1,2}\:\d{1,2}\:\d{1,2}\s\D\D\d");
            var d = noExist.Select(x => exp.Matches(x)).ToList();
            d.RemoveAt(0);
            d.RemoveAt(0);
            var result = new List<string>();
            foreach (var match in d)
            {
                result.AddRange(match.Select(x => x.Value));
            }
            return result;
        }

        public List<string> LoadData(string dates)
        {
            IWorkbook _workbook = new XSSFWorkbook(@"C:\Users\DATIA-SOFT.DEV02\Desktop\Events(2).xlsx");
            var sheet = _workbook.GetSheetAt(0);
            var result = new List<string>();
            var noExist = new List<string>();
            var last = sheet.LastRowNum;
            var noExistCount = 0;
            for (int i = 0; ; i++)
            {
                var row = sheet.GetRow(i);
                if (row == null)
                {
                    break;
                }
                var value = row.GetCell(0).StringCellValue;
                Console.WriteLine("Resolved({0}/{1}),Not Exist count({2})", i + 1, last + 1, noExistCount);
                var start = dates.IndexOf(value);
                if (start == -1)
                {
                    noExistCount += 1;
                    result.Add(value);
                }
                else
                {
                    var data = dates.Substring(0, start);
                    if (data.IndexOf(@"/2021") > -1)
                    {
                        noExistCount += 1;
                        noExist.Add(data);
                    }
                    dates = dates.Remove(0, start);
                    dates = dates.Remove(0, value.Length);
                }
                if (value == "6/1/2021 8:55:41 AM")
                {
                    break;
                }
            }

            return noExist;
        }
    }
}
