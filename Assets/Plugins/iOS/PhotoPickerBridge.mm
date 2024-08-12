#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>
#import <Photos/Photos.h>

@interface PhotoPickerDelegate : NSObject<UIImagePickerControllerDelegate, UINavigationControllerDelegate>
{
    NSString* callbackGameObject;
}
- (void)pickPhoto:(NSString*)gameObject;
@end

@implementation PhotoPickerDelegate

- (void)pickPhoto:(NSString*)gameObject {
    callbackGameObject = gameObject;
    
    dispatch_async(dispatch_get_main_queue(), ^{
        UIImagePickerController* picker = [[UIImagePickerController alloc] init];
        picker.delegate = self;
        picker.sourceType = UIImagePickerControllerSourceTypePhotoLibrary;
        
        UIViewController *viewController = [[[UIApplication sharedApplication] keyWindow] rootViewController];
        while (viewController.presentedViewController != nil) {
            viewController = viewController.presentedViewController;
        }
        
        [viewController presentViewController:picker animated:YES completion:nil];
    });
}

- (void)imagePickerController:(UIImagePickerController *)picker didFinishPickingMediaWithInfo:(NSDictionary<UIImagePickerControllerInfoKey,id> *)info {
    NSURL *imageURL = [info objectForKey:UIImagePickerControllerImageURL];
    NSString *path = [imageURL path];
    
    UnitySendMessage([callbackGameObject UTF8String], "OnPhotoPicked", [path UTF8String]);
    
    [picker dismissViewControllerAnimated:YES completion:nil];
}

- (void)imagePickerControllerDidCancel:(UIImagePickerController *)picker {
    [picker dismissViewControllerAnimated:YES completion:nil];
}

@end

static PhotoPickerDelegate* delegateInstance = nil;

#ifdef __cplusplus
extern "C" {
#endif
    
void _OpenPhotoLibrary(const char* gameObject) {
    if (delegateInstance == nil) {
        delegateInstance = [[PhotoPickerDelegate alloc] init];
    }
    [delegateInstance pickPhoto:[NSString stringWithUTF8String:gameObject]];
}
    
#ifdef __cplusplus
}
#endif
