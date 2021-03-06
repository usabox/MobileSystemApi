﻿<%@ WebHandler Language="C#" Class="MemberSessionCheck" %>

using System.IO;
using System.Threading.Tasks;
using System.Web;
using W88.BusinessLogic.Rewards.Helpers;
using W88.BusinessLogic.Rewards.Models;
using W88.Utilities;
using Members = W88.BusinessLogic.Accounts.Helpers.Members;

public class MemberSessionCheck : HttpTaskAsyncHandler, System.Web.SessionState.IReadOnlySessionState
{
    public override async Task ProcessRequestAsync(HttpContext context)
    {   
        var token = new StreamReader(context.Request.InputStream).ReadToEnd();
        var process = await (new Members()).MembersSessionCheck(token);
        context.Response.ContentType = "application/json";
        context.Response.Write(Common.SerializeObject(process));
        context.Response.End();
    }
    
    public bool IsReusable {
        get {
            return false;
        }
    }
}