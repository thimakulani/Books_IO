using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using System.IO;

namespace Admin.FirebaseHelper
{
    public class FirebaseAdminSDK
    {
        public static FirebaseAdmin.Auth.FirebaseAuth GetFirebaseAdminAuth(Stream input)
        {
            FirebaseAdmin.Auth.FirebaseAuth auth;
            FirebaseAdmin.AppOptions options = new FirebaseAdmin.AppOptions()
            {
                Credential = GoogleCredential.FromStream(input),
                ProjectId = "main-event-ce70c",
                ServiceAccountId = "firebase-adminsdk-oeoec@main-event-ce70c.iam.gserviceaccount.com",

            };
            if (FirebaseAdmin.FirebaseApp.DefaultInstance == null)
            {
                var app = FirebaseAdmin.FirebaseApp.Create(options);
                auth = FirebaseAdmin.Auth.FirebaseAuth.GetAuth(app);
            }
            else
            {
                auth = FirebaseAdmin.Auth.FirebaseAuth.DefaultInstance;
            }
            return auth;
        }
        public static FirebaseMessaging GetFirebaseMessaging(Stream input)
        {
            FirebaseMessaging messaging;
            
            FirebaseAdmin.AppOptions options = new FirebaseAdmin.AppOptions()
            {
                Credential = GoogleCredential.FromStream(input),
                ProjectId = "main-event-ce70c",
                ServiceAccountId = "firebase-adminsdk-oeoec@main-event-ce70c.iam.gserviceaccount.com",

            };
            if (FirebaseAdmin.FirebaseApp.DefaultInstance == null)
            {
                var app = FirebaseAdmin.FirebaseApp.Create(options);
                messaging = FirebaseMessaging.GetMessaging(app);
            }
            else
            {
                messaging = FirebaseMessaging.DefaultInstance;//.FirebaseAuth.DefaultInstance;
            }
            
            return messaging;
        }
    }
}