using System;
using CoreGraphics;
using UIKit;
using Foundation;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;

namespace AppRopio.Base.iOS.UIExtentions
{
    public static class UIViewExtentionMethods
    {
        public static UITextView SetPlaceholder(this UITextView self, string hint, UIColor textColor, UIColor hintColor, bool resignResponderOnReturn = true)
        {
            if (resignResponderOnReturn)
            {
                self.ShouldChangeText = (textView, range, text) =>
                    {
                        if (text.Contains("\n"))
                        {
                            self.ResignFirstResponder();
                            return false;
                        }

                        return true;
                    };
            }

            self.ShouldBeginEditing = (textView) =>
                {
                    if (textView.Text == hint)
                        textView.Text = "";
                    textView.TextColor = textColor;

                    return true;
                };
            self.ShouldEndEditing = (textView) =>
                {
                    if (textView.Text == "")
                    {
                        textView.Text = hint;
                        textView.TextColor = hintColor;
                    }

                    return true;
                };

            return self;
        }

        public static CGPoint GetAbsolutePositionIn(this UIView subview, UIView view)
        {
            if (subview == null)
                throw new NullReferenceException();

            if (view == null)
                throw new ArgumentNullException(nameof(view));

            if (view.Subviews.Contains(subview))
                return new CGPoint(subview.Frame.X, subview.Frame.Y);
            else
            {
                var p = subview.Superview.GetAbsolutePositionIn(view);
                return new CGPoint(p.X + subview.Frame.X, p.Y + subview.Frame.Y);
            }
        }

        public static bool IsInsideOf(this UIView subview, UIView view)
        {
            if (subview == null)
                throw new NullReferenceException();

            if (view == null)
                throw new ArgumentNullException(nameof(view));

            var superview = subview.Superview;
            while (superview != null)
            {
                if (superview.Equals(view))
                    return true;
                superview = superview.Superview;
            }

            return false;
        }

        public static UILabel WithLineSpacing(this UILabel label, float lineSpacing)
        {
            var alignment = label.TextAlignment;

            var stringAttributes = new UIStringAttributes
            {
                ParagraphStyle = new NSMutableParagraphStyle() { LineSpacing = lineSpacing }
            };

            var attributedText = new NSMutableAttributedString(label.Text);
            attributedText.AddAttributes(stringAttributes, new NSRange(0, label.Text.Length));
            label.AttributedText = attributedText;
            label.TextAlignment = alignment;
            return label;
        }

        public static UIView SetFrameAnimated(this UIView self, CGRect frame, double duration = 0.22, Action completion = null)
        {
            if (completion == null)
                completion = () => { };

            UIView.Animate(duration, () =>
               {
                   self.Frame = frame;
               }, completion);

            return self;
        }

        public static UIView SetAlphaAnimated(this UIView self, float alpha, double duration = 0.22, double delay = 0, Action completion = null)
        {
            if (completion == null)
                completion = () => { };

            UIView.Animate(duration, () =>
               {
                   self.Alpha = alpha;
               }, completion);

            return self;
        }

        public static UIView SetHiddenAnimated(this UIView self, bool hidden, double duration = 0.22, double delay = 0, Action completion = null)
        {
            if (completion == null)
                completion = () => { };

            UIView.Animate(duration, () =>
               {
                   self.Alpha = hidden ? 0 : 1;
                   self.Hidden = hidden;
               }, completion);

            return self;
        }

        public static UIButton WithButtonImage(this UIButton self, string filePath, UIControlState state)
        {
            self.SetImage(UIImage.FromFile(filePath), state);

            return self;
        }

        public static UIButton WithButtonImage(this UIButton self, UIImage image, UIControlState state)
        {
            self.SetImage(image, state);

            return self;
        }

        public static UIButton WithButtonImageInsets(this UIButton self, UIEdgeInsets insets)
        {
            self.ImageEdgeInsets = insets;

            return self;
        }

        public static UIButton WithButtonTitleInsets(this UIButton self, UIEdgeInsets insets)
        {
            self.TitleEdgeInsets = insets;

            return self;
        }

        public static UIButton WithBackgroundImageForAllStates(this UIButton self, UIImage image)
        {
            self.SetBackgroundImage(image, UIControlState.Normal);
            self.SetBackgroundImage(image, UIControlState.Highlighted);
            self.SetBackgroundImage(image, UIControlState.Application);
            self.SetBackgroundImage(image, UIControlState.Disabled);
            self.SetBackgroundImage(image, UIControlState.Reserved);
            self.SetBackgroundImage(image, UIControlState.Selected);

            return self;
        }

        public static UIButton WithAttributedTitleForAllStates(this UIButton self, NSAttributedString title)
        {
            self.SetAttributedTitle(title, UIControlState.Normal);
            self.SetAttributedTitle(title, UIControlState.Highlighted);
            self.SetAttributedTitle(title, UIControlState.Application);
            self.SetAttributedTitle(title, UIControlState.Disabled);
            self.SetAttributedTitle(title, UIControlState.Reserved);
            self.SetAttributedTitle(title, UIControlState.Selected);

            return self;
        }

        public static UIButton WithTitleForAllStates(this UIButton self, string title)
        {
            self.SetTitle(title, UIControlState.Normal);
            self.SetTitle(title, UIControlState.Highlighted);
            self.SetTitle(title, UIControlState.Application);
            self.SetTitle(title, UIControlState.Disabled);
            self.SetTitle(title, UIControlState.Reserved);
            self.SetTitle(title, UIControlState.Selected);

            return self;
        }

        public static UIButton WithTitleColorForAllStates(this UIButton self, UIColor color)
        {
            self.SetTitleColor(color, UIControlState.Normal);
            self.SetTitleColor(color, UIControlState.Highlighted);
            self.SetTitleColor(color, UIControlState.Application);
            self.SetTitleColor(color, UIControlState.Disabled);
            self.SetTitleColor(color, UIControlState.Reserved);
            self.SetTitleColor(color, UIControlState.Selected);

            return self;
        }

        public static T WithSubviews<T>(this T self, params UIView[] subviews)
            where T : UIView
        {
            self.AddSubviews(subviews);
            return self;
        }

        public static T WithTune<T>(this T self, Action<T> tune)
            where T : UIView
        {
            if (tune != null)
                tune(self);
            return self;
        }

        public static T WithFrame<T>(this T self, CGRect frame)
            where T : UIView
        {
            self.Frame = frame;
            return self;
        }

        public static T WithFrame<T>(this T self, nfloat left, nfloat top, nfloat width, nfloat height)
            where T : UIView
        {
            self.Frame = new CGRect(left, top, width, height);
            return self;
        }

        public static T WithBackground<T>(this T self, UIColor backgroundColor)
            where T : UIView
        {
            self.BackgroundColor = backgroundColor;
            return self;
        }

        public static T WithBackground<T>(this T self, int red, int green, int blue)
            where T : UIView
        {
            self.BackgroundColor = UIColor.FromRGB(red, green, blue);
            return self;
        }

        public static T WithBackground<T>(this T self, string backgroundColor)
            where T : UIView
        {
            self.BackgroundColor = backgroundColor.ToUIColor();
            return self;
        }

        public static T WithAlpha<T>(this T self, nfloat alpha)
            where T : UIView
        {
            self.Alpha = alpha;
            return self;
        }

        public static UILabel WithLineBreakMode(this UILabel self, UILineBreakMode lineBreakMode)
        {
            self.LineBreakMode = lineBreakMode;
            return self;
        }

        public static UILabel WithTextAlignment(this UILabel self, UITextAlignment textAlignment)
        {
            self.TextAlignment = textAlignment;
            return self;
        }

        public static UILabel WithLinesNumber(this UILabel self, int linesNumber)
        {
            self.Lines = linesNumber;
            return self;
        }

        public static List<UIView> AllSubviews(this UIView self, params Type[] types)
        {
            var result = new List<UIView>();
            CollectSubviews(self, result, types);
            return result;
        }

        private static void CollectSubviews(UIView parent, List<UIView> views, Type[] types)
        {
            foreach (var view in parent.Subviews)
            {
                var valid = true;
                if (types.Length > 0)
                {
                    valid = false;
                    for (int i = 0; i < types.Length; i++)
                    {
                        if (types[i].IsAssignableFrom(view.GetType()))
                        {
                            valid = true;
                            break;
                        }
                    }
                }
                if (valid)
                    views.Add(view);
                CollectSubviews(view, views, types);
            }
        }

        public static UILabel WithUnderlinedText(this UILabel self, NSUnderlineStyle style)
        {
            self.AttributedText = new NSAttributedString(self.Text, new UIStringAttributes
            {
                UnderlineStyle = style,
                Font = self.Font,
                Shadow = new NSShadow { ShadowColor = UIColor.White, ShadowOffset = new CGSize(0, 1) },
                ForegroundColor = self.TextColor
            });

            return self;
        }

        public static void AddAttributedPlaceholder(this UITextField textField, string text, UIFont font, UIColor color)
        {
            textField.AttributedPlaceholder = new NSMutableAttributedString(text, font, color);
        }
    }
}

