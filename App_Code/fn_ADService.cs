using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.DirectoryServices;
using System.Collections;
using System.Security.Principal;
using System.Diagnostics;
using System.Collections.Specialized;

/// <summary>
/// AD 資訊
/// </summary>
public class ADService
{
    #region - AD資訊取得 -
    /// <summary>
    /// 取得使用者/群組屬性[0: SID, 1:name, 2:sAMAccountName, 3.Guid, 4.群組]
    /// </summary>
    /// <returns>字串集合</returns>
    public StringCollection getADAttributes(out string ErrMsg)
    {
        try
        {
            ErrMsg = "";

            //[判斷參數] - 使用者帳號
            if (string.IsNullOrEmpty(AD_UserName))
            {
                ErrMsg = "使用者帳號空白";
                return null;
            }
            //[判斷參數] - 使用者密碼
            if (string.IsNullOrEmpty(AD_UserPwd))
            {
                ErrMsg = "使用者密碼空白";
                return null;
            }

            StringCollection result = new StringCollection();
            DirectoryEntry mEntry = new DirectoryEntry(AD_Path, AD_DomainUserName(), AD_UserPwd);
            DirectorySearcher mySearcher = new DirectorySearcher();

            mySearcher.SearchRoot = mEntry;
            //設定條件, 判斷帳號
            mySearcher.Filter = "(SAMAccountName=" + AD_UserName + ")";
            mySearcher.PropertiesToLoad.Add("name");
            mySearcher.PropertiesToLoad.Add("sAMAccountName");

            SearchResult groupResult = mySearcher.FindOne();
            if (groupResult != null)
            {
                DirectoryEntry groupEntry = groupResult.GetDirectoryEntry();

                //新增 - SID
                byte[] byteArray = (byte[])groupEntry.Properties["objectSid"][0];
                SecurityIdentifier mySID = new SecurityIdentifier(byteArray, 0);
                result.Add(mySID.ToString());

                //新增 - name, sAMAccountName
                try
                {
                    result.Add(groupEntry.Properties["name"][0].ToString());
                    result.Add(groupEntry.Properties["sAMAccountName"][0].ToString());
                }
                catch (Exception)
                {
                    result.Add("");
                    result.Add("");
                }

                //新增 - Guid
                result.Add(groupEntry.Guid.ToString("B"));

                //新增 - 群組
                ArrayList groupAry = getGroupGUID(groupResult);
                result.Add(string.Join(",", groupAry.ToArray()));
            }
            else
            {
                ErrMsg = "找不到這個使用者";
                return null;
            }

            return result;
        }
        catch (Exception ex)
        {
            ErrMsg = ex.Message.ToString();
            return null;
        }
    }

    /// <summary>
    /// 取得Group的GUID
    /// </summary>
    /// <param name="groupResult"></param>
    /// <returns></returns>
    private ArrayList getGroupGUID(SearchResult groupResult)
    {
        try
        {
            ArrayList result = new ArrayList();

            if (groupResult != null)
            {
                DirectoryEntry objectEntry = groupResult.GetDirectoryEntry();
                String EntryPath = objectEntry.Path;
                objectEntry.Close();
                result = getAllParentsGUIDFromPath(EntryPath);
            }

            return result;
        }
        catch (Exception)
        {
            throw;
        }

    }

    /// <summary>
    /// Get all Parents  GUID in Format of {xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx} from this Directory Entry
    /// </summary>
    /// <param name="EntryPath">Directory Entry</param>
    /// <returns>ArrayList of GUID</returns>
    private ArrayList getAllParentsGUIDFromPath(String EntryPath)
    {
        try
        {
            ArrayList result = new ArrayList();
            //Add itself to ArrayList 
            DirectoryEntry myEntry = new DirectoryEntry(EntryPath, AD_DomainUserName(), AD_UserPwd);
            //Guid
            result.Add(myEntry.Guid.ToString("B"));

            foreach (object obj in myEntry.Properties["memberOf"])
            {
                //Use Recursiv to find parent groups
                ArrayList tempResult = getAllParentsGUIDFromPath(string.Format("LDAP://{0}/{1}", AD_Domain, obj.ToString()));
                foreach (object tempObj in tempResult)
                {
                    result.Add(tempObj);
                }
            }

            return result;
        }
        catch (Exception)
        {
            throw;
        }

    }
    #endregion


    #region - 參數設定-
    /// <summary>
    /// LDAP路徑(Webconfig)
    /// </summary>
    private string _AD_Path;
    private string AD_Path
    {
        get
        {
            return System.Web.Configuration.WebConfigurationManager.AppSettings["AD_Path"];
        }
        set
        {
            _AD_Path = value;
        }
    }

    /// <summary>
    /// AD Domain(Webconfig)
    /// </summary>
    private string _AD_Domain;
    private string AD_Domain
    {
        get
        {
            return System.Web.Configuration.WebConfigurationManager.AppSettings["AD_Domain"];
        }
        set
        {
            _AD_Domain = value;
        }
    }

    /// <summary>
    /// AD 帳號
    /// </summary>
    private string _AD_UserName;
    public string AD_UserName
    {
        get;
        set;
    }

    /// <summary>
    /// AD 密碼
    /// </summary>
    private string _AD_UserPwd;
    public string AD_UserPwd
    {
        get;
        set;
    }

    public string AD_DomainUserName()
    {
        return AD_Domain + @"\" + AD_UserName;
    }
    #endregion


}