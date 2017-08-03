//using System.Web;
//using Microsoft.AspNet.SignalR;
//using Microsoft.AspNet.SignalR.Hubs;
//using NightChat.WebApi.Common.Authorization;
//using Ninject;
//using AuthorizeAttribute = Microsoft.AspNet.SignalR.AuthorizeAttribute;

//namespace NightChat.WebApi.Facebook.Authorization
//{
//    //public class FacebookHubAuthorizeAttribute : AuthorizeAttribute
//    //{
//    //    [Inject]
//    //    public IFacebookAuthorization FacebookAuthorization { get; set; }

//    //    [Inject]
//    //    public IFormsAuthenticationService FormsAuthenticationService { get; set; }

//    //    public override bool AuthorizeHubConnection(HubDescriptor hubDescriptor, IRequest request)
//    //    {
//    //        var s = GlobalHost.HubPipeline;
//    //        HttpContextBase context = request.GetHttpContext();
//    //        if (!FacebookAuthorization.IsAuthorized(context.User))
//    //        {
//    //            FormsAuthenticationService.SignOut();
//    //            return false;
//    //        }
//    //        return true;
//    //    }
//    //}
//}