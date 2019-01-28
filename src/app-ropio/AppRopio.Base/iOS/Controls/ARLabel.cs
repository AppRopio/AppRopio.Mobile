using System;
using System.ComponentModel;
using AppRopio.Base.iOS.Models.ThemeConfigs;
using AppRopio.Base.iOS.UIExtentions;
using CoreGraphics;
using Foundation;
using UIKit;

namespace AppRopio.Base.iOS.Controls
{
    [Register("ARLabel"), DesignTimeVisible(true)]
    public class ARLabel : UILabel
    {
        public override string Text
        {
            get
            {
                return base.Text.ApplyTransform(TextTransform);
            }
            set
            {
                base.Text = value.ApplyTransform(TextTransform);
                AttributedText = new NSAttributedString(base.Text);
            }
        }

        public override NSAttributedString AttributedText
        {
            get
            {
                ApplyKerning(Kerning);
                ApplyTextDecoration(TextDecoration);
                return base.AttributedText;
            }
            set
            {
                base.AttributedText = value;
                ApplyKerning(Kerning);
                ApplyTextDecoration(TextDecoration);
                ApplyTransform();
            }
        }

        private TextTransform _textTransform;
        public TextTransform TextTransform
        {
            get => _textTransform;
            set
            {
                _textTransform = value;

                if (!string.IsNullOrEmpty(Text))
                    Text.ApplyTransform(value);
            }
        }

        private TextDecoration _textDecoration;
        public TextDecoration TextDecoration
        {
            get => _textDecoration;
            set
            {
                _textDecoration = value;

                ApplyTextDecoration(value);
            }
        }

        private float? _kerning;
        public float? Kerning 
        {
            get => _kerning;
            set
            {
                _kerning = value;
                ApplyKerning(value);
            }
        }

        public ARLabel()
        {
        }

        public ARLabel(NSCoder coder) 
            : base(coder)
        {
        }

        public ARLabel(CGRect frame) 
            : base(frame)
        {
        }

        protected ARLabel(NSObjectFlag t) 
            : base(t)
        {
        }

        protected internal ARLabel(IntPtr handle) 
            : base(handle)
        {
        }

        private void ApplyKerning(float? value)
        {
            if (base.AttributedText != null && value.HasValue)
            {
                var attrStr = new NSMutableAttributedString(base.AttributedText);

                if (attrStr.Length > 0)
                {
                    var attrs = attrStr.GetAttributes(0, out NSRange error);
                    if (attrs != null)
                    {
                        var mutableAttrs = new NSMutableDictionary(attrs);
                        if (mutableAttrs.ContainsKey(UIStringAttributeKey.KerningAdjustment))
                            mutableAttrs[UIStringAttributeKey.KerningAdjustment] = NSNumber.FromFloat(value ?? 0f);
                        else
                        {
                            mutableAttrs.Add(UIStringAttributeKey.KerningAdjustment, NSNumber.FromFloat(value ?? 0f));
                            attrStr.SetAttributes(mutableAttrs, new NSRange(0, attrStr.Length));
                        }

                        base.AttributedText = attrStr;
                    }
                }
            }
        }

        private void ApplyTextDecoration(TextDecoration value)
        {
            if (base.AttributedText != null && value != TextDecoration.None)
            {
                var attrStr = new NSMutableAttributedString(base.AttributedText);

                if (attrStr.Length > 0)
                {
                    var attrs = attrStr.GetAttributes(0, out NSRange error);
                    if (attrs != null)
                    {
                        if (value == TextDecoration.Stroke)
                        {
                            var mutableAttrs = new NSMutableDictionary(attrs);
                            if (mutableAttrs.ContainsKey(UIStringAttributeKey.StrikethroughStyle))
                            {
                                mutableAttrs[UIStringAttributeKey.StrikethroughStyle] = new NSNumber((long)NSUnderlineStyle.Thick);
                                mutableAttrs[UIStringAttributeKey.StrikethroughColor] = TextColor;
                            }
                            else
                            {
                                mutableAttrs.Add(UIStringAttributeKey.StrikethroughStyle, new NSNumber((long)NSUnderlineStyle.Thick));
                                mutableAttrs.Add(UIStringAttributeKey.StrikethroughColor, TextColor);
                                attrStr.SetAttributes(mutableAttrs, new NSRange(0, attrStr.Length));
                            }
                        }
                        else if (value == TextDecoration.Underline)
                        {
                            var mutableAttrs = new NSMutableDictionary(attrs);
                            if (mutableAttrs.ContainsKey(UIStringAttributeKey.UnderlineStyle))
                            {
                                mutableAttrs[UIStringAttributeKey.UnderlineStyle] = new NSNumber((long)NSUnderlineStyle.Single);
                                mutableAttrs[UIStringAttributeKey.UnderlineColor] = TextColor;
                            }
                            else
                            {
                                mutableAttrs.Add(UIStringAttributeKey.UnderlineStyle, new NSNumber((long)NSUnderlineStyle.Single));
                                mutableAttrs.Add(UIStringAttributeKey.UnderlineColor, TextColor);
                                attrStr.SetAttributes(mutableAttrs, new NSRange(0, attrStr.Length));
                            }
                        }

                        base.AttributedText = attrStr;
                    }
                }
            }
        }

        private void ApplyTransform()
        {
            if (base.AttributedText != null)
            {
                var attrStr = new NSMutableAttributedString(base.AttributedText);
                if (attrStr.Length > 0)
                {
                    attrStr.MutableString.SetString(new NSString(attrStr.Value.ApplyTransform(TextTransform)));
                    base.AttributedText = attrStr;
                }
            }
        }
    }
}
