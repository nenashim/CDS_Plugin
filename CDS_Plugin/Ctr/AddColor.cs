using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.ComApi;
using Autodesk.Navisworks.Api.DocumentParts;
using Autodesk.Navisworks.Api.Interop.ComApi;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.PerformanceData;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Animation;
using static System.Windows.Forms.AxHost;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using ComApi = Autodesk.Navisworks.Api.Interop.ComApi;
using ComApiBridge = Autodesk.Navisworks.Api.ComApi.ComApiBridge;

namespace CDS_Plugin.AddColor
{

    internal class ITRAdd
    {
        public string ReadRoleName;
        public string ReadPersonName;
        public string parth = @"C:\Users\Public\RolePC.txt";
        public bool g = true;
        public string Date = DateTime.Today.ToString("dd/MM/yy");

        public List<InwOaProperty> GenerateParamListITR(string role, string state, string fio, string date, string readrolename, string readpersonname, InwOpState10 cdoc_)
        {
            List<InwOaProperty> paramsModelItem = new List<InwOaProperty>();
            for (int k = 0; k < 4; k++)
            {
                InwOaProperty paramITR = (InwOaProperty)cdoc_.ObjectFactory(nwEObjectType.eObjectType_nwOaProperty, null, null);
                paramsModelItem.Add(paramITR);
            }

            DateTime Date = DateTime.Now;

            paramsModelItem[0].name = role;
            paramsModelItem[0].value = readrolename;
            paramsModelItem[1].name = fio;
            paramsModelItem[1].value = readpersonname;
            paramsModelItem[2].name = state;
            paramsModelItem[2].value = "Выполнено";
            paramsModelItem[3].name = date;
            paramsModelItem[3].value = Date.ToString();

            return paramsModelItem;
        }

        public int Execute(params string[] parameters)
        {

            //Получаем доступ к файлу 
            Document doc = Autodesk.Navisworks.Api.Application.ActiveDocument;
            //Получаем доступ к файлу (COM)
            InwOpState10 cdoc = ComApiBridge.State;
            //Выбираем элементы
            ModelItemCollection selectionItems = doc.CurrentSelection.SelectedItems;
            //Забираем в коллекцию выбранные элементы
            ModelItemCollection newCollection = new ModelItemCollection();
            ModelItemCollection invertCollection = new ModelItemCollection();
            //Копируем все выбранные элементы в инвертируемую коллекцию
            invertCollection.CopyFrom(selectionItems);
            //Инвертируем коллекцию с выбранными элементами
            invertCollection.Invert(doc);

            //Проверка на наличие файла RolePC
            if (File.Exists(parth))
            {
                ReadRoleName = File.ReadLines(parth).Skip(0).First();
                ReadPersonName = File.ReadLines(parth).Skip(1).First();

                if (ReadRoleName != "Начальник участка (ИТР)")
                {
                    System.Windows.MessageBox.Show("У вас определена другая роль. Пожалуйста пользуйтесь функциями своей выбранной роли");
                    doc.Models.ResetPermanentMaterials(selectionItems);
                    return 0;
                }
                    //Собираем все выбранные файлы в box
                    foreach (ModelItem i in selectionItems)
                    {
                        //Получаем box
                        BoundingBox3D box1 = i.BoundingBox(true);

                        //преобразовать элемент модели в COM-путь
                        InwOaPath citem = (InwOaPath)ComApiBridge.ToInwOaPath(i);
                        // Get item's PropertyCategoryCollection
                        InwGUIPropertyNode2 cpropcates = (InwGUIPropertyNode2)cdoc.GetGUIPropertyNode(citem, true);
                        // create a new Category (PropertyDataCollection)

                        InwOaPropertyVec newPvec = cdoc.ObjectFactory(nwEObjectType.eObjectType_nwOaPropertyVec, null, null);

                        List<InwOaProperty> paramsModelItem = GenerateParamListITR("Role", "Status", "FIO","Date", ReadRoleName, ReadPersonName, cdoc);

                        string category = "ИТР";

                    List<string> Categories = new List<string>();
                        foreach (ComApi.InwGUIAttribute2 nwAtt in cpropcates.GUIAttributes())
                        {
                            if (!Categories.Contains(nwAtt.ClassUserName))
                            {
                                Categories.Add(nwAtt.ClassUserName);
                            }
                        }

                    int indecategory = 1;
                    foreach (ComApi.InwGUIAttribute2 nwAtt in cpropcates.GUIAttributes())
                    {
                        if (nwAtt.UserDefined)
                        {
                            if (nwAtt.ClassUserName == category)
                                break;
                            indecategory += 1;
                        }
                    }

                    if (Categories.Contains(category))
                        {
                        List<InwOaProperty> paramsModelItemInObjekt = new List<InwOaProperty>();

                        foreach (ComApi.InwGUIAttribute2 nwAtt in cpropcates.GUIAttributes())
                        {
                            if (nwAtt.ClassUserName.Equals(category))
                            {
                                foreach (InwOaProperty param in nwAtt.Properties())
                                {
                                    InwOaProperty TempProperty = (InwOaProperty)cdoc.ObjectFactory(nwEObjectType.eObjectType_nwOaProperty, null, null);
                                    TempProperty.name = param.name;
                                    TempProperty.value = param.value;
                                    paramsModelItemInObjekt.Add(TempProperty);
                                }
                            }
                        }

                        //Добавление новых параметров в истории 
                        paramsModelItemInObjekt.AddRange(GenerateParamListITR("Role", "Status", "FIO", "Date", ReadRoleName, ReadPersonName, cdoc));

                        foreach (InwOaProperty item in paramsModelItemInObjekt)
                            {
                                newPvec.Properties().Add(item);
                            }
                        cpropcates.RemoveUserDefined(indecategory);

                        cpropcates.SetUserDefined(0, "ИТР", "ITR_InteralName", newPvec);
                        //Добавляем синий свет в выбранные элементы
                        doc.Models.OverridePermanentColor(selectionItems, Autodesk.Navisworks.Api.Color.FromByteRGB(0, 102, 204));

                    }

                        else
                        {
                        //Создание новой катигории и добавление первоначальных параметров
                            foreach (InwOaProperty item in paramsModelItem)
                            {
                                newPvec.Properties().Add(item);

                            }
                        cpropcates.SetUserDefined(0, "ИТР", "ITR_InteralName", newPvec);
                        //Добавляем синий свет в выбранные элементы
                        doc.Models.OverridePermanentColor(selectionItems, Autodesk.Navisworks.Api.Color.FromByteRGB(0, 102, 204));
                    }
                    }
            }

            else
            {
                System.Windows.MessageBox.Show("Выберите роль!");
                FormAddRole FormAddRole = new FormAddRole();
                FormAddRole.Show();
            }

            //Добавляем синий свет в выбранные элементы

            return 0;
        }
    }
    

    internal class ITRCorrect
    {
        public string ReadRoleName;
        public string ReadPersonName;
        public string parth = @"C:\Users\Public\RolePC.txt";
        public bool g = true;

        public List<InwOaProperty> GenerateParamListITR(string role, string state, string fio, string date, string readrolename, string readpersonname, InwOpState10 cdoc_)
        {
            List<InwOaProperty> paramsModelItem = new List<InwOaProperty>();
            for (int k = 0; k < 4; k++)
            {
                InwOaProperty paramITR = (InwOaProperty)cdoc_.ObjectFactory(nwEObjectType.eObjectType_nwOaProperty, null, null);
                paramsModelItem.Add(paramITR);
            }

            DateTime Date = DateTime.Now;

            paramsModelItem[0].name = role;
            paramsModelItem[0].value = readrolename;
            paramsModelItem[1].name = fio;
            paramsModelItem[1].value = readpersonname;
            paramsModelItem[2].name = state;
            paramsModelItem[2].value = "Замечание устранено";
            paramsModelItem[3].name = date;
            paramsModelItem[3].value = Date.ToString();

            return paramsModelItem;
        }

        public int Execute(params string[] parameters)
        {

            //Получаем доступ к файлу 
            Document doc = Autodesk.Navisworks.Api.Application.ActiveDocument;
            //Получаем доступ к файлу (COM)
            InwOpState10 cdoc = ComApiBridge.State;
            //Выбираем элементы
            ModelItemCollection selectionItems = doc.CurrentSelection.SelectedItems;
            //Забираем в коллекцию выбранные элементы
            ModelItemCollection newCollection = new ModelItemCollection();
            ModelItemCollection invertCollection = new ModelItemCollection();
            //Копируем все выбранные элементы в инвертируемую коллекцию
            invertCollection.CopyFrom(selectionItems);
            //Инвертируем коллекцию с выбранными элементами
            invertCollection.Invert(doc);

            //Проверка на наличие файла RolePC
            if (File.Exists(parth))
            {
                ReadRoleName = File.ReadLines(parth).Skip(0).First();
                ReadPersonName = File.ReadLines(parth).Skip(1).First();

                if (ReadRoleName != "Начальник участка (ИТР)")
                {
                    System.Windows.MessageBox.Show("У вас определена другая роль. Пожалуйста пользуйтесь функциями своей выбранной роли");
                    doc.Models.ResetPermanentMaterials(selectionItems);
                    return 0;
                }
                //Собираем все выбранные файлы в box
                foreach (ModelItem i in selectionItems)
                {
                    //Получаем box
                    BoundingBox3D box1 = i.BoundingBox(true);

                    //преобразовать элемент модели в COM-путь
                    InwOaPath citem = (InwOaPath)ComApiBridge.ToInwOaPath(i);
                    // Get item's PropertyCategoryCollection
                    InwGUIPropertyNode2 cpropcates = (InwGUIPropertyNode2)cdoc.GetGUIPropertyNode(citem, true);
                    // create a new Category (PropertyDataCollection)

                    InwOaPropertyVec newPvec = cdoc.ObjectFactory(nwEObjectType.eObjectType_nwOaPropertyVec, null, null);

                    List<InwOaProperty> paramsModelItem = GenerateParamListITR("Role", "Status", "FIO", "Date", ReadRoleName, ReadPersonName, cdoc);

                    string category = "ИТР";

                    List<string> Categories = new List<string>();
                    foreach (ComApi.InwGUIAttribute2 nwAtt in cpropcates.GUIAttributes())
                    {
                        if (!Categories.Contains(nwAtt.ClassUserName))
                        {
                            Categories.Add(nwAtt.ClassUserName);
                        }
                    }

                    int indecategory = 1;
                    foreach (ComApi.InwGUIAttribute2 nwAtt in cpropcates.GUIAttributes())
                    {
                        if (nwAtt.UserDefined)
                        {
                            if (nwAtt.ClassUserName == category)
                                break;
                            indecategory += 1;
                        }
                    }

                    if (Categories.Contains(category))
                    {
                        List<InwOaProperty> paramsModelItemInObjekt = new List<InwOaProperty>();

                        foreach (ComApi.InwGUIAttribute2 nwAtt in cpropcates.GUIAttributes())
                        {
                            if (nwAtt.ClassUserName.Equals(category))
                            {
                                foreach (InwOaProperty param in nwAtt.Properties())
                                {
                                    InwOaProperty TempProperty = (InwOaProperty)cdoc.ObjectFactory(nwEObjectType.eObjectType_nwOaProperty, null, null);
                                    TempProperty.name = param.name;
                                    TempProperty.value = param.value;
                                    paramsModelItemInObjekt.Add(TempProperty);
                                }
                            }
                        }
                        //Добавление новых параметров в истории 
                        paramsModelItemInObjekt.AddRange(GenerateParamListITR("Role", "Status", "FIO", "Date", ReadRoleName, ReadPersonName, cdoc));
                        foreach (InwOaProperty item in paramsModelItemInObjekt)
                        {
                            newPvec.Properties().Add(item);
                        }
                        cpropcates.RemoveUserDefined(indecategory);

                        cpropcates.SetUserDefined(0, "ИТР", "ITR_InteralName", newPvec);
                        //Добавляем оранжевый свет в выбранные элементы
                        doc.Models.OverridePermanentColor(selectionItems, Autodesk.Navisworks.Api.Color.FromByteRGB(255, 128, 0));

                    }

                    else
                    {
                        //Создание новой катигории и добавление первоначальных параметров
                        foreach (InwOaProperty item in paramsModelItem)
                        {
                            newPvec.Properties().Add(item);

                        }
                        cpropcates.SetUserDefined(0, "ИТР", "ITR_InteralName", newPvec);
                        //Добавляем оранжевый свет в выбранные элементы
                        doc.Models.OverridePermanentColor(selectionItems, Autodesk.Navisworks.Api.Color.FromByteRGB(255, 128, 0));
                    }
                }
            }

            else
            {
                System.Windows.MessageBox.Show("Выберите роль!");
                FormAddRole FormAddRole = new FormAddRole();
                FormAddRole.Show();
            }

            //Добавляем синий свет в выбранные элементы

            return 0;
        }
    }

    internal class USKAdd
    {
        public string ReadRoleName;
        public string ReadPersonName;
        public string parth = @"C:\Users\Public\RolePC.txt";
        public bool g = true;

        public List<InwOaProperty> GenerateParamListUSK(string role, string state, string fio, string date, string readrolename, string readpersonname, InwOpState10 cdoc_)
        {
            List<InwOaProperty> paramsModelItem = new List<InwOaProperty>();
            for (int k = 0; k < 4; k++)
            {
                InwOaProperty paramUSK = (InwOaProperty)cdoc_.ObjectFactory(nwEObjectType.eObjectType_nwOaProperty, null, null);
                paramsModelItem.Add(paramUSK);
            }

            DateTime Date = DateTime.Now;

            paramsModelItem[0].name = role;
            paramsModelItem[0].value = readrolename;
            paramsModelItem[1].name = fio;
            paramsModelItem[1].value = readpersonname;
            paramsModelItem[2].name = state;
            paramsModelItem[2].value = "Принято";
            paramsModelItem[3].name = date;
            paramsModelItem[3].value = Date.ToString();

            return paramsModelItem;
        }


        public int Execute(params string[] parameters)
        {

            //Получаем доступ к файлу 
            Document doc = Autodesk.Navisworks.Api.Application.ActiveDocument;
            //Получаем доступ к файлу (COM)
            InwOpState10 cdoc = ComApiBridge.State;
            //Выбираем элементы
            ModelItemCollection selectionItems = doc.CurrentSelection.SelectedItems;
            //Забираем в коллекцию выбранные элементы
            ModelItemCollection newCollection = new ModelItemCollection();
            ModelItemCollection invertCollection = new ModelItemCollection();
            //Копируем все выбранные элементы в инвертируемую коллекцию
            invertCollection.CopyFrom(selectionItems);
            //Инвертируем коллекцию с выбранными элементами
            invertCollection.Invert(doc);

            //Проверка на наличие файла RolePC
            if (File.Exists(parth))
            {
                ReadRoleName = File.ReadLines(parth).Skip(0).First();
                ReadPersonName = File.ReadLines(parth).Skip(1).First();

                if (ReadRoleName != "Инженер УСК")
                {
                    System.Windows.MessageBox.Show("У вас определена другая роль. Пожалуйста пользуйтесь функциями своей выбранной роли");
                    doc.Models.ResetPermanentMaterials(selectionItems);
                    return 0;
                }



                //Собираем все выбранные файлы в box
                foreach (ModelItem i in selectionItems)
                {
                    //Получаем box
                    BoundingBox3D box1 = i.BoundingBox(true);

                    //преобразовать элемент модели в COM-путь
                    InwOaPath citem = (InwOaPath)ComApiBridge.ToInwOaPath(i);
                    // Get item's PropertyCategoryCollection
                    InwGUIPropertyNode2 cpropcates = (InwGUIPropertyNode2)cdoc.GetGUIPropertyNode(citem, true);
                    // create a new Category (PropertyDataCollection)

                    InwOaPropertyVec newPvec = cdoc.ObjectFactory(nwEObjectType.eObjectType_nwOaPropertyVec, null, null);

                    List<InwOaProperty> paramsModelItem = GenerateParamListUSK("Role","Status", "FIO", "Date", ReadRoleName, ReadPersonName, cdoc);

                    string category = "УСК";

                    List<string> Categories = new List<string>();
                    foreach (ComApi.InwGUIAttribute2 nwAtt in cpropcates.GUIAttributes())
                    {
                        if (!Categories.Contains(nwAtt.ClassUserName))
                        {
                            Categories.Add(nwAtt.ClassUserName);
                        }
                    }

                    int indecategory = 1;
                    foreach (ComApi.InwGUIAttribute2 nwAtt in cpropcates.GUIAttributes())
                    {
                        if (nwAtt.UserDefined)
                        {
                            if (nwAtt.ClassUserName == category)
                                break;
                            indecategory += 1;
                        }
                    }

                    if (Categories.Contains(category))
                    {
                        List<InwOaProperty> paramsModelItemInObjekt = new List<InwOaProperty>();

                        foreach (ComApi.InwGUIAttribute2 nwAtt in cpropcates.GUIAttributes())
                        {
                            if (nwAtt.ClassUserName.Equals(category))
                            {
                                foreach (InwOaProperty param in nwAtt.Properties())
                                {
                                    InwOaProperty TempProperty = (InwOaProperty)cdoc.ObjectFactory(nwEObjectType.eObjectType_nwOaProperty, null, null);
                                    TempProperty.name = param.name;
                                    TempProperty.value = param.value;
                                    paramsModelItemInObjekt.Add(TempProperty);
                                }
                            }
                        }
                        //Добавление новых параметров в истории 
                        paramsModelItemInObjekt.AddRange(GenerateParamListUSK("Role", "Status", "FIO", "Date", ReadRoleName, ReadPersonName, cdoc));
                        foreach (InwOaProperty item in paramsModelItemInObjekt)
                        {
                            newPvec.Properties().Add(item);
                        }
                        cpropcates.RemoveUserDefined(indecategory);

                        cpropcates.SetUserDefined(0, "УСК", "USK_InteralName", newPvec);
                        //Добавляем зеленого свет в выбранные элементы
                        doc.Models.OverridePermanentColor(selectionItems, Autodesk.Navisworks.Api.Color.FromByteRGB(0, 204, 0));

                    }

                    else
                    {
                        //Создание новой катигории и добавление первоначальных параметров
                        foreach (InwOaProperty item in paramsModelItem)
                        {
                            newPvec.Properties().Add(item);

                        }
                        cpropcates.SetUserDefined(0, "УСК", "USK_InteralName", newPvec);
                        //Добавляем зеленого свет в выбранные элементы
                        doc.Models.OverridePermanentColor(selectionItems, Autodesk.Navisworks.Api.Color.FromByteRGB(0, 204, 0));
                    }
                }
            }

            else
            {
                System.Windows.MessageBox.Show("Выберите роль!");
                FormAddRole FormAddRole = new FormAddRole();
                FormAddRole.Show();
            }

            //Добавляем синий свет в выбранные элементы

            return 0;
        }
    }

    internal class USKRemark
    {
        public string ReadRoleName;
        public string ReadPersonName;
        public string parth = @"C:\Users\Public\RolePC.txt";

 
        public List<InwOaProperty> GenerateParamListUSK(string role, string state, string fio, string remark, string remarktext, string date, string readrolename, string readpersonname, InwOpState10 cdoc_)
        {
            List<InwOaProperty> paramsModelItem = new List<InwOaProperty>();
            for (int k = 0; k < 5; k++)
            {
                InwOaProperty paramUSK = (InwOaProperty)cdoc_.ObjectFactory(nwEObjectType.eObjectType_nwOaProperty, null, null);
                paramsModelItem.Add(paramUSK);
            }

            DateTime Date = DateTime.Now;

            paramsModelItem[0].name = role;
            paramsModelItem[0].value = readrolename;
            paramsModelItem[1].name = fio;
            paramsModelItem[1].value = readpersonname;
            paramsModelItem[2].name = state;
            paramsModelItem[2].value = "Замечание";
            paramsModelItem[3].name = remark;
            paramsModelItem[3].value = remarktext;
            paramsModelItem[4].name = date;
            paramsModelItem[4].value = Date.ToString();

            return paramsModelItem;
        }

        public int Execute(params string[] parameters)
        {

            //Получаем доступ к файлу 
            Document doc = Autodesk.Navisworks.Api.Application.ActiveDocument;
            //Получаем доступ к файлу (COM)
            InwOpState10 cdoc = ComApiBridge.State;
            //Выбираем элементы
            ModelItemCollection selectionItems = doc.CurrentSelection.SelectedItems;
            //Забираем в коллекцию выбранные элементы
            ModelItemCollection newCollection = new ModelItemCollection();
            ModelItemCollection invertCollection = new ModelItemCollection();
            //Копируем все выбранные элементы в инвертируемую коллекцию
            invertCollection.CopyFrom(selectionItems);
            //Инвертируем коллекцию с выбранными элементами
            invertCollection.Invert(doc);



            //Проверка на наличие файла RolePC
            if (File.Exists(parth))
            {
                ReadRoleName = File.ReadLines(parth).Skip(0).First();
                ReadPersonName = File.ReadLines(parth).Skip(1).First();
                if (ReadRoleName != "Инженер УСК")
                {
                    System.Windows.MessageBox.Show("У вас определена другая роль. Пожалуйста пользуйтесь функциями своей выбранной роли");
                    doc.Models.ResetPermanentMaterials(selectionItems);
                    return 0;
                }

                FormRemark FormRemark = new FormRemark();
                FormRemark.ShowDialog();

                if (FormRemark.Remark.Trim() == "") return 0;

                //Собираем все выбранные файлы в box
                foreach (ModelItem i in selectionItems)
                {
                    //Получаем box
                    BoundingBox3D box1 = i.BoundingBox(true);

                    //преобразовать элемент модели в COM-путь
                    InwOaPath citem = (InwOaPath)ComApiBridge.ToInwOaPath(i);
                    // Get item's PropertyCategoryCollection
                    InwGUIPropertyNode2 cpropcates = (InwGUIPropertyNode2)cdoc.GetGUIPropertyNode(citem, true);
                    // create a new Category (PropertyDataCollection)

                    InwOaPropertyVec newPvec = cdoc.ObjectFactory(nwEObjectType.eObjectType_nwOaPropertyVec, null, null);

                    List<InwOaProperty> paramsModelItem = GenerateParamListUSK("Role", "Status", "FIO","Remark",FormRemark.Remark, "Date", ReadRoleName, ReadPersonName, cdoc);

                    string category = "УСК";

                    List<string> Categories = new List<string>();
                    foreach (ComApi.InwGUIAttribute2 nwAtt in cpropcates.GUIAttributes())
                    {
                        if (!Categories.Contains(nwAtt.ClassUserName))
                        {
                            Categories.Add(nwAtt.ClassUserName);
                        }
                    }

                    int indecategory = 1;
                    foreach (ComApi.InwGUIAttribute2 nwAtt in cpropcates.GUIAttributes())
                    {
                        if (nwAtt.UserDefined)
                        {
                            if (nwAtt.ClassUserName == category)
                                break;
                            indecategory += 1;
                        }
                    }

                    if (Categories.Contains(category))
                    {
                        List<InwOaProperty> paramsModelItemInObjekt = new List<InwOaProperty>();

                        foreach (ComApi.InwGUIAttribute2 nwAtt in cpropcates.GUIAttributes())
                        {
                            if (nwAtt.ClassUserName.Equals(category))
                            {
                                foreach (InwOaProperty param in nwAtt.Properties())
                                {
                                    InwOaProperty TempProperty = (InwOaProperty)cdoc.ObjectFactory(nwEObjectType.eObjectType_nwOaProperty, null, null);
                                    TempProperty.name = param.name;
                                    TempProperty.value = param.value;
                                    paramsModelItemInObjekt.Add(TempProperty);
                                }
                            }
                        }
                        //Добавление новых параметров в истории 
                        paramsModelItemInObjekt.AddRange(GenerateParamListUSK("Role", "Status", "FIO", "Remark", FormRemark.Remark, "Date", ReadRoleName, ReadPersonName, cdoc));

                        foreach (InwOaProperty item in paramsModelItemInObjekt)
                        {
                            newPvec.Properties().Add(item);
                        }
                        cpropcates.RemoveUserDefined(indecategory);

                        cpropcates.SetUserDefined(0, "УСК", "USK_InteralName", newPvec);
                        //Добавляем зеленого свет в выбранные элементы
                        doc.Models.OverridePermanentColor(selectionItems, Autodesk.Navisworks.Api.Color.FromByteRGB(255, 0, 0));
                    }

                    else
                    {
                        //Создание новой катигории и добавление первоначальных параметров
                        foreach (InwOaProperty item in paramsModelItem)
                        {
                         
                            newPvec.Properties().Add(item);

                        }
                        cpropcates.SetUserDefined(0, "УСК", "USK_InteralName", newPvec);
                        //Добавляем красный свет в выбранные элементы
                        doc.Models.OverridePermanentColor(selectionItems, Autodesk.Navisworks.Api.Color.FromByteRGB(255, 0, 0));
                    }
                }
            }

            else
            {
                System.Windows.MessageBox.Show("Выберите роль!");
                FormAddRole FormAddRole = new FormAddRole();
                FormAddRole.Show();
            }

            //Добавляем синий свет в выбранные элементы

            return 0;
        }
    }





    internal class AddColor1
    {
        public string ReadRoleName;
        public string ReadPersonName;

        public int Execute(params string[] parameters)
        {

            //Получаем доступ к файлу 
            Document doc = Autodesk.Navisworks.Api.Application.ActiveDocument;
            //Получаем доступ к файлу (COM)
            InwOpState10 cdoc = ComApiBridge.State;
            //Выбираем элементы
            ModelItemCollection selectionItems = doc.CurrentSelection.SelectedItems;
            //Забираем в коллекцию выбранные элементы
            ModelItemCollection newCollection = new ModelItemCollection();
            ModelItemCollection invertCollection = new ModelItemCollection();
            //Копируем все выбранные элементы в инвертируемую коллекцию
            invertCollection.CopyFrom(selectionItems);
            //Инвертируем коллекцию с выбранными элементами
            invertCollection.Invert(doc);


            ReadRoleName = File.ReadLines(@"C:\Work\RolePC.txt").Skip(0).First();
            ReadPersonName = File.ReadLines(@"C:\Work\RolePC.txt").Skip(1).First();


            foreach (ModelItem i in selectionItems)
            {
                //Получаем box
                BoundingBox3D box1 = i.BoundingBox(true);

                //преобразовать элемент модели в COM-путь
                InwOaPath citem = (InwOaPath)ComApiBridge.ToInwOaPath(i);
                // Get item's PropertyCategoryCollection
                InwGUIPropertyNode2 cpropcates = (InwGUIPropertyNode2)cdoc.GetGUIPropertyNode(citem, true);
                // create a new Category (PropertyDataCollection)

                InwOaPropertyVec newPvec = cdoc.ObjectFactory(nwEObjectType.eObjectType_nwOaPropertyVec, null, null);
                InwOaProperty param1 = (InwOaProperty)cdoc.ObjectFactory(nwEObjectType.eObjectType_nwOaProperty, null, null);
                InwOaProperty param2 = (InwOaProperty)cdoc.ObjectFactory(nwEObjectType.eObjectType_nwOaProperty, null, null);
                InwOaProperty param3 = (InwOaProperty)cdoc.ObjectFactory(nwEObjectType.eObjectType_nwOaProperty, null, null);
                InwOaProperty param4 = (InwOaProperty)cdoc.ObjectFactory(nwEObjectType.eObjectType_nwOaProperty, null, null);

                DateTime Date = DateTime.Today;
                string category = "УСК";
                param1.name = "Роль";
                param1.UserName = "Роль";
                param2.name = "ФИО";
                param2.UserName = "ФИО";
                param3.name = "Статус";
                param3.UserName = "Статус";
                param4.name = "Дата";
                param4.UserName = "Дата";

                int result;
                bool success = int.TryParse("Выполнено", out result);

                if (success)
                {
                    param1.value = result;
                    param2.value = result;
                    param3.value = result;
                    param4.value = result;
                }
                else
                {
                    param1.value = ReadRoleName;
                    param2.value = ReadPersonName;
                    param3.value = "Выполнено";
                    param4.value = Date.ToString("dd/MM/yyyy");
                }

                //if custom tub already exist - delete it
                int index = 1;
                foreach (ComApi.InwGUIAttribute2 nwAtt in cpropcates.GUIAttributes())
                {
                    if (nwAtt.UserDefined)
                    {
                        if (nwAtt.ClassUserName == category)
                            break;
                        index += 1;
                    }
                }

                foreach (ComApi.InwGUIAttribute2 nwAtt in cpropcates.GUIAttributes())
                {
                    if (nwAtt.ClassUserName == category)
                    {
                        foreach (InwOaProperty nwProp in nwAtt.Properties())
                        {
                            InwOaProperty nwNewProp = (InwOaProperty)cdoc.ObjectFactory(nwEObjectType.eObjectType_nwOaProperty, null, null);

                            if (param1.name != nwProp.name)
                            {
                                nwNewProp.name = nwProp.name;
                                nwNewProp.value = nwProp.value;

                                newPvec.Properties().Add(nwNewProp);
                            }
                        }
                        cpropcates.RemoveUserDefined(index);
                        break;
                    }
                }
                newPvec.Properties().Add(param1);
                newPvec.Properties().Add(param2);
                newPvec.Properties().Add(param3);
                newPvec.Properties().Add(param4);

                //Добавляем параметры и задаем имя категории параметров
                cpropcates.SetUserDefined(0, "УСК", "USK_InteralName", newPvec);
            }
            //Добавляем желтый свет в выбранные элементы

            doc.Models.OverridePermanentColor(selectionItems, Autodesk.Navisworks.Api.Color.FromByteRGB(32, 178, 170));

            return 0;
        }
    }
    internal class AddColor2
        {
            public int Execute(params string[] parameters)
            {
                //Получаем доступ к файлу 
                Document doc = Autodesk.Navisworks.Api.Application.ActiveDocument;
                //Получаем доступ к файлу (COM)
                InwOpState10 cdoc = ComApiBridge.State;
                //Выбираем элементы
                ModelItemCollection selectionItems = doc.CurrentSelection.SelectedItems;
                //Забираем в коллекцию выбранные элементы
                ModelItemCollection newCollection = new ModelItemCollection();
                ModelItemCollection invertCollection = new ModelItemCollection();
                //Копируем все выбранные элементы в инвертируемую коллекцию
                invertCollection.CopyFrom(selectionItems);
                //Инвертируем коллекцию с выбранными элементами
                invertCollection.Invert(doc);

                foreach (ModelItem i in selectionItems)
                {
                    //Получаем box
                    BoundingBox3D box1 = i.BoundingBox(true);

                    //преобразовать элемент модели в COM-путь
                    InwOaPath citem = (InwOaPath)ComApiBridge.ToInwOaPath(i);
                    // Get item's PropertyCategoryCollection
                    InwGUIPropertyNode2 cpropcates = (InwGUIPropertyNode2)cdoc.GetGUIPropertyNode(citem, true);
                    // create a new Category (PropertyDataCollection)

                    ComApi.InwOaPropertyVec newPvec = cdoc.ObjectFactory(nwEObjectType.eObjectType_nwOaPropertyVec, null, null);
                    ComApi.InwOaProperty FirstP = (ComApi.InwOaProperty)cdoc.ObjectFactory(ComApi.nwEObjectType.eObjectType_nwOaProperty, null, null);

                    FirstP.name = "УСК";
                    FirstP.UserName = "Статус";
                    string category = "УСК";

                    int result;
                    bool success = int.TryParse("Замечание", out result);

                    if (success)
                    {
                        FirstP.value = result;
                    }
                    else
                    {
                        FirstP.value = "Замечание";
                    }

                    //if custom tub already exist - delete it
                    int index = 1;
                    foreach (ComApi.InwGUIAttribute2 nwAtt in cpropcates.GUIAttributes())
                    {
                        if (nwAtt.UserDefined)
                        {
                            if (nwAtt.ClassUserName == category)
                                break;
                            index += 1;
                        }
                    }
                    foreach (ComApi.InwGUIAttribute2 nwAtt in cpropcates.GUIAttributes())
                    {
                        if (nwAtt.ClassUserName == category)
                        {
                            foreach (ComApi.InwOaProperty nwProp in nwAtt.Properties())
                            {
                                ComApi.InwOaProperty nwNewProp = (ComApi.InwOaProperty)cdoc.ObjectFactory(ComApi.nwEObjectType.eObjectType_nwOaProperty, null, null);

                                if (FirstP.name != nwProp.name)
                                {
                                    nwNewProp.name = nwProp.name;
                                    nwNewProp.value = nwProp.value;

                                    newPvec.Properties().Add(nwNewProp);
                                }
                            }
                            cpropcates.RemoveUserDefined(index);
                            break;
                        }
                    }
                    newPvec.Properties().Add(FirstP);

                    //Добавляем параметры и задаем имя категории параметров
                    cpropcates.SetUserDefined(0, "УСК", "USK_InteralName", newPvec);
                }
                //Добавляем желтый свет в выбранные элементы

                doc.Models.OverridePermanentColor(selectionItems, Autodesk.Navisworks.Api.Color.FromByteRGB(255, 255, 0));

                return 0;
            }
        }
    internal class AddColor3
        {
            public int Execute(params string[] parameters)
            {
                //Получаем доступ к файлу 
                Document doc = Autodesk.Navisworks.Api.Application.ActiveDocument;
                //Получаем доступ к файлу (COM)
                InwOpState10 cdoc = ComApiBridge.State;
                //Выбираем элементы
                ModelItemCollection selectionItems = doc.CurrentSelection.SelectedItems;
                //Забираем в коллекцию выбранные элементы
                ModelItemCollection newCollection = new ModelItemCollection();
                ModelItemCollection invertCollection = new ModelItemCollection();
                //Копируем все выбранные элементы в инвертируемую коллекцию
                invertCollection.CopyFrom(selectionItems);
                //Инвертируем коллекцию с выбранными элементами
                invertCollection.Invert(doc);

                foreach (ModelItem i in selectionItems)
                {
                    //Получаем box
                    BoundingBox3D box1 = i.BoundingBox(true);

                    //преобразовать элемент модели в COM-путь
                    InwOaPath citem = (InwOaPath)ComApiBridge.ToInwOaPath(i);
                    // Get item's PropertyCategoryCollection
                    InwGUIPropertyNode2 cpropcates = (InwGUIPropertyNode2)cdoc.GetGUIPropertyNode(citem, true);
                    // create a new Category (PropertyDataCollection)

                    ComApi.InwOaPropertyVec newPvec = cdoc.ObjectFactory(nwEObjectType.eObjectType_nwOaPropertyVec, null, null);
                    ComApi.InwOaProperty FirstP = (ComApi.InwOaProperty)cdoc.ObjectFactory(ComApi.nwEObjectType.eObjectType_nwOaProperty, null, null);

                    FirstP.name = "УСК";
                    FirstP.UserName = "Статус";
                    string category = "УСК";

                    int result;
                    bool success = int.TryParse("Принято", out result);

                    if (success)
                    {
                        FirstP.value = result;
                    }
                    else
                    {
                        FirstP.value = "Принято";
                    }

                    //if custom tub already exist - delete it
                    int index = 1;
                    foreach (ComApi.InwGUIAttribute2 nwAtt in cpropcates.GUIAttributes())
                    {
                        if (nwAtt.UserDefined)
                        {
                            if (nwAtt.ClassUserName == category)
                                break;
                            index += 1;
                        }
                    }
                    foreach (ComApi.InwGUIAttribute2 nwAtt in cpropcates.GUIAttributes())
                    {
                        if (nwAtt.ClassUserName == category)
                        {
                            foreach (ComApi.InwOaProperty nwProp in nwAtt.Properties())
                            {
                                ComApi.InwOaProperty nwNewProp = (ComApi.InwOaProperty)cdoc.ObjectFactory(ComApi.nwEObjectType.eObjectType_nwOaProperty, null, null);

                                if (FirstP.name != nwProp.name)
                                {
                                    nwNewProp.name = nwProp.name;
                                    nwNewProp.value = nwProp.value;

                                    newPvec.Properties().Add(nwNewProp);
                                }
                            }
                            cpropcates.RemoveUserDefined(index);
                            break;
                        }
                    }
                    newPvec.Properties().Add(FirstP);

                    //Добавляем параметры и задаем имя категории параметров
                    cpropcates.SetUserDefined(0, "УСК", "USK_InteralName", newPvec);
                }
                //Добавляем зеленый свет в выбранные элементы
                doc.Models.OverridePermanentColor(selectionItems, Autodesk.Navisworks.Api.Color.FromByteRGB(0, 128, 0));

                return 0;
            }
        }
    internal class ResetColorEl
        {
            public int Execute(params string[] parameters)
            {

                //Получаем доступ к файлу 
                Document doc = Autodesk.Navisworks.Api.Application.ActiveDocument;
                //Получаем доступ к файлу (COM)
                InwOpState10 cdoc = ComApiBridge.State;
                //Выбираем элементы
                ModelItemCollection selectionItems = doc.CurrentSelection.SelectedItems;
                //Забираем в коллекцию выбранные элементы
                ModelItemCollection newCollection = new ModelItemCollection();
                ModelItemCollection invertCollection = new ModelItemCollection();
                //Копируем все выбранные элементы в инвертируемую коллекцию
                invertCollection.CopyFrom(selectionItems);
                //Инвертируем коллекцию с выбранными элементами
                invertCollection.Invert(doc);

                foreach (ModelItem i in selectionItems)
                {
                    //Получаем box
                    BoundingBox3D box1 = i.BoundingBox(true);
                    //преобразовать элемент модели в COM-путь
                    InwOaPath citem = (InwOaPath)ComApiBridge.ToInwOaPath(i);
                    // Get item's PropertyCategoryCollection
                    InwGUIPropertyNode2 cpropcates = (InwGUIPropertyNode2)cdoc.GetGUIPropertyNode(citem, true);
                    // create a new Category (PropertyDataCollection)

                   InwOaPropertyVec newPvec = cdoc.ObjectFactory(nwEObjectType.eObjectType_nwOaPropertyVec, null, null);
                   InwOaProperty FirstP = (InwOaProperty)cdoc.ObjectFactory(ComApi.nwEObjectType.eObjectType_nwOaProperty, null, null);
                    FirstP.name = "УСК";
                    string category = "УСК";

                    int index = 1;
                    foreach (ComApi.InwGUIAttribute2 nwAtt in cpropcates.GUIAttributes())
                    {
                        if (nwAtt.UserDefined)
                        {
                            if (nwAtt.ClassUserName == category)
                                break;
                            index += 1;
                        }
                    }
                    foreach (InwGUIAttribute2 nwAtt in cpropcates.GUIAttributes())
                    {
                        if (nwAtt.ClassUserName == category)
                        {
                            foreach (InwOaProperty nwProp in nwAtt.Properties())
                            {
                                InwOaProperty nwNewProp = (InwOaProperty)cdoc.ObjectFactory(nwEObjectType.eObjectType_nwOaProperty, null, null);

                                if (FirstP.name != nwProp.name)
                                {
                                    nwNewProp.name = nwProp.name;
                                    nwNewProp.value = nwProp.value;

                                    newPvec.Properties().Add(nwNewProp);
                                }
                            }
                            cpropcates.RemoveUserDefined(index);
                            break;
                        }
                    }
                }
                //Удаление всех постоянных параметров цвета и прозрачности во всех элементах модели
                doc.Models.ResetPermanentMaterials(selectionItems);

                return 0;
            }
        }
    internal class ResetColorAll
        {
        public int Execute(params string[] parameters)
        {
            //Получаем доступ к файлу 
            Document doc = Autodesk.Navisworks.Api.Application.ActiveDocument;
            //Получаем доступ к файлу(COM)
            InwOpState10 cdoc = ComApiBridge.State;
            Search search = new Search();
            search.Selection.SelectAll();
            //SearchCondition condition = SearchCondition.HasPropertyByDisplayName("Объект", "Имя");
            search.SearchConditions.Add(SearchCondition.HasCategoryByName(PropertyCategoryNames.Geometry));
            //search.SearchConditions.Add(condition);

            ModelItemCollection oModelColl = search.FindAll(doc,true);
             

            if (oModelColl.Count > 0)

            {
                System.Windows.MessageBox.Show("Кол-во "+ oModelColl.Count.ToString(), "Количество выбранных элементов");
            }

            //Забираем в коллекцию выбранные элементы
            ModelItemCollection newCollection = new ModelItemCollection();
            ModelItemCollection invertCollection = new ModelItemCollection();
            //Копируем все выбранные элементы в инвертируемую коллекцию
            invertCollection.CopyFrom(oModelColl);
            //Инвертируем коллекцию с выбранными элементами
            invertCollection.Invert(doc);

            foreach (ModelItem i in oModelColl)
            {
                //Получаем box
                BoundingBox3D box1 = i.BoundingBox(true);
                //преобразовать элемент модели в COM-путь
                InwOaPath citem = (InwOaPath)ComApiBridge.ToInwOaPath(i);
                // Get item's PropertyCategoryCollection
                InwGUIPropertyNode2 cpropcates = (InwGUIPropertyNode2)cdoc.GetGUIPropertyNode(citem, true);
                // create a new Category (PropertyDataCollection)

                InwOaPropertyVec newPvec = cdoc.ObjectFactory(nwEObjectType.eObjectType_nwOaPropertyVec, null, null);
                InwOaProperty FirstP = (InwOaProperty)cdoc.ObjectFactory(nwEObjectType.eObjectType_nwOaProperty, null, null);

                FirstP.name = "УСК";
                string category = "УСК";

                int index = 1;
                foreach (ComApi.InwGUIAttribute2 nwAtt in cpropcates.GUIAttributes())
                {
                    if (nwAtt.UserDefined)
                    {
                        if (nwAtt.ClassUserName == category)
                            break;
                        index += 1;
                    }
                }

                foreach (ComApi.InwGUIAttribute2 nwAtt in cpropcates.GUIAttributes())
                {
                    if (nwAtt.ClassUserName == category)
                    {
                        cpropcates.RemoveUserDefined(index);
                    }
                    break;
                }
            }
                //Удаление всех постоянных параметров цвета и прозрачности во всех элементах модели
            doc.Models.ResetPermanentMaterials(oModelColl); 

                return 0;

            }
        }

    }


