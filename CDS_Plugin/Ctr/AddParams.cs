using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.DocumentParts;
using Autodesk.Navisworks.Api.Interop.ComApi;
using System;
using System.Collections.Generic;
using System.Diagnostics.PerformanceData;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using static System.Windows.Forms.AxHost;
using ComApi = Autodesk.Navisworks.Api.Interop.ComApi;
using ComApiBridge = Autodesk.Navisworks.Api.ComApi.ComApiBridge;

namespace CDS_Plugin.AddParams
{
    internal class AddCategory 
    {
        public int Execute(params string[] parameters)
        {
            //Получаем доступ к файлу (.NET)
            Document doc = Autodesk.Navisworks.Api.Application.ActiveDocument;
            //Получаем доступ к файлу (COM)
            InwOpState10 cdoc = ComApiBridge.State;
            //Выбираем элементы и записываем в коллекцию
            ModelItemCollection items = doc.CurrentSelection.SelectedItems;
            //Если выбранные элементы > 0, то начинаем процесс записывания категорий и параметров 
            if (items.Count > 0)
            {
                //Перебираем все элементы в коллекции
                foreach (ModelItem item in items)
                {
                    //преобразовать элемент модели в COM-путь
                    InwOaPath citem = (InwOaPath)ComApiBridge.ToInwOaPath(item);
                    // Get item's PropertyCategoryCollection
                    InwGUIPropertyNode2 cpropcates = (InwGUIPropertyNode2)cdoc.GetGUIPropertyNode(citem, true);
                    // create a new Category (PropertyDataCollection)
                  
                    ComApi.InwOaPropertyVec newPvec = cdoc.ObjectFactory(nwEObjectType.eObjectType_nwOaPropertyVec, null, null);
                    ComApi.InwOaProperty FirstP =(ComApi.InwOaProperty)cdoc.ObjectFactory(ComApi.nwEObjectType.eObjectType_nwOaProperty, null, null);

                   //Добавляем параметры и задаеи им значение
                    FirstP.name = "Prop_CDS";
                    FirstP.UserName = "Параметр ЦДС";
                    FirstP.value = "true";

                    newPvec.Properties().Add(FirstP);

                    //Добавляем параметры и задаем имя категории параметров
                    cpropcates.SetUserDefined(0, "CDS_Plugin", "CDS_Plugin_InteralName", newPvec);

                }
            }

            else
            {
                MessageBox.Show("Нет элементов");

            }

            return 0;
        }

        }



}
