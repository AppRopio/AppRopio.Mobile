// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace AppRopio.Feedback.iOS.Views.ReviewPost.Cell.Text
{
    [Register ("TextCell")]
    partial class TextCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextView TextView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (TextView != null) {
                TextView.Dispose ();
                TextView = null;
            }
        }
    }
}