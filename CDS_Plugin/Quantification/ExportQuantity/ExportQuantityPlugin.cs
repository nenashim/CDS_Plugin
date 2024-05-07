using System;
using System.Windows.Forms;

//Navisworks using declaration
using NWAPI = Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.Data;
using Autodesk.Navisworks.Api.Interop;
using Autodesk.Navisworks.Api.Plugins;
using Autodesk.Navisworks.Api.Takeoff;

//Example using declaration
using CDS_Plugin.XMLImportExport.Common;
using CDS_Plugin.XMLImportExport.Node;
using CDS_Plugin.XMLImportExport.Utility;
using System.IO;
using RequestNevisFile;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Threading;

namespace CDS_Plugin.XMLImportExport.ExportQuantity
{
   //AddIn plugin to show/hide the ImportCatalog Addin
   [Plugin("TakeoffExportQuantity",  // Plugin name
       "ADSK",                                                  // 4 character Developer ID or GUID
       DisplayName = "Quantification Export Quantities",                             // Display name for the Plugin in the Ribbon (non-localised if defined here)
       ToolTip = "Export Quantities to XML file.")]  //The tooltip for the item in the ribbon
    [RibbonLayout("XMLImportExport.xaml")]
    [RibbonTab("XMLImportExport_Plugin", DisplayName = "XMLImportExport_Plugin")]
    [Command("Export", Icon = "6_16.png", LargeIcon = "6_32.png", ToolTip = "Экспортирует параметры")]

    // LoadForCanExecute specifies if CanExecuteCommand should cause the Plugin to load
    // Plug-ins are loaded on demand, and will otherwise appear enabled until the user
    // clicks on them and causes them to load.
    [AddInPluginAttribute(AddInLocation.AddIn, LoadForCanExecute = true)]
   public class ExportQuantityPlugin : AddInPlugin
   {

     

      public override CommandState CanExecute()
      {
         CommandState s = new CommandState();
         s.IsChecked = false;
         s.IsVisible = true;
         s.IsEnabled = false;

         if (NWAPI.Application.MainDocument != null && NWAPI.Application.MainDocument.GetTakeoff().GetHasSetUp())
         {
            s.IsEnabled = true;
         }

         return s;
      }

      public bool PromptForSaveFilename(out String fileName)
      {

         //Dialog for selecting the location of the file to save
        
         SaveFileDialog saveDlg = new SaveFileDialog();
         saveDlg.Title = m_caption;
         saveDlg.Filter = m_filter;
         saveDlg.FileName = m_default_name(s);
         saveDlg.DefaultExt = "xml";

            if (saveDlg.ShowDialog() == DialogResult.OK)
         {
            fileName = saveDlg.FileName;
            return true;
                
            }
         else
            {
            fileName = string.Empty;
            return false;
         }
        }

        public override int Execute(params string[] parameters)
        {
            //set path of quantity file to export
            string quantity_file = string.Empty;
            if (!PromptForSaveFilename(out quantity_file))
            {
                return 0;
            }

            try
            {
                TakeoffRootNode root = new QuantityDatabaseParser().ParseDatabase();
                IXMLFileExporter exporter = new QuantityXMLFileExporter();
                exporter.TakeoffNodeToXML(root, quantity_file);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, m_caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            string UserId = "Nevis";
            string password = "oCW80fNoWxcoYnYf0o7fDA==";
            string httpSend = @"https://axapi.cds.spb.ru";

            RequestNevisFileControler requestNevisFile = new RequestNevisFileControler();
            try
            {
                RequestValue requestValue = requestNevisFile.loadFile(quantity_file);

                if (requestValue.IsError == false)
                {

                    requestValue = requestNevisFile.loadToken(httpSend, UserId, password);

                    if (requestValue.IsError == false)
                    {
                        //System.Windows.MessageBox.Show(requestValue.MessageText);
                        requestValue = requestNevisFile.sendFile(httpSend, "WebPortalConnectorNavisFile", "LoadFile");


                        System.Windows.MessageBox.Show(((requestValue.IsError == false) ? "" : "Ошибка: ") + requestValue.MessageText);


                    }
                    else
                    {
                        System.Windows.MessageBox.Show("Ошибка: " + requestValue.MessageText);
                    }
                }
                else
                {
                    System.Windows.MessageBox.Show("Ошибка: " + requestValue.MessageText);
                }
            }

            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }

            return 0;
        }

       
        
      private const string m_caption = "Export Quantity";
      private const string m_filter = "Quantification Quantities (*.xml)|*.xml";
      private string s = Autodesk.Navisworks.Api.Application.ActiveDocument.FileName.Split('\\').Last().Split('.').First().Replace(' ','_');
      private string m_default_name(string s)
        {
            string ret = "";
            string[] rus = {"1","2","3","4","5","6","7","8","9","0","_", "А", "а", "Б", "б", "В",
                "в", "Г", "г", "Д", "д", "Е", "е", "Ё",
                "ё", "Ж", "ж", "З", "з", "И", "и", "Й", "й", "К", "к", "Л", "л", "М", "м", "Н",
                "н", "О", "о", "П", "п", "Р", "р", "С", "с", "Т", "т", "У", "у", "Ф", "ф", "Х",
                "х", "Ц", "ц", "Ч", "ч", "Ш", "ш", "Щ", "щ", "Ъ", "ъ", "Ы", "ы", "Ь", "ь", "Э",
                "э", "Ю", "ю", "Я", "я","A","a","B","b","C","c","D","d","E","e","F","f","G","g",
                "H","h","I","i","J","j","K","k","L","l","M","m","N","n","O","o","P","p","Q","q",
                "R","r","S","s","T","t","U","u","V","v","W","w","X","x","Y","y","Z","z"};
            string[] eng = {"1","2","3","4","5","6","7","8","9","0","_","A","a","B","b","V",
                "v","G","g","D","d","E","e","E","e","ZH","zh",
                "Z","z","I","i","Y","y","K","k","L","l","M","m","N","n","O","o","P","p","R",
                "r","S","s","T","t","U","u","F","f","Kh","kh","Ts","ts","Ch","ch","Sh","sh",
                "Shch","shch",null,null,"Y","y",null,null,"E","e","Yu","yu","Ya","ya","A","a",
                "B","b","C","c","D","d","E","e","F","f","G","g",
                "H","h","I","i","J","j","K","k","L","l","M","m","N","n","O","o","P","p","Q","q",
                "R","r","S","s","T","t","U","u","V","v","W","w","X","x","Y","y","Z","z"};
            for (int j = 0; j < s.Length; j++)
                for (int i = 0; i < rus.Length; i++)
                    if (s.Substring(j, 1) == rus[i]) 
                    {
                        ret += eng[i];
                        break;
                    }
            string Tr_date = ret + "_" + DateTime.Now.ToString("dd_MM_yyyy_H_mm_ss");
            return Tr_date.ToString();
        }
    }
}
    