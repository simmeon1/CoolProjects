using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ClassLibrary.Shared
{
    public class ExcelPrinterBase
    {
        protected ExcelPackage Package { get; set; }
        protected ExcelWorksheet Worksheet { get; set; }

        protected ExcelPrinterBase()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }
    }
}
