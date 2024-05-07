
using System;
using System.Windows.Documents;
using System.Windows.Forms;
using Autodesk.Navisworks.Api.Plugins;
using CDS_Plugin.AddColor;
using CDS_Plugin.AddParams;
using CDS_Plugin.Exp_Imp;

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
using CDS_Plugin.XMLImportExport.ExportQuantity;
using System.Security.Cryptography;
using CDS_Plugin.Quantification.ExportQuantity;
using Microsoft.Office.Interop.Excel;
using CDS_Plugin.Quantification.CreateTakeoff;
using CDS_Plugin.Quantification.CreateQuantification;

namespace CDS_Plugin
{

    // Добавление панели (с ссылкой на xaml на оформление), добавление кнопок с ссылками на иконки.
    [Plugin("AddinRibbon", "CDS", DisplayName = "AddinRibbon")]
    [RibbonLayout("CDSRibbon.xaml")]
    [RibbonTab("CDS_Plugin", DisplayName = "CDS_Plugin")]

    [Command("ITR", Icon = "ITR_Tab_16px.png", LargeIcon = "ITR_Tab_32px.png", ToolTip = "Тест")]
    [Command("ITRBt1", Icon = "1_16.png", LargeIcon = "1_32.png", ToolTip = "Окрашивает выбранные элементы в синий цвет")]
    [Command("ITRBt2", Icon = "2_16.png", LargeIcon = "2_32.png", ToolTip = "Окрашивает выбранные элементы в оранжевый цвет")]

    [Command("USK", Icon = "USK_Tab_16px.png", LargeIcon = "USK_Tab_32px.png", ToolTip = "Тест")]
    [Command("USKBt1", Icon = "3_16.png", LargeIcon = "3_32.png", ToolTip = "Окрашивает выбранные элементы в зеленый цвет")]
    [Command("USKBt2", Icon = "4_16.png", LargeIcon = "4_32.png", ToolTip = "Окрашивает выбранные элементы в красный цвет")]

    [Command("AddClChoice", Icon = "0_16.png", LargeIcon = "0_32.png", ToolTip = "Тест")]
    [Command("BtRole", Icon = "Role_Tab_16px.png", LargeIcon = "Role_Tab_32px.png", ToolTip = "Определяет роли")]
    [Command("AddClBt1", Icon = "1_16.png", LargeIcon = "1_32.png", ToolTip = "Окрашивает выбранные элементы в синий цвет")]
    [Command("AddClBt2", Icon = "2_16.png", LargeIcon = "2_32.png", ToolTip = "Окрашивает выбранные элементы в красный цвет")]
    [Command("AddClBt3", Icon = "3_16.png", LargeIcon = "3_32.png", ToolTip = "Окрашивает выбранные элементы в зеленый цвет")]
    [Command("RstClEl", Icon = "ResetEl_16.png", LargeIcon = "ResetEl_32.png", ToolTip = "Сбрасывает постоянные настройки цвета и прозрачности для выбранных элементов")]
    [Command("RstClAll", Icon = "ResetAll_16.png", LargeIcon = "ResetAll_32.png", ToolTip = "Сбрасывает все постоянные настройки цвета и прозрачности")]
    [Command("Add_p", Icon = "Add_p_16.png", LargeIcon = "Add_p_32.png", ToolTip = "Добавить свойства для ПТО")]
    [Command("Add_all_p_xlsx", Icon = "Add_all_p_xlsx_16.png", LargeIcon = "Add_all_p_xlsx_32.png", ToolTip = "Добавить свойства для ПТО ей модели")]
    [Command("Add_el_p_xlsx", Icon = "Add_el_p_xlsx_16.png", LargeIcon = "Add_el_p_xlsx_32.png", ToolTip = "Добавить свойства для ПТО по элементно")]
    [Command("SetSettings", Icon = "SetSetting_16px.png", LargeIcon = "SetSetting_32px.png", ToolTip = "Добавление поисковых наборов по параметрам")]
    [Command("TestTab", Icon = "Test_Tab_16px.png", LargeIcon = "Test_Tab_32px.png", ToolTip = "Тест")]
    
    //[Command("AddCategory", Icon = "5_16.png", LargeIcon = "5_32.png", ToolTip = "Добавляет кастомные параметры в модель")]
    [Command("Export", Icon = "6_16.png", LargeIcon = "6_32.png", ToolTip = "Экспортирует параметры")]
    [Command("Test", Icon = "6_16.png", LargeIcon = "6_32.png", ToolTip = "Тест")]
    [Command("Custom_Tab", Icon = "7_16.png", LargeIcon = "7_32.png", ToolTip = "Добавить свойства для ПТО")]
    //, Icon = "1_16.png", LargeIcon = "1_32.png",

    //Создем ленту в Navis
    //Добавляем ссылки отдельных процессов (классов) на кнопки
    public class AddingButton : CommandHandlerPlugin
    {
        public override int ExecuteCommand(string name, params string[] parameters)
        {
            switch (name)
            {
                //Кнопка 1 с изменением цвета элемента на синий
                case "BtRole":
                    FormAddRole FormAddRole = new FormAddRole();
                    FormAddRole.Show();
                    break;
                case "ITRBt1":
                    var tp11 = new ITRAdd();
                    //tp1.Show();
                    tp11.Execute(parameters);
                    break;
                case "ITRBt2":
                    var tp12 = new ITRCorrect();
                    //tp1.Show();
                    tp12.Execute(parameters);
                    break;

                    case "USKBt1":
                    var tp21 = new USKAdd();
                    //tp1.Show();
                    tp21.Execute(parameters);
                    break;
                case "USKBt2":
                    var tp22 = new USKRemark();
                    //tp1.Show();
                    tp22.Execute(parameters);
                    break;


                //Кнопка 1 с изменением цвета элемента на синий
                //case "AddClBt1":
                //    var tp1 = new AddColor1();
                //    //tp1.Show();
                //    tp1.Execute(parameters);
                    //break;
                //Кнопка 2 с изменением цвета элемента на красный
                //case "AddClBt2":
                //    var tp2 = new AddColor.AddColor2();
                //    tp2.Execute(parameters);
                //    break;
                //Кнопка 3 с изменением цвета элемента на зеленый
                //case "AddClBt3":
                //    var tp3 = new AddColor.AddColor3();
                //    tp3.Execute(parameters);
                //    break;


                //Кнопка 4 на удаление любого изменения цвета и прозрачности в выбранных элементах
                case "RstClEl":
                    var tp4 = new AddColor.ResetColorEl();
                    tp4.Execute(parameters);
                    break;
                //Кнопка 5 на удаление любого изменения цвета и прозрачности в моделе
                case "RstClAll":
                    var tp5 = new AddColor.ResetColorAll();
                    tp5.Execute(parameters);
                    break;
                case "Add_all_p_xlsx":
                    FormAddPropAll FormAddPropAll = new FormAddPropAll();
                    FormAddPropAll.Show();
                    break;

                case "Add_el_p_xlsx":
                    FormAddPropEll FormAddPropEll = new FormAddPropEll();
                    FormAddPropEll.Show();
                    break;

                case "SetSettings":
                    FormSetSettings FormSetSettings = new FormSetSettings();
                    FormSetSettings.Show();
                    break;
                case "TestTab":
                    FormAddRole FormAddRole1 = new FormAddRole();
                    FormAddRole1.Show();
                    break;

                //Кнопка 5 - добавляет категорию свойств
                case "AddCategory":
                    PTO_Form pto_Form = new PTO_Form();
                    pto_Form.Show();

                    //var tp5 = new AddParams.AddCategory();
                    //tp5.Execute(parameters);
                    break;
                //Кнопка 6 - экспортирует все элемены модели в xlsx
                case "Export":
                    var tp6 = new ExportQuantityPlugin(); 
                    tp6.Execute(parameters);
                   
                    break;
                case "Test":
                    var tp7 = new AddResources();

                    try
                    {
                        tp7.Execute(parameters);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                    break;

                //Кнопка 7 - скопированно с Custom Ribbon
                case "Custom_Tab":
                    MessageBox.Show("В разработке");
                    break;

            }
            return 0;
        }
    }

}