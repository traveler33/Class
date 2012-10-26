using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Text;
using System.Collections.Generic;
using System.Collections;

using AjaxControlToolkit;
/// <summary>
/// Summary description for ClientJSManager
/// </summary>
public static class ClientJSManager
{
    static public String ConfirmedClientID = string.Empty;

    #region   Define const and Hash table
    public static Hashtable FieldCol = new Hashtable();
    public static Hashtable MsgCol = new Hashtable();



    public const string JavaScriptBegin = "<script type='text/javascript'>";
    public const string JavaScriptEnd = "</script>";
    const string JavaScriptFunctionName = "function";
    public const string javaScriptFormFieldName = "<FieldName>";

    #endregion

    


    #region   Set focus
    public static void SetFocusLoad(Page oPage, String RegName, String TxtClientID)
    {
        StringBuilder sbScript = new StringBuilder(50);
        sbScript.Append(JavaScriptBegin);
        // sbScript.Append("var " + TxtClientID + "= GetDivObject('" + TxtClientID + "');");
        //sbScript.Append(TxtClientID + ".focus();");
        sbScript.Append(" $(document).ready(function(){");
        sbScript.Append(" $('#" + TxtClientID + "').focus();");
        sbScript.Append("});");

        sbScript.Append(JavaScriptEnd);
        oPage.ClientScript.RegisterStartupScript(typeof(String), RegName, sbScript.ToString());




    }

    #endregion

    #region   Pop a client script
    public static void PopClientScript(Page oPage, String RegName, String FocusClientID)
    {
        StringBuilder sbScript = new StringBuilder(50);
        sbScript.Append(JavaScriptBegin);
        // sbScript.Append("var " + TxtClientID + "= GetDivObject('" + TxtClientID + "');");
        //sbScript.Append(TxtClientID + ".focus();");
        sbScript.Append("alert('test'); ");

        sbScript.Append(JavaScriptEnd);
        oPage.ClientScript.RegisterClientScriptBlock(typeof(String), RegName, sbScript.ToString());



    }

    #endregion


    #region   A Java script function to show message on page load
    public static void ClientMessage(Page oPage, String MsgName, String message)
    {

        if (!string.IsNullOrEmpty(message))
        {
            if (message.EndsWith("."))
                message = message.Substring(0, message.Length - 1);
        }
        StringBuilder sbScript = new StringBuilder(50);
        //Java Script header           
        sbScript.Append(JavaScriptBegin);

        sbScript.Append("alert( '" + message + "' );");
        sbScript.Append(JavaScriptEnd);
        // Gets the executing web page         
        Page currentPage = HttpContext.Current.CurrentHandler as Page;
        if (currentPage != null && !currentPage.ClientScript.IsStartupScriptRegistered(MsgName))
        {
            currentPage.ClientScript.RegisterStartupScript(typeof(string), MsgName, sbScript.ToString());
        }
    }
    #endregion

    #region Register java script function

    public static void RegisterClientFunction(String FunctionName, String FunctionCode)
    {
        if (!string.IsNullOrEmpty(FunctionCode))
        {
            if (FunctionCode.EndsWith("."))
                FunctionCode = FunctionCode.Substring(0, FunctionCode.Length - 1);
        }
        StringBuilder sbScript = new StringBuilder(50);
        //Java Script header           
        sbScript.Append("<script type='text/javascript'>" + Environment.NewLine);
        sbScript.Append("// build in java script function" + Environment.NewLine);
        FunctionCode = FunctionCode.Replace("\n", "\\n").Replace("\"", "'");
        sbScript.Append(FunctionCode);
        sbScript.Append(@"</script>");
        // Gets the executing web page         
        Page currentPage = HttpContext.Current.CurrentHandler as Page;
        // Checks if the handler is a Page and that the script isn't already on the Page     
        if (currentPage != null && !currentPage.ClientScript.IsStartupScriptRegistered(FunctionName))
        {
            currentPage.ClientScript.RegisterStartupScript(typeof(Page), FunctionName, sbScript.ToString());
        }






    }
    #endregion

    #region Call Javascript function on the page

    public static void CallClientFunctionByName(String FunctionName)
    {
        StringBuilder sbScript = new StringBuilder(50);
        //Java Script header           
        sbScript.Append("<script type='text/javascript'>" + FunctionName + Environment.NewLine);

        //  sbScript.Append(@" """ + FunctionName + @" ()"" ;");
        sbScript.Append(@"</script>");

        // return sbScript.ToString();
        Page currentPage = HttpContext.Current.CurrentHandler as Page;
        string RegName = "Newfunction";
        // Checks if the handler is a Page and that the script isn't already on the Page     
        if (currentPage != null && !currentPage.ClientScript.IsStartupScriptRegistered(RegName))
        {
            currentPage.ClientScript.RegisterStartupScript(typeof(Page), RegName, sbScript.ToString());
        }
    }


    public static void RegScritBlockClientFunctionByName(String FunctionName, String sRegName)
    {
        StringBuilder sbScript = new StringBuilder(50);
        //Java Script header           
        sbScript.Append("<script type='text/javascript'>" + FunctionName + Environment.NewLine);

        //  sbScript.Append(@" """ + FunctionName + @" ()"" ;");
        sbScript.Append(@"</script>");

        // return sbScript.ToString();
        Page currentPage = HttpContext.Current.CurrentHandler as Page;
        string RegName = sRegName;
        // Checks if the handler is a Page and that the script isn't already on the Page     
        if (currentPage != null && !currentPage.ClientScript.IsStartupScriptRegistered(RegName))
        {
            currentPage.ClientScript.RegisterClientScriptBlock(typeof(Page), RegName, sbScript.ToString());
        }
    }
    #endregion


    #region client side form validation on the page

    //public static void ClientSideFormValidation
    //    (String RegName, String FunctionName, String Msg, Hashtable FieldCol)
    //{
    //    StringBuilder sbScript = new StringBuilder(50);
    //    //Java Script header           
    //    sbScript.Append(JavaScriptBegin);

    //    sbScript.Append(JavaScriptFunctionName + " " + FunctionName + @"(){");
    //    foreach (object key in FieldCol.Keys)
    //    {
    //        ValidField oField = (ValidField)FieldCol[key];
    //        //sbScript.Append("var " + key.ToString() + "= GetDivObject('" + oField.FieldClientID + "');");
    //        sbScript.Append("if ( $('#" + oField.FieldClientID + "').val()=='')");
    //        if (oField.ValidType == AppModuleManager.ClientSideValidType.NoEmpty)
    //        {
    //            sbScript.Append("{alert( '" + Msg.Replace(javaScriptFormFieldName, oField.FieldName) + "'); $('#" + oField.FieldClientID + "').focus();return false;}");
    //        }
    //    }

    //    sbScript.Append(@"return true;");

    //    sbScript.Append(@"}");
    //    sbScript.Append(JavaScriptEnd);

    //    Page currentPage = HttpContext.Current.CurrentHandler as Page;
    //    // Checks if the handler is a Page and that the script isn't already on the Page     
    //    if (currentPage != null && !currentPage.ClientScript.IsStartupScriptRegistered(RegName))
    //    {
    //        currentPage.ClientScript.RegisterStartupScript(typeof(Page), RegName, sbScript.ToString());
    //    }

    //}

    #endregion


    #region client side pop message on the page

    public static void SendClinetMessageByUsingScriptManager(string Messge, UpdatePanel oUpDatePanel)
    {
        Guid gMessage = Guid.NewGuid();
        string sJavascript = string.Empty;
        sJavascript = @"alert( '" + Messge + "') ";
        ScriptManager.RegisterStartupScript(oUpDatePanel, oUpDatePanel.GetType(), gMessage.ToString(), sJavascript, true);
    }

    public static void CallClientFuncitonByUsingScriptManager(string Functions, UpdatePanel oUpDatePanel)
    {
        Guid gMessage = Guid.NewGuid();

        ScriptManager.RegisterStartupScript(oUpDatePanel, oUpDatePanel.GetType(), gMessage.ToString(), Functions, true);
    }

    public static void ClientSidePopMessage
         (String RegName, String HidClientID, Hashtable MsgCol,
        String SetFocusFieldClientID)
    {

        StringBuilder sbScript = new StringBuilder(50);
        sbScript.Append(JavaScriptBegin);

        sbScript.Append(JavaScriptFunctionName + " EndRequestHandler(sender, args) { ");
        // sbScript.Append("var " + SetFocusFieldClientID + "= GetDivObject('" + SetFocusFieldClientID + "');");
        if (HidClientID != string.Empty)
        {
            sbScript.Append("var " + HidClientID + "= GetDivObject('" + HidClientID + "');");

        }

        foreach (object key in MsgCol.Keys)
        {
            string sValue = MsgCol[key].ToString();
            if (HidClientID == string.Empty)
            {
                sbScript.Append("alert('");
                sbScript.Append(sValue);
                sbScript.Append("');  $('#" + SetFocusFieldClientID + "').focus();");

            }
            else
            {


                sbScript.Append("if ( " + HidClientID + ".value=='" + key + "')");

                sbScript.Append("{ alert('");
                sbScript.Append(sValue);
                sbScript.Append("'); $('#" + SetFocusFieldClientID + "').focus();}");


            }
        }
        sbScript.Append("}");
        sbScript.Append("window.onload=load;");





        sbScript.Append(JavaScriptEnd);

        Page currentPage = HttpContext.Current.CurrentHandler as Page;
        if (currentPage != null && !currentPage.ClientScript.IsStartupScriptRegistered(RegName))
        {
            currentPage.ClientScript.RegisterStartupScript(typeof(Page), RegName, sbScript.ToString());
        }

    }
    #endregion





}

//public class ValidField
//{

//    private string _FieldName;
//    private string _FieldClientID;
//    private AppModuleManager.ClientSideValidType _ValidType;

//    public String FieldName
//    {
//        get
//        {
//            return _FieldName;
//        }
//        set
//        {
//            _FieldName = value;
//        }

//    }

//    public String FieldClientID
//    {
//        get
//        {
//            return _FieldClientID;
//        }
//        set
//        {
//            _FieldClientID = value;
//        }

//    }


//    public AppModuleManager.ClientSideValidType ValidType
//    {
//        get
//        {
//            return _ValidType;
//        }
//        set
//        {
//            _ValidType = value;
//        }

//    }



//}


