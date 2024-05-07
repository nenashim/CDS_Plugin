using System;
using Autodesk.Navisworks.Api.Plugins;
using System.Windows.Forms;
using Autodesk.Navisworks.Api;
using System.Collections.Generic;
using System.Linq;
using App = Autodesk.Navisworks.Api.Application;
using Autodesk.Navisworks.Api.DocumentParts;
using Microsoft.Office.Interop.Excel;
using Range = Microsoft.Office.Interop.Excel.Range;
using AppExcel = Microsoft.Office.Interop.Excel.Application;

using ComApi = Autodesk.Navisworks.Api.Interop.ComApi;
using ComApiBridge = Autodesk.Navisworks.Api.ComApi;



namespace CDS_Plugin
{
   
    public class AllCustomAddin
    { 
        public static object GetPropertyValue(DataProperty property)
        {
            object obj = null;
            switch (property.Value.DataType)
            {
                case VariantDataType.Boolean:
                    obj = property.Value.ToBoolean();
                    break;
                case VariantDataType.DateTime:
                    obj = property.Value.ToDateTime();
                    break;
                case VariantDataType.DisplayString:
                    obj = property.Value.ToDisplayString();
                    break;
                case VariantDataType.Double:
                    obj = property.Value.ToDouble();
                    break;
                case VariantDataType.DoubleAngle:
                    obj = property.Value.ToDoubleAngle();
                    break;
                case VariantDataType.DoubleArea:
                    obj = property.Value.ToDoubleArea() * 0.092903;
                    break;
                case VariantDataType.DoubleLength:
                    obj = property.Value.ToDoubleLength() * 0.3048;
                    break;
                case VariantDataType.DoubleVolume:
                    obj = property.Value.ToDoubleVolume() * 0.0283168;
                    break;
                case VariantDataType.IdentifierString:
                    obj = property.Value.ToIdentifierString();
                    break;
                case VariantDataType.Int32:
                    obj = property.Value.ToInt32();
                    break;
                case VariantDataType.NamedConstant:
                    obj = property.Value.ToNamedConstant();
                    break;
                case VariantDataType.None:
                    obj = property.Value.ToString();
                    break;
                case VariantDataType.Point3D:
                    obj = property.Value.ToString();
                    break;
            }
            return obj;
        }
       

        public static void setValueToProperties(string category, string property, ModelItem oEachSelectedItem, ComApi.InwOaProperty newP)
        {
            var cat = oEachSelectedItem.DescendantsAndSelf.Where(i => i.PropertyCategories.FindCategoryByDisplayName(category) != null);
            var pro = cat.Where(m => m.PropertyCategories.FindCategoryByDisplayName(category).Properties.FindPropertyByDisplayName(property) != null);

            if (pro.Any())
            {
                foreach (var categ in oEachSelectedItem.PropertyCategories)
                {
                    if (categ.DisplayName == category)
                    {
                        foreach (var prop in categ.Properties)
                        {
                            if (prop.DisplayName == property)
                            {
                                if (GetPropertyValue(prop).ToString() != "0")
                               {
                                    newP.value = GetPropertyValue(prop);                                  
                                }
                            }
                        }
                    }
                }
            }
        }

        public static string[] columns = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "AA", "AB", "AC", "AD", "AE", "AF", "AG", "AH", "AI", "AJ","AK", "AL", "AM", "AN", "AO","AP", "AQ", "AR", "AS","AT", "AU", "AV","AW","AX","AY","AZ", "BA", "BB", "BC", "BD", "BE", "BF", "BG", "BH", "BI", "BJ", "BK", "BL", "BM", "BN", "BO", "BP", "BQ", "BR", "BS", "BT", "BU", "BV", "BW", "BX", "BY", "BZ" };

        public static List<string> getPropNamesFromExcel(string path)
        {
            //Create App.
            AppExcel ObjExcel = new AppExcel();
            //Open Book.                                                                                                                                                        
            Workbook ObjWorkBook = ObjExcel.Workbooks.Open(path);
            //Choose sheet.
            Worksheet ObjWorkSheet;
            ObjWorkSheet = (Worksheet)ObjWorkBook.Sheets[1];
            int lastRow = ObjWorkSheet.Cells.SpecialCells(XlCellType.xlCellTypeLastCell).Row;
            int lastColumn = getLastColumn(1, path);

            Range range = ObjWorkSheet.UsedRange.Range[columns[0] + "1:" + columns[lastColumn - 1] + "1"];
            Array cells = (Array)range.Cells.Value2;
            List<string> data = cells.OfType<object>().Select(o => o.ToString()).ToList();
            // Выходим из программы Excel.
            ObjExcel.Quit();
            return data;
        }

        public static List<string> getPropTypesFromExcel(string path)
        {   //Create App.
            AppExcel ObjExcel = new AppExcel();
            //Open Book.                                                                                                                                                        
            Workbook ObjWorkBook = ObjExcel.Workbooks.Open(path);
            //Choose sheet.
            Worksheet ObjWorkSheet;
            ObjWorkSheet = (Worksheet)ObjWorkBook.Sheets[1];
            int lastRow = ObjWorkSheet.Cells.SpecialCells(XlCellType.xlCellTypeLastCell).Row;
            int lastColumn = getLastColumn(1, path);

            Range range = ObjWorkSheet.UsedRange.Range[columns[0] + "2:" + columns[lastColumn - 1] + "2"];
            //Range range = ObjWorkSheet.UsedRange.Range["A2:" + lastColumn];
            Array cells = (Array)range.Cells.Value2;
            List<string> data = cells.OfType<object>().Select(o => o.ToString()).ToList();

            // Выходим из программы Excel.
            ObjExcel.Quit();

            return data;
        }

        public static List<string> getDataForProp(string cat_param, string prop_param)
        {
            ModelItemEnumerableCollection items = App.ActiveDocument.Models.RootItemDescendants;

            List<string> list = new List<string>();

            foreach (ModelItem item in items)
            {
                foreach (var cat in item.PropertyCategories)
                {
                    if (cat.DisplayName == cat_param)
                    {
                        foreach (var prop in cat.Properties)
                        {
                            if (prop.DisplayName == prop_param)
                            {
                                if (!list.Contains(prop.Value.ToString().Substring(prop.Value.ToString().IndexOf(':') + 1)))
                                {
                                    list.Add(prop.Value.ToString().Substring(prop.Value.ToString().IndexOf(':') + 1));
                                }
                            }
                        }
                    }
                }

            }
            list.Sort();

            return list;
        }

        public static int getLastColumn(int sheet, string path)
        {
            //Create App.
            AppExcel ObjExcel = new AppExcel();
            //Open Book.                                                                                                                                                        
            Workbook ObjWorkBook = ObjExcel.Workbooks.Open(path);
            //Choose sheet.
            Worksheet ObjWorkSheet;
            ObjWorkSheet = (Worksheet)ObjWorkBook.Sheets[sheet];

            int lastColumn = ObjWorkSheet.Cells.SpecialCells(XlCellType.xlCellTypeLastCell).Column;

            // Exit Excel.
            ObjExcel.Quit();

            return lastColumn;
        }

        public static List<List<List<string>>> getDataOfAllProperties(int sheet, string path)
        {
            int lastColumn = getLastColumn(sheet, path);
            List<List<List<string>>> data = new List<List<List<string>>>();
            int odd = 0;
            for (int i = 0; i < lastColumn / 2; i++)
            {
                data.Add(sortDataByTypes(getCustomPropertiesData(sheet, path), odd));
                odd = odd + 2;
            }
            return data;
        }

        public static List<List<string>> getCustomPropertiesData(int sheet, string path)
        {

            List<List<string>> data = new List<List<string>>();

            //Create App.
            AppExcel ObjExcel = new AppExcel();
            //Open Book.                                                                                                                                                        
            Workbook ObjWorkBook = ObjExcel.Workbooks.Open(path);
            //Choose sheet.
            Worksheet ObjWorkSheet;
            ObjWorkSheet = (Worksheet)ObjWorkBook.Sheets[sheet];
            int lastRow = ObjWorkSheet.Cells.SpecialCells(XlCellType.xlCellTypeLastCell).Row;
            int lastColumn = getLastColumn(sheet, path);
            ;
            for (int i = 0; i < lastColumn; i++)
            {
                List<string> types = new List<string>();

                for (int j = 0; j < lastRow - 2; j++)

                {
                    types.Add(getRangeFromExcel(columns[i], "3", lastRow, ObjWorkSheet)[j]);
                }
                data.Add(types);
            }

            // Exit Excel.
            ObjExcel.Quit();

            return data;
        }

        public static List<List<string>> sortDataByTypes(List<List<string>> data, int num)
        {
            List<List<string>> prop = new List<List<string>>();
            prop.Add(data[num]);
            prop.Add(data[num + 1]);
            return prop;

        }

        public static List<string> getRangeFromExcel(string leter, string number, int lastRow, Worksheet ObjWorkSheet)
        {
            if (lastRow > 3)
            {
                Range range = ObjWorkSheet.UsedRange.Range[leter + number + ":" + leter + lastRow];
                Array cells = (Array)range.Cells.Value2;
                List<string> data = cells.OfType<object>().Select(o => o.ToString()).ToList();
                return data;
            }
            else
            {
                Range range = ObjWorkSheet.UsedRange.Range[leter + number];
                string cell = range.Cells.Value2;
                List<string> data = new List<string>();
                data.Add(cell);
                return data;
            }
        }

    }
}





