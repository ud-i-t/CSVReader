using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RLF3
{
    class CSVReader
    {
        public static List<UserData> Parse(string FileName)
        {
            TextFieldParser parser = new TextFieldParser(FileName, Encoding.GetEncoding("Shift_JIS"));
            parser.TextFieldType = FieldType.Delimited;
            parser.SetDelimiters(",");

            string[] keys = parser.ReadFields();
            foreach(var k in keys.Select((v, i) => new { v, i }))
            {
                keys[k.i] = k.v.ToLower();
            }

            var tbl = new List<UserData>();

            while (parser.EndOfData == false)
            {
                string[] column = parser.ReadFields();
                var row = new UserData();

                foreach (var c in column.Select((v, i) => new { v, i }))
                {
                    row.Add(keys[c.i], c.v);
                }
                tbl.Add(row);
            }

            return tbl;
        }

        public static List<T> Parse<T>(string FileName) where T : new()
        {
            TextFieldParser parser = new TextFieldParser(FileName, Encoding.GetEncoding("Shift_JIS"));
            parser.TextFieldType = FieldType.Delimited;
            parser.SetDelimiters(",");

            string[] keys = parser.ReadFields();
            foreach (var k in keys.Select((v, i) => new { v, i }))
            {
                keys[k.i] = k.v.ToLower();
            }

            var tbl = new List<T>();

            while (parser.EndOfData == false)
            {
                string[] column = parser.ReadFields();
                var row = new T();
                Type t = row.GetType();

                foreach (var c in column.Select((v, i) => new { v, i }))
                {
                    var p = t.GetProperty(keys[c.i]);

                    // プロパティが存在しない場合
                    if(p == null)
                    {
                        continue;
                    }

                    var val = c.v;
                    if (p.PropertyType.Equals(typeof(Int32))) {
                        int v = val == "" ? -1 : Int32.Parse(val);
                        p.SetValue(row, Convert.ToInt32(v));
                    }
                    else
                    {
                        p.SetValue(row, val);
                    }
                    
                }
                //データセット後処理
                var afterDataset = t.GetMethod("AfterDataSet");
                if (afterDataset != null) afterDataset.Invoke(row, null);

                tbl.Add(row);
            }

            return tbl;
        }
    }
}
