using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data;
using System.Configuration;

using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections;
using AjaxControlToolkit;


namespace eForm.Class
{
    public  class AjexTabs
    {
        public const string TranslationFormDesignAddNewTabMSGValue = "This tab name already exists. Please change this tab name";
        const string DefaultStyle = "display: block; visibility: visible;";
        const string DefaultTabCssClass = "gray";

        public static string TabBodyCssClass = string.Empty;
        public string Error = string.Empty;
       
        public static string AddNewTabToTabContainer(TabContainer oTC, String TabName, String TabIdentity, string TabDesc, string hidClientID,  bool IsEnabled)
        {
            Boolean IsTabExist = false;
            for (int n = 0; n <= oTC.Tabs.Count - 1; n++)
            {
                if (TabName.ToUpper() == oTC.Tabs[n].HeaderText.ToUpper())
                {
                    IsTabExist = true;
                }

            }

            if (!IsTabExist)
            {
                
                TabPanel oNewTab = CreateTab(TabName, TabDesc);
                oNewTab.Visible = true;
                oNewTab.Enabled = IsEnabled;
                oNewTab.ToolTip = TabDesc;
                oNewTab.ID = TabIdentity;
                oNewTab.HeaderTemplate = new TabHeaderTemplate(TabName, "", hidClientID, oNewTab.ID);
                oNewTab.TabIndex = Convert.ToInt16(oTC.Tabs.Count + 1);
                
                oTC.Tabs.Add(oNewTab);

                oTC.ActiveTabIndex = Convert.ToInt16(oTC.Tabs.Count - 1);
            }
            else
            {

                return TranslationFormDesignAddNewTabMSGValue;

            }
            return string.Empty;

        }

        public static void SetActiveIndex(TabContainer oTC, String TabName, String TabIdentity)
        {
            for (int n = 0; n <= oTC.Tabs.Count - 1; n++)
            {
                if (oTC.Tabs[n].ID.ToUpper() == TabIdentity.ToUpper())
                {
                    oTC.ActiveTabIndex = n;
                }
            }

        }

        private static TabPanel CreateTab(string name, string desc)
        {
            TabPanel tp = new TabPanel();

           // tp.HeaderTemplate = new MyHeaderTemplate(name, desc); ;


            //tp.Controls.Add(new LiteralControl(string.Formatndiv class=\"tabBody\">{0}</div>", name)));
            return tp;
        }

      
   
     
      

        public static void RemoveTab(TabContainer oTC, String TabName, String TabIdentity)
        {
            for (int n = 0; n <= oTC.Tabs.Count - 1; n++)
            {
                TabPanel oPanel = oTC.Tabs[n];
                if (oPanel.ID == TabIdentity)
                {
                    oTC.Tabs.RemoveAt(n);
                    break;
                }
            }
        }

       
     
    }
    public class MyHeaderTemplate : ITemplate
    {
        private string skinID = "skinTabName";
        private string _HeadText = string.Empty;
        private string _Desc = string.Empty;

        public MyHeaderTemplate()
        {

        }

        public MyHeaderTemplate(String HeadText, String Desc)
        {
            _HeadText = HeadText;
            _Desc = Desc;
        }

        public void InstantiateIn(System.Web.UI.Control container)
        {      //TODO: Add you customisation here...      
            Label lbl = new Label();
            lbl.ID = _HeadText;
            lbl.Text = _HeadText;
            lbl.ToolTip = _Desc;
            lbl.SkinID = skinID;
            // Lastly, add the controls to the container...      
            container.Controls.Add(lbl);
        }
    }
}