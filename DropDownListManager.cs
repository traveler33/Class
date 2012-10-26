using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Data;
using Pelesys.Data;
using Pelesys.Scheduling;

namespace eForm.Class
{
    public class DropDownListManager : System.Web.UI.Page
    {

        public void SetDropDownListSelectedValueByText(DropDownList ddl, string SelectText)
        {



            for (int n = 0; n <= ddl.Items.Count - 1; n++)
            {
                if (ddl.Items[n].Text == SelectText)
                {
                    ddl.SelectedIndex = n;
                    break;
                }
            }

        }


        public void SetPagingDropDownList(DropDownList ddl, System.Web.UI.WebControls.GridView oGrid)
        {
            for (int i = 0; i < oGrid.PageCount; i++)
            {
                ddl.Items.Add(new ListItem(Convert.ToString(i + 1), i.ToString()));
            }
            ddl.AutoPostBack = true;
            // assign an Event Handler when its Selected Index Changed

            // synchronize its selected index to GridView's current PageIndex
            if (ddl.Items.Count > 0)
            {
                ddl.SelectedIndex = oGrid.PageIndex;
            }



        }

        //public void SetCheckBoxListForMemberList(List<Design> oLists, CheckBoxList oBoxList)
        //{

        //    foreach (Design oList in oLists)
        //    {
        //        ListItem oItem = new ListItem();
        //        oItem.Text = oList.ColumnName.ToString();
        //        oBoxList.Items.Add(oItem);

        //    }

        //}


        public void SetDatasetToDropDownList(DataSet oDS, DropDownList oDDL, String TextField, String IDField)
        {
            oDDL.DataSource = oDS;
            oDDL.DataTextField = TextField;
            oDDL.DataValueField = IDField;
            oDDL.DataBind();

        }

        public void SetDatasetToDropDownList(DataSet oDS, DropDownList oDDL, String TextField, String IDField, Boolean IsAddNewBlankItem)
        {
            oDDL.DataSource = oDS;
            oDDL.DataTextField = TextField;
            oDDL.DataValueField = IDField;
            oDDL.DataBind();

            oDDL.Items.Insert(0, new ListItem(string.Empty, string.Empty));
        }

      
        //public void SetValueListToDropDownList(DropDownList DDLList, string ValueListColumn)
        //{
        //    try
        //    {


        //        DDLList.DataSource = eForm.GetValueListByCategory(ValueListColumn);
        //        DDLList.DataTextField = "DisplayValue";
        //        DDLList.DataValueField = "DisplayValue";
        //        DDLList.DataBind();

        //        DDLList.Items.Insert(0, new ListItem(string.Empty, string.Empty));

        //    }
        //    catch (Exception ex)
        //    {

        //        throw new ArgumentException(ex.Message);
        //    }


        //}

        //public void SetDictionaryToDropDownList(DropDownList DDlList, String Category,
        //        String TextField, String IDField)
        //{
        //    try
        //    {
        //        DataMappingDataContext db = new DataMappingDataContext(AppModuleManager.ConnectionString);


        //        var dictionary =
        //            from DicCollection in db.Dictionaries
        //            where DicCollection.Category == Category
        //            orderby DicCollection.DisplayValue

        //            select DicCollection;
        //        DDlList.DataSource = dictionary;
        //        DDlList.DataTextField = TextField;
        //        DDlList.DataValueField = IDField;
        //        DDlList.DataBind();

        //        if (Category == "Paid" || Category == "Status")
        //        {
        //            ListItem oItem = new ListItem();
        //            oItem.Text = TranslationManager.GetTranslationByKey(TranslationManager.TranslationMemberSearchDropDownListKey);
        //            oItem.Value = string.Empty;
        //            DDlList.Items.Insert(0, oItem);
        //            DDlList.Items[0].Selected = true;

        //        }

        //    }
        //    catch (Exception ex)
        //    {

        //        throw new ArgumentException(ex.Message);
        //    }



        //}
    }

}