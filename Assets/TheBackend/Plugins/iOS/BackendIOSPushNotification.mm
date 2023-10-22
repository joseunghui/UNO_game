#import <stdlib.h>
#import <objc/runtime.h>
#import <UIKit/UIKit.h>
#import <UserNotifications/UserNotifications.h>
#import <UnityAppController.h>

@interface BackendIOSPushNotification: NSObject
@end

@implementation BackendIOSPushNotification
const char* tokenStr = NULL;

+ (void)registerForPushNotifications {
    if (@available(iOS 10.0, *)) {
        UNUserNotificationCenter *center = [UNUserNotificationCenter currentNotificationCenter];
        [center requestAuthorizationWithOptions:(UNAuthorizationOptionBadge | UNAuthorizationOptionSound | UNAuthorizationOptionAlert)
                              completionHandler:^(BOOL granted, NSError * _Nullable error) {
                                  if (!error) {
                                      dispatch_async(dispatch_get_main_queue(), ^{
                                          [[UIApplication sharedApplication] registerForRemoteNotifications];
                                      });
                                  }
                              }];
    } else {
        UIUserNotificationType types = (UIUserNotificationTypeAlert | UIUserNotificationTypeSound | UIUserNotificationTypeBadge);
        UIUserNotificationSettings *settings = [UIUserNotificationSettings settingsForTypes:types categories:nil];
        [[UIApplication sharedApplication] registerUserNotificationSettings:settings];
        [[UIApplication sharedApplication] registerForRemoteNotifications];
    }
}

@end

extern "C" void activePushTokenByBackend() {
    [BackendIOSPushNotification registerForPushNotifications];
}

extern "C" const char* getTokenByBackend() {
    return tokenStr ? strdup(tokenStr) : "";
}

void didRegisterForRemoteNotificationsWithDeviceToken(id self, SEL _cmd, UIApplication* application, NSData *deviceToken) {
    
    if(tokenStr) {
        return;
    }
    
        const unsigned char *tokenBytes = (const unsigned char *)[deviceToken bytes];
        NSUInteger len = deviceToken.length;
        NSMutableString *str  = [NSMutableString stringWithCapacity: (len * 2)];
        
        for (int i = 0; i < len; ++i)
            [str appendFormat: @"%02x", tokenBytes[i]];
        
    if(tokenStr) {
        free(&tokenStr);
    }
    // Make a copy of the string
    tokenStr = strdup([str UTF8String]);
}

__attribute__((constructor))
static void initialize() {
    class_addMethod([UnityAppController class], @selector(application:didRegisterForRemoteNotificationsWithDeviceToken:), (IMP)didRegisterForRemoteNotificationsWithDeviceToken, "v@:@@");
}
