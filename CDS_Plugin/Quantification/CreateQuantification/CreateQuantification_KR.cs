using Autodesk.Navisworks.Api.ComApi;
using Autodesk.Navisworks.Api;
using System;
using App = Autodesk.Navisworks.Api.Application;
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

namespace CDS_Plugin.Quantification.CreateQuantification
{
    public partial class CreateQuantification_KR : Form
    {
        public CreateQuantification_KR()
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
            if (!Count_section_OK()){
                MessageBox.Show("Не так в секции"); return; }
            if (!Name_OK()) {
                MessageBox.Show("Не так в именем"); return; }

                string folder = textBox_nameObject.Text;
                string category_stage = text_cat.Text;
                string category = "Объект";
                string property_stage = text_param.Text;
                string property_section = "ADSK_Номер секции";
                string property_category = "Категория";
                string property_type = "Тип";
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

                                    //ДОРАБОТКА НА СЛУЧИЙ НЕСКОЛЬКИХ СЕКЦИЙ В ОДНОЙ МОДЕЛИ
                                    //for (int i = 0; i < sections; i++)
                                    //{ }
                                    ////Создаем список параметров секции из проекта, фильтруем уникальные значения
                                    //List<string> values_sections = getDataForProp(category, property_section).Distinct().ToList();

                    //Создаем список параметров стадий из проекта, фильтруем уникальные значения
                    List<string> values_stages = getDataForProp(category_stage, property_stage).Distinct().ToList();
                    //Создаем родительскую папку с названием проекта
                    FolderItem pFold = selectionSets.Value.FirstOrDefault(x => x.DisplayName == folder) as FolderItem;
                    //Создаем папку с именем секции первого вложения в родительскую папку
                    FolderItem sFolder = new FolderItem();//Папка секций
                    sFolder.DisplayName = sectionstr + sections.ToString();
                    selectionSets.InsertCopy((GroupItem)pFold, 0, sFolder);

                    //Проходим по всему списку стадий
                    for (int j = 0; j < values_stages.Count(); j++)
                    {
                        //Создаем список категорий элементов из проекта, , фильтруем уникальные значения
                        List<string> values_category = getDataForProp(category, property_category).Distinct().ToList();

                        FolderItem StagesFolder = new FolderItem(); //Создаем папку по стад
                        StagesFolder.DisplayName = values_stages[j].ToString();
                        //Создаем поисковый набор по стадиям объектов, чтобы икслючить стадии, в которых нет объектов
                        var search_stage = new Search();
                        search_stage.Locations = SearchLocations.DescendantsAndSelf;
                        search_stage.Selection.SelectAll();
                        //Условие по содержанию всех стадий в проекте
                        SearchCondition search_stages = SearchCondition.HasPropertyByDisplayName(category_stage, property_stage);
                        //Условие по равенству стадии к значению из списка
                        search_stage.SearchConditions.Add(search_stages.EqualValue(VariantData.FromDisplayString(values_stages[j].ToString())));
                        //Если поисковый набор пуст, то пропускаем его и не создаем папку
                        if (search_stage.FindAll(App.ActiveDocument, true).Count == 0)
                        {
                            continue;
                        }
                        //Создание папки с именем стадии второго вложения в родительскую (первое вложение в папку секции)
                        selectionSets.InsertCopy((GroupItem)pFold.Children[0], 0, StagesFolder);
                        //Проходим по всему списку категорий
                        for (int k = 0; k < values_category.Count(); k++)
                        {
                            FolderItem CategoryFolder = new FolderItem(); //Папка категорий
                            CategoryFolder.DisplayName = values_category[k].ToString();
                            var FloorsFolder_Chil = pFold.Children[0] as FolderItem;

                            if (Check_search_category_KR( category_stage,  category,
                                property_section,  property_category,  property_stage
                                ,  values_category,  values_stages,  j,  k))
                            {
                                selectionSets.InsertCopy((GroupItem)FloorsFolder_Chil.Children[0], 0, CategoryFolder);
                            }

                            List<string> values_type = getDataForProp(category, property_type);
                            values_type.Distinct().ToList();

                                for (int t = 0; t < values_type.Count(); t++)
                                {
                                    var search = new Search();
                                    search.Locations = SearchLocations.DescendantsAndSelf;
                                    search.Selection.SelectAll();

                                    SearchCondition search_category = SearchCondition.HasPropertyByDisplayName(category, property_category);
                                    SearchCondition search_type = SearchCondition.HasPropertyByDisplayName(category, property_type);

                                    search.SearchConditions.Add(search_stages.EqualValue(VariantData.FromDisplayString(values_stages[j].ToString())));
                                    search.SearchConditions.Add(search_category.EqualValue(VariantData.FromDisplayString(values_category[k].ToString())));
                                    search.SearchConditions.Add(search_type.EqualValue(VariantData.FromDisplayString(values_type[t].ToString())));
    
                                    //прописать поисковый запрос, как неравно - делали

                                    if (search.FindAll(App.ActiveDocument, true).Count > 0)
                                    {
                                        var set = new SelectionSet(search) { DisplayName = values_type[t] };
                                        selectionSets.AddCopy(set);

                                        var newSet = selectionSets.Value[selectionSets.Value.IndexOfDisplayName(values_type[t])] as SavedItem;

                                        var setFolder = pFold.Children[0] as GroupItem;
                                        var setsFolder = setFolder.Children[0] as GroupItem;
                                        var setStagesFolder = setsFolder.Children[0] as GroupItem;
                                        //var TypeFolder = setStagesFolder.Children[0] as GroupItem; 

                                    selectionSets.Move(newSet.Parent, selectionSets.Value.IndexOfDisplayName(values_type[t]), setStagesFolder, 0);
                                    }
                                }    
                        }
                    }
                
                }
                catch (Exception)
                {

                MessageBox.Show("Не удалось создать поисковые наборы");

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

        public bool Check_search_category_KR (string category_stage, string category,
            string property_section, string property_category, string property_stage
          , List<string> values_category, List<string> values_stages,int j, int k)
        {
            var search = new Search();
            search.Locations = SearchLocations.DescendantsAndSelf;
            search.Selection.SelectAll();

            //SearchCondition search_floors_new = SearchCondition.HasPropertyByDisplayName(category, property);
            SearchCondition search_stages = SearchCondition.HasPropertyByDisplayName(category_stage, property_stage);
            SearchCondition search_category = SearchCondition.HasPropertyByDisplayName(category, property_category);

            search.SearchConditions.Add(search_stages.EqualValue(VariantData.FromDisplayString(values_stages[j].ToString())));
            search.SearchConditions.Add(search_category.EqualValue(VariantData.FromDisplayString(values_category[k].ToString())));

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
