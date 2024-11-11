#import <UIKit/UIKit.h>

extern "C" {
    void ShowIosSnackbarNative(const char* message) {
        NSString* messageStr = [NSString stringWithUTF8String:message];
        
        dispatch_async(dispatch_get_main_queue(), ^{
            UIWindow* window = [UIApplication sharedApplication].keyWindow;
            if (window) {
                UIView* snackbarView = [[UIView alloc] init];
                snackbarView.backgroundColor = [UIColor darkGrayColor];
                snackbarView.layer.cornerRadius = 4;
                
                UILabel* label = [[UILabel alloc] init];
                label.text = messageStr;
                label.textColor = [UIColor whiteColor];
                label.textAlignment = NSTextAlignmentCenter;
                
                [snackbarView addSubview:label];
                [window addSubview:snackbarView];
                
                snackbarView.translatesAutoresizingMaskIntoConstraints = NO;
                label.translatesAutoresizingMaskIntoConstraints = NO;
                
                [NSLayoutConstraint activateConstraints:@[
                    [snackbarView.bottomAnchor constraintEqualToAnchor:window.bottomAnchor constant:-50],
                    [snackbarView.centerXAnchor constraintEqualToAnchor:window.centerXAnchor],
                    [label.topAnchor constraintEqualToAnchor:snackbarView.topAnchor constant:8],
                    [label.bottomAnchor constraintEqualToAnchor:snackbarView.bottomAnchor constant:-8],
                    [label.leadingAnchor constraintEqualToAnchor:snackbarView.leadingAnchor constant:16],
                    [label.trailingAnchor constraintEqualToAnchor:snackbarView.trailingAnchor constant:-16],
                ]];
                
                dispatch_after(dispatch_time(DISPATCH_TIME_NOW, (int64_t)(2.0 * NSEC_PER_SEC)), dispatch_get_main_queue(), ^{
                    [snackbarView removeFromSuperview];
                });
            }
        });
    }
}