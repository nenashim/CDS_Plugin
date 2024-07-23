using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.Plugins;
using CDS_Plugin.Quantification.CreateQuantification;
using System;
using App = Autodesk.Navisworks.Api.Application;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Navisworks.Api.DocumentParts;
using Newtonsoft.Json.Linq;
using System.Windows;

namespace CDS_Plugin.Classifier
{
    internal class FindingClassifier : AddInPlugin
    {
        public override int Execute(params string[] parameters)
        {
            string category_get = "Объект";
            string category_classifier = "Тип в приложении Revit";
            string property_classifier = "Ключевая пометка";
            string category = "Объект";
            string property_floor = "ADSK_Этаж";
            string property_section = "ADSK_Номер секции";
            string property_category = "Категория";
            string property_type = "Тип";
            string floorstr = " этаж";
            string sectionstr = "С ";
 
            Document activeDoc = App.ActiveDocument;
            var curSelect = activeDoc.CurrentSelection;
            

            try
            {
                DocumentModels models = activeDoc.Models;

                foreach (var m in models)
                {
                    var selectionSets = activeDoc.SelectionSets;
                    FolderItem pFold = new FolderItem();
                    pFold.DisplayName = m.RootItem.DisplayName.Split('.').First();
                    //FolderItem pFold = selectionSets.Value.FirstOrDefault(x => x.DisplayName == m.RootItem.DisplayName.Split('.').First()) as FolderItem;
                    // FolderItem pFold = selectionSets.Value.FirstOrDefault(x => x.DisplayName == m.FileName.Split('\\').Last().Split('.').First()) as FolderItem;
                    List<string> header_classifiers = getDataForProp(category_classifier, property_classifier);
                    header_classifiers.Distinct().ToList();

                    for (int c = 0; c < header_classifiers.Count; c++)
                    {
                        FolderItem sFolder = new FolderItem();    //Папка классификатора
                        sFolder.DisplayName = header_classifiers[c];
                        selectionSets.InsertCopy((GroupItem)pFold, 0, sFolder);

                        List<string> values_sections = getDataForProp(category_get, property_section);
                        values_sections.Distinct().ToList();

                        for (int i = 0; i < values_sections.Count; i++)
                        {
                            FolderItem SectionsFolder = new FolderItem(); //Папка секций
                            SectionsFolder.DisplayName = sectionstr + values_sections[i].ToString();

                            selectionSets.InsertCopy((GroupItem)pFold.Children[0], 0, SectionsFolder);

                            List<string> values_floors = getDataForProp(category_get, property_section);
                            values_floors.Distinct().ToList();

                            for (int l = 0; l < values_floors.Count; l++)
                            {
                                FolderItem FloorFolder = new FolderItem(); //Папка этажей
                                FloorFolder.DisplayName = values_floors[l].ToString() + floorstr;

                                var floor_sections = new Search();
                                floor_sections.Locations = SearchLocations.DescendantsAndSelf;
                                floor_sections.Selection.CopyFrom((IEnumerable<ModelItem>)m);//Выбрать файл
                                SearchCondition search_section = SearchCondition.HasPropertyByDisplayName(category_get, property_section);
                                SearchCondition search_floors = SearchCondition.HasPropertyByDisplayName(category_get, property_floor);
                                floor_sections.SearchConditions.Add(search_section.EqualValue(VariantData.FromDisplayString(values_sections[i].ToString())));
                                floor_sections.SearchConditions.Add(search_floors.EqualValue(VariantData.FromDisplayString(values_floors[l].ToString())));

                                if (floor_sections.FindAll(App.ActiveDocument, true).Count == 0)
                                {
                                    continue;
                                }
                                var setsFolder = SectionsFolder.Children[0] as GroupItem;

                                selectionSets.InsertCopy((GroupItem)setsFolder, 0, FloorFolder);

                                List<string> classifiers = getDataForProp(category_classifier, property_classifier);
                                classifiers.Distinct().ToList();


                                for (int t = 0; t < classifiers.Count(); t++)
                                {

                                    curSelect.SelectAll();

                                    var search = new Search();
                                    search.Locations = SearchLocations.DescendantsAndSelf;
                                    search.Selection.SelectAll();

                                    SearchCondition search_category = SearchCondition.HasPropertyByDisplayName(category, property_category);
                                    SearchCondition search_type = SearchCondition.HasPropertyByDisplayName(category, property_type);
                                    search.SearchConditions.Add(search_section.EqualValue(VariantData.FromDisplayString(values_sections[i].ToString())));
                                    search.SearchConditions.Add(search_floors.EqualValue(VariantData.FromDisplayString(values_floors[l].ToString())));
                                    search.SearchConditions.Add(search_category.EqualValue(VariantData.FromDisplayString(header_classifiers[c].ToString())));


                                    if (search.FindAll(App.ActiveDocument, true).Count > 0)
                                    {
                                        var set = new SelectionSet(search) { DisplayName = classifiers[t] };
                                        selectionSets.AddCopy(set);

                                        var newSet = selectionSets.Value[selectionSets.Value.IndexOfDisplayName(classifiers[t])] as SavedItem;

                                        var setFolder = pFold.Children[0] as GroupItem;
                                        var sectionsFolder = setFolder.Children[0] as GroupItem;
                                        var setClassifiersFolder = setsFolder.Children[0] as GroupItem;

                                        selectionSets.Move(newSet.Parent, selectionSets.Value.IndexOfDisplayName(classifiers[t]), setClassifiersFolder, 0);
                                    }
                                }
                            }
                        }
                    }


                    //int setsValues = selectionSets.Value.IndexOfDisplayName(folder);

                    ////If folder doesn't exist add folder
                    //if (setsValues != -1)
                    //{
                    //    selectionSets.RemoveAt(selectionSets.RootItem, setsValues);
                    //}
                    //selectionSets.AddCopy(new FolderItem() { DisplayName = folder });

                }
            }
            catch (Exception)
            {

                MessageBox.Show("Не удалось создать поисковые наборы");

            }
            return 0;
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
    }
}
