using Autodesk.Navisworks.Api.ComApi;
using Autodesk.Navisworks.Api;
using System;
using App = Autodesk.Navisworks.Api.Application;
using Pl = Autodesk.Navisworks.Api.Plugins;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using ComApi = Autodesk.Navisworks.Api.Interop.ComApi;
using ComApiBridge = Autodesk.Navisworks.Api.ComApi;
using System.Windows.Documents;
using Newtonsoft.Json.Linq;
using static System.Collections.Specialized.BitVector32;
using Microsoft.Office.Interop.Excel;
using static CDS_Plugin.Quantification.CreateQuantification.ApiUtility;
using Autodesk.Navisworks.Api.Plugins;

namespace CDS_Plugin.Quantification.CreateQuantification
{
    public partial class CreateQuantification_AR : Form
    {
        public CreateQuantification_AR()
        {
            InitializeComponent();
        }

        private void lb_cat_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        private bool Count_section_OK ()
        {
            if (Box_countSection.Text.Length == 0)
            {  return false; }

            int section = Convert.ToInt32(Box_countSection.Text);
            if (section >0)
            {
                return true;
            }
            MessageBox.Show("Введите количество секций больше 0");
            return false;
        }
        private bool Name_OK()
        {
            //Делаем проверки по наполнению text box
            if (textBox_nameObject.Text.Length == 0)
            {
                MessageBox.Show("Заполните поле Объект");
                return false; }


            if (text_cat.Text.Length == 0)
            {
                MessageBox.Show("Заполните поле Категория");
                return false; }

            if (text_param.Text.Length == 0)
            {
                MessageBox.Show("Заполните поле Параметр");
                return false; }

            return true;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            //Определяем основные переменные для дальнейшего использования
            if (!Count_section_OK()){
                MessageBox.Show("Не так в секции"); return; }
            if (!Name_OK()) {
                MessageBox.Show("Не так в именем"); return; }

                string folder = textBox_nameObject.Text;
                string category_get = text_cat.Text;
                string category = "Объект";
                string property_floor = text_param.Text;
                string property_section = "ADSK_Номер секции";
                string property_category = "Категория";
                string property_material = "Материал";
                string property_type = "Тип";
                string floorstr = " этаж";
                string sectionstr = "С ";
                Visible = false;
                Document activeDoc = App.ActiveDocument;
                var curSelect = activeDoc.CurrentSelection;
                var selectionSets = activeDoc.SelectionSets;
                int sections = Convert.ToInt32(Box_countSection.Text);
                int setsValues = selectionSets.Value.IndexOfDisplayName(folder);

            try
            {             
                //Если подобная папка уже есть, то удаляем
                if (setsValues != -1)
                {
                    selectionSets.RemoveAt(selectionSets.RootItem, setsValues);
                }
                selectionSets.AddCopy(new FolderItem() { DisplayName = folder });

                for (int i = 0; i < sections; i++)
                {
                    //Создаем список параметров стадий из проекта, фильтруем уникальные значения
                    List<string> List_sections = getDataForProp(category_get, property_section).Distinct().ToList();
                    //Создаем список параметров этажей из проекта, фильтруем уникальные значения
                    List<string> List_floors = getDataForProp(category_get, property_floor).Distinct().ToList();
                    int lastfloor = List_floors.Count();//Получаем общее кол-во элементов в листе
                    //Создаем родительскую папку с названием проекта
                    FolderItem pFold = selectionSets.Value.FirstOrDefault(x => x.DisplayName == folder) as FolderItem;
                    //Создаем папку с именем секции первого вложения в родительскую папку
                    FolderItem sFolder = new FolderItem();//Папка секций
                    sFolder.DisplayName = sectionstr + List_sections[i].ToString();
                    selectionSets.InsertCopy((GroupItem)pFold, 0, sFolder);
                    //Проходим по всему списку этажей
                    for (int j = 0; j < lastfloor; j++)
                    {
                        //Создаем список параметров категорий из проекта, фильтруем уникальные значения
                        List<string> List_category = getDataForProp(category, property_category).Distinct().ToList();

                        FolderItem FloorsFolder = new FolderItem(); //Папка этажей
                        FloorsFolder.DisplayName = List_floors[j].ToString() + floorstr;
                        //Создаем поисковый набор по этажам и секциям, чтобы икслючить этажи в секциях, в которых нет объектов
                        var search_floor = new Search();
                        search_floor.Locations = SearchLocations.DescendantsAndSelf;
                        search_floor.Selection.SelectAll();
                        SearchCondition search_section = SearchCondition.HasPropertyByDisplayName(category_get, property_section);//Условие поиска имеет параметр ADSK_Номер секции 
                        SearchCondition search_floors = SearchCondition.HasPropertyByDisplayName(category_get, property_floor);//Условие поиска имеет параметр ADSK_Этаж 
                        search_floor.SearchConditions.Add(search_section.EqualValue(VariantData.FromDisplayString(List_sections[i].ToString())));//Добавляем в поиск значение секции 
                        search_floor.SearchConditions.Add(search_floors.EqualValue(VariantData.FromDisplayString(List_floors[j].ToString())));//Добавляем в поиск значение этажа 
                        //Если поисковый набор равен нуля, ничего не создаем
                        if (search_floor.FindAll(App.ActiveDocument, true).Count == 0)
                        { continue; }
                        //Иначе, создаем папку с этажом
                        selectionSets.InsertCopy((GroupItem)pFold.Children[0], 0, FloorsFolder);
                        //Проходим по всему списку категорий
                        for (int k = 0; k < List_category.Count(); k++)
                        {
                            FolderItem CategoryFolder = new FolderItem(); //Папка категорий
                            CategoryFolder.DisplayName = List_category[k].ToString();
                            var FloorsFolder_Chil = pFold.Children[0] as FolderItem; //Создаем папку категорий
                            //Если у нас конкретная категория существует на этаже и в секции, то создаем папку
                            if (Check_search_category(category_get, category, property_section,
                                   property_floor, property_category, List_sections,
                                   List_category, i, List_floors, j, k))
                            {
                                selectionSets.InsertCopy((GroupItem)FloorsFolder_Chil.Children[0], 0, CategoryFolder);
                            }
                            //Собираем лист рабочих наборов из проекта, фильтруем уникальные значения
                            List<string> List_workingset = getDataForProp(category, "Рабочий набор").Distinct().ToList();
                            //Проходим по всему списку рабочих наборов
                            for (int w = 0; w < List_workingset.Count(); w++)
                            {
                                FolderItem WorkingsetFolder = new FolderItem(); //Создаем папку рабочих наборов
                                WorkingsetFolder.DisplayName = List_workingset[w].ToString();
                                var CategoryFolder_Chil = FloorsFolder_Chil.Children[0] as FolderItem;
                                //Создаем поисковый набор по рабочим наборам объектов, чтобы икслючить рабочие наборы, в которых нет объектов
                                var search_workingset_folder = new Search();
                                search_workingset_folder.Locations = SearchLocations.DescendantsAndSelf;
                                search_workingset_folder.Selection.SelectAll();

                                SearchCondition search_workingset = SearchCondition.HasPropertyByDisplayName(category, "Рабочий набор");//Условие поиска имеет параметр Рабочий набор 
                                search_workingset_folder.SearchConditions.Add(search_section.EqualValue(VariantData.FromDisplayString(List_sections[i].ToString())));//Добавляем в поиск значение секции 
                                search_workingset_folder.SearchConditions.Add(search_floors.EqualValue(VariantData.FromDisplayString(List_floors[j].ToString())));//Добавляем в поиск значение этажа 
                                search_workingset_folder.SearchConditions.Add(search_workingset.EqualValue(VariantData.FromDisplayString(List_workingset[w].ToString())));//Добавляем в поиск значение рабочего набора
                                //Если поисковый набор пуст, то пропускаем его и не создаем папку
                                if (search_workingset_folder.FindAll(App.ActiveDocument, true).Count == 0)
                                { continue; }
                                //Если у нас конкретный рабочий набор существует на этаже и в секции, то создаем папку
                                if (Check_search_workingset(category_get, category, property_section, 
                                    property_floor, property_category, property_type, List_sections, 
                                    List_category, i, List_floors, j, k, List_workingset, w))
                                {
                                    selectionSets.InsertCopy((GroupItem)CategoryFolder_Chil.Children[0], 0, WorkingsetFolder);//Создаем папку рабочих наборов
                                }

                                //Собираем лист материалов деталей из проекта, фильтруем уникальные значения
                                List<string> List_datail = getDataForProp("Элемент", property_material).Distinct().ToList();
                                //Собираем лист толщин деталей из проекта, фильтруем уникальные значения
                                List<DataProperty> List_thickness_find = getDataForProp_Value(category, "Толщина").Distinct().ToList();
                                //Собираем лист имен типов из проекта, фильтруем уникальные значения
                                List<string> List_type = getDataForProp(category, property_type).Distinct().ToList();

                                //Если внутри поискового набора есть Детали, то идем по пути добавления поисковых наборов с наименованием материала
                                if (Check_detail(category_get, category, property_section, property_floor, property_category, property_type, List_sections, List_category, i, List_floors, j, k))
                                {
                                    //Собирает поисковые наборы стен, в которых есть Детали. Проходим по листу деталей
                                    for (int d = 0; d < List_datail.Count(); d++)
                                    {
                                        //Сортируем детали по толщине (параметр). Проходим по листу толщин
                                        for (int thicknes = 0; thicknes < List_thickness_find.Count(); thicknes++)
                                        {
                                            curSelect.SelectAll();
                                            //Создаем поисковый набор по этажам, секциям, категориям, типам, рабочим наборам, материалу и толщинам
                                            var search = new Search();
                                            search.Locations = SearchLocations.DescendantsAndSelf;
                                            search.Selection.SelectAll();

                                            SearchCondition search_category = SearchCondition.HasPropertyByDisplayName(category, property_category);//Условие поиска имеет параметр Категория
                                            SearchCondition search_type = SearchCondition.HasPropertyByDisplayName(category, property_type);//Условие поиска имеет параметр Тип
                                            SearchCondition search_material = SearchCondition.HasPropertyByDisplayName("Элемент", property_material);//Условие поиска имеет в элементе параметр Материал
                                            SearchCondition search_thickness = SearchCondition.HasPropertyByDisplayName(category, "Толщина");//Условие поиска имеет параметр Толщина

                                            search.SearchConditions.Add(search_section.EqualValue(VariantData.FromDisplayString(List_sections[i].ToString())));//Добавляем в поиск значение секции 
                                            search.SearchConditions.Add(search_floors.EqualValue(VariantData.FromDisplayString(List_floors[j].ToString())));//Добавляем в поиск значение этажа 
                                            search.SearchConditions.Add(search_workingset.EqualValue(VariantData.FromDisplayString(List_workingset[w].ToString())));//Добавляем в поиск значение рабочего набора 
                                            search.SearchConditions.Add(search_category.EqualValue(VariantData.FromDisplayString(List_category[k].ToString())));//Добавляем в поиск значение категории 
                                            search.SearchConditions.Add(search_material.EqualValue(VariantData.FromDisplayString(List_datail[d].ToString())));//Добавляем в поиск значение материала детали     
                                            search.SearchConditions.Add(search_thickness.EqualValue(List_thickness_find[thicknes].Value));//Создаем отдельный поисковый запрос на толщину каждой детали
                                            //Если поисковый набор больше 0 и материал не содержит "Возд" (для того, чтобы избежать детали с воздушной прослойкой в наименовании) создаем поисковый набор
                                            if (search.FindAll(App.ActiveDocument, true).Count > 0 && !List_datail[d].Contains("Возд"))
                                            {
                                                //Привидение строки в вид, как это должно выглядеть в Navis -> футы переводим в метры, разделяем "," после имени материала
                                                string value_datail_with_thickness = List_datail[d] + ", " + (List_thickness_find[thicknes].Value.ToDoubleLength() / 3.28).ToString().Substring(0, 5);

                                                var set = new SelectionSet(search) { DisplayName = value_datail_with_thickness };
                                                selectionSets.AddCopy(set);

                                                var newSet = selectionSets.Value[selectionSets.Value.IndexOfDisplayName(value_datail_with_thickness)] as SavedItem;

                                                var setFolder = pFold.Children[0] as GroupItem;
                                                var setsFolder = setFolder.Children[0] as GroupItem;
                                                var setTypeFolder = setsFolder.Children[0] as GroupItem;
                                                var setWorkingsetFolder = setTypeFolder.Children[0] as GroupItem;

                                                selectionSets.Move(newSet.Parent, selectionSets.Value.IndexOfDisplayName(value_datail_with_thickness), setWorkingsetFolder, 0);
                                            }
                                        }                             
                                }

                                    //Собирает поисковые наборы стен, в которых нет Деталей 
                                    for (int t = 0; t < List_type.Count(); t++)
                                    {
                                        curSelect.SelectAll();
                                        //Создаем поисковый набор по этажам, секциям, категориям, типам, рабочим наборам, материалу и с условием без деталей
                                        var search = new Search();
                                        search.Locations = SearchLocations.DescendantsAndSelf;
                                        search.Selection.SelectAll();

                                        SearchCondition search_category = SearchCondition.HasPropertyByDisplayName(category, property_category);//Условие поиска имеет параметр Категория
                                        SearchCondition search_type = SearchCondition.HasPropertyByDisplayName(category, property_type);//Условие поиска имеет параметр Тип
                                        SearchCondition search_material = SearchCondition.HasPropertyByDisplayName("Элемент", property_material);//Условие поиска имеет в элементе параметр Материал
                                        SearchCondition search_object = SearchCondition.HasPropertyByDisplayName("Объект", "Имя").Negate();

                                        search.SearchConditions.Add(search_section.EqualValue(VariantData.FromDisplayString(List_sections[i].ToString())));
                                        search.SearchConditions.Add(search_floors.EqualValue(VariantData.FromDisplayString(List_floors[j].ToString())));
                                        search.SearchConditions.Add(search_workingset.EqualValue(VariantData.FromDisplayString(List_workingset[w].ToString())));
                                        search.SearchConditions.Add(search_category.EqualValue(VariantData.FromDisplayString(List_category[k].ToString())));
                                        search.SearchConditions.Add(search_type.EqualValue(VariantData.FromDisplayString(List_type[t].ToString())));
                                        search.SearchConditions.Add(search_object.EqualValue(VariantData.FromDisplayString("Деталь")));

                                        if (search.FindAll(App.ActiveDocument, true).Count > 0)
                                        {
                                            var set = new SelectionSet(search) { DisplayName = List_type[t] };
                                            selectionSets.AddCopy(set);

                                            var newSet = selectionSets.Value[selectionSets.Value.IndexOfDisplayName(List_type[t])] as SavedItem;

                                            var setFolder = pFold.Children[0] as GroupItem;
                                            var setsFolder = setFolder.Children[0] as GroupItem;
                                            var setTypeFolder = setsFolder.Children[0] as GroupItem;
                                            var setWorkingsetFolder = setTypeFolder.Children[0] as GroupItem;

                                            selectionSets.Move(newSet.Parent, selectionSets.Value.IndexOfDisplayName(List_type[t]), setWorkingsetFolder, 0);
                                        }
                                    }
                                }
                                //Если внутри поискового набора нет Детали, то идем по пути добавления поисковых наборов с именем типа объектов
                                else
                                {
                                    for (int t = 0; t < List_type.Count(); t++)
                                    {

                                        curSelect.SelectAll();
                                        var search = new Search();
                                        search.Locations = SearchLocations.DescendantsAndSelf;
                                        search.Selection.SelectAll();

                                        //SearchCondition search_floors_new = SearchCondition.HasPropertyByDisplayName(category, property);
                                        SearchCondition search_category = SearchCondition.HasPropertyByDisplayName(category, property_category);
                                        SearchCondition search_type = SearchCondition.HasPropertyByDisplayName(category, property_type);
                                        SearchCondition search_material = SearchCondition.HasPropertyByDisplayName("Элемент", property_material);
                                        SearchCondition search_object = SearchCondition.HasPropertyByDisplayName("Объект", "Имя").Negate();


                                        search.SearchConditions.Add(search_section.EqualValue(VariantData.FromDisplayString(List_sections[i].ToString())));
                                        search.SearchConditions.Add(search_floors.EqualValue(VariantData.FromDisplayString(List_floors[j].ToString())));
                                        search.SearchConditions.Add(search_workingset.EqualValue(VariantData.FromDisplayString(List_workingset[w].ToString())));
                                        search.SearchConditions.Add(search_category.EqualValue(VariantData.FromDisplayString(List_category[k].ToString())));
                                        search.SearchConditions.Add(search_type.EqualValue(VariantData.FromDisplayString(List_type[t].ToString())));
                                        search.SearchConditions.Add(search_object.EqualValue(VariantData.FromDisplayString("Деталь")));


                                        //прописать поисковый запрос, как неравно - делали


                                        if (search.FindAll(App.ActiveDocument, true).Count > 0)
                                        {
                                            var set = new SelectionSet(search) { DisplayName = List_type[t] };
                                            selectionSets.AddCopy(set);

                                            var newSet = selectionSets.Value[selectionSets.Value.IndexOfDisplayName(List_type[t])] as SavedItem;

                                            var setFolder = pFold.Children[0] as GroupItem;
                                            var setsFolder = setFolder.Children[0] as GroupItem;
                                            var setTypeFolder = setsFolder.Children[0] as GroupItem;
                                            var setWorkingsetFolder = setTypeFolder.Children[0] as GroupItem;

                                            selectionSets.Move(newSet.Parent, selectionSets.Value.IndexOfDisplayName(List_type[t]), setWorkingsetFolder, 0);
                                        }

                                    }
                                }

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Не удалось создать поисковые наборы");
                MessageBox.Show(ex.ToString());
            }

            var tp7 = new Main();

            try
            {
                string[] a = { folder };
                tp7.Execute(a);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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
        public static List<DataProperty> getDataForProp_Value(string cat_param, string prop_param)
        {
            ModelItemEnumerableCollection items = App.ActiveDocument.Models.RootItemDescendants;

            List<DataProperty> list = new List<DataProperty>();

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
                               list.Add(prop);
                            }
                        }
                    }
                }
            }
            return list;
        }

        public bool Check_search_workingset(string category_get, string category,
           string property_section, string property, string property_category,
           string property_type, List<string> values_sections, List<string> values_category,
           int i, List<string> values_floors, int j, int k, List<string> List_workingset, int w)
        {
            var search = new Search();
            search.Locations = SearchLocations.DescendantsAndSelf;
            search.Selection.SelectAll();

            //SearchCondition search_floors_new = SearchCondition.HasPropertyByDisplayName(category, property);
            SearchCondition search_section = SearchCondition.HasPropertyByDisplayName(category_get, property_section);
            SearchCondition search_floors = SearchCondition.HasPropertyByDisplayName(category_get, property);
            SearchCondition search_category = SearchCondition.HasPropertyByDisplayName(category, property_category);
            SearchCondition search_workingset = SearchCondition.HasPropertyByDisplayName(category, "Рабочий набор");

            search.SearchConditions.Add(search_section.EqualValue(VariantData.FromDisplayString(values_sections[i].ToString())));
            search.SearchConditions.Add(search_floors.EqualValue(VariantData.FromDisplayString(values_floors[j].ToString())));
            search.SearchConditions.Add(search_category.EqualValue(VariantData.FromDisplayString(values_category[k].ToString())));
            search.SearchConditions.Add(search_workingset.EqualValue(VariantData.FromDisplayString(List_workingset[w].ToString())));

            if (search.FindAll(App.ActiveDocument, true).Count == 0)

            {
                return false;
            }
            return true;
        }
        public bool Check_search_category (string category_get, string category,
            string property_section, string property, string property_category,
            List<string> values_sections, List<string> values_category,
            int i, List<string> values_floors,int j, int k)
        {
            var search = new Search();
            search.Locations = SearchLocations.DescendantsAndSelf;
            search.Selection.SelectAll();

            //SearchCondition search_floors_new = SearchCondition.HasPropertyByDisplayName(category, property);
            SearchCondition search_section = SearchCondition.HasPropertyByDisplayName(category_get, property_section);
            SearchCondition search_floors = SearchCondition.HasPropertyByDisplayName(category_get, property);
            SearchCondition search_category = SearchCondition.HasPropertyByDisplayName(category, property_category);


            search.SearchConditions.Add(search_section.EqualValue(VariantData.FromDisplayString(values_sections[i].ToString())));
            search.SearchConditions.Add(search_floors.EqualValue(VariantData.FromDisplayString(values_floors[j].ToString())));
            search.SearchConditions.Add(search_category.EqualValue(VariantData.FromDisplayString(values_category[k].ToString())));


            if (search.FindAll(App.ActiveDocument, true).Count == 0) 

            { 
            return false;
            }
            return true;
        }
        public bool Check_detail (string category_get, string category,
            string property_section, string property, string property_category,
            string property_type, List<string> values_sections, List<string> values_category,
            int i, List<string> values_floors, int j, int k)
            {
            var search = new Search();
            search.Locations = SearchLocations.DescendantsAndSelf;
            search.Selection.SelectAll();

            SearchCondition search_detail = SearchCondition.HasPropertyByDisplayName(category, "Имя");
            SearchCondition search_section = SearchCondition.HasPropertyByDisplayName(category_get, property_section);
            SearchCondition search_floors = SearchCondition.HasPropertyByDisplayName(category_get, property);
            SearchCondition search_category = SearchCondition.HasPropertyByDisplayName(category, property_category);

            search.SearchConditions.Add(search_section.EqualValue(VariantData.FromDisplayString(values_sections[i].ToString())));
            search.SearchConditions.Add(search_floors.EqualValue(VariantData.FromDisplayString(values_floors[j].ToString())));
            search.SearchConditions.Add(search_category.EqualValue(VariantData.FromDisplayString(values_category[k].ToString())));
            search.SearchConditions.Add(search_detail.EqualValue(VariantData.FromDisplayString("Деталь")));

            if (search.FindAll(App.ActiveDocument, true).Count == 0)
            {
                return false;
            }
            return true; 
        }
            

        private void textBox_nameObject_TextChanged(object sender, EventArgs e)
        {

        }

        private void maskedTextBox1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void textBox_countSection_ValueChanged(object sender, EventArgs e)
        {

        }

        private void CreateQuantification_Load(object sender, EventArgs e)
        {

        }

        private void tab_header_Click(object sender, EventArgs e)
        {

        }
    }
}
