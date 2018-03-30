using System;
using System.ComponentModel;
using AppRopio.Base.iOS.Models.ThemeConfigs;
using CoreGraphics;
using Foundation;
using UIKit;

namespace AppRopio.Base.iOS.Controls
{
    [Register("ARTextField"), DesignTimeVisible(true)]
    public class ARTextField : UITextField
    {
        #region Fields

        private const float TEXT_HORIZONTAL_MARGINS = 16;
        private const float TEXT_BOTTOM_MARGINS = 26;

        private UIView _separator;
        private ARLabel _placeholderLabel;

        private UIColor _placeholderColor;
        private UIFont _placeholderFont;

        private UIColor _defaultBackgroundColor;
        private UIColor _tintBackgroundColor;

        #endregion

        #region Properties

        protected bool OnEditing { get; set; }

        private UIColor _separatorColor;
        [Export("SeparatorColor"), Browsable(true)]
        public UIColor SeparatorColor
        {
            get => _separatorColor;
            set
            {
                _separatorColor = value;

                UpdateSeparatorBackground();

                base.TintColor = value;
            }
        }

        [Export("InvalidTintColor"), Browsable(true)]
        public UIColor InvalidTintColor { get; set; }

        private TextTransform _textTransform;
        public TextTransform TextTransform
        {
            get => _textTransform;
            set
            {
                _textTransform = value;

                if (_placeholderLabel != null)
                    _placeholderLabel.TextTransform = value;

                ApplyTransform(value);
            }
        }

        private float? _kerning;
        public float? Kerning
        {
            get => _kerning;
            set
            {
                _kerning = value;

                if (_placeholderLabel != null)
                    _placeholderLabel.Kerning = value;

                ApplyKerning(value);
                ApplyTypingKerning(value);
            }
        }

        private bool _error;
        public bool Error
        {
            get => _error;
            set
            {
                if (_error != value)
                {
                    _error = value;
                    ErrorChanged?.Invoke(this, EventArgs.Empty);
                }

                UpdateSeparatorBackground();
            }
        }

        public EventHandler ErrorChanged;

        public override UIColor TintColor
        {
            get => base.TintColor;
            set
            {
                base.TintColor = value;

                if (value != SeparatorColor)
                    _tintBackgroundColor = value;

            }
        }

        private string _placeholder;
        public override string Placeholder
        {
            get => _placeholder ?? (_placeholder = base.Placeholder);
            set
            {
                _placeholder = value;

                UpdatePlaceholderText();

                base.Placeholder = " ";
            }
        }

        private NSAttributedString _attributedPlaceholder;
        public override NSAttributedString AttributedPlaceholder
        {
            get => _attributedPlaceholder;
            set
            {
                var attrStr = new NSMutableAttributedString(value);
                attrStr.MutableString.SetString(new NSString(" "));
                _attributedPlaceholder = attrStr;

                if (value.Length > 0)
                {
                    var attrs = value.GetAttributes(0, out NSRange eR);

                    _placeholderFont = attrs.ValueForKey(UIStringAttributeKey.Font) as UIFont ?? Font;
                    _placeholderColor = attrs.ValueForKey(UIStringAttributeKey.ForegroundColor) as UIColor ?? TextColor;

                    UpdatePlaceholderText();
                }

                base.AttributedPlaceholder = attrStr;
            }
        }

        public override UIColor BackgroundColor
        {
            get
            {
                return base.BackgroundColor;
            }
            set
            {
                base.BackgroundColor = value;

                if (value == null)
                    _defaultBackgroundColor = value;
                else if (value != null)
                {
                    value.GetRGBA(out nfloat red, out nfloat green, out nfloat blue, out nfloat alpha);
                    _tintBackgroundColor.GetRGBA(out nfloat tintRed, out nfloat tintGreen, out nfloat tintBlue, out nfloat tintAlpha);

                    if (red != tintRed && green != tintGreen && blue != tintBlue)
                        _defaultBackgroundColor = value;
                }
            }
        }

        public override NSAttributedString AttributedText
        {
            get
            {
                ApplyKerning(Kerning);
                return base.AttributedText;
            }
            set
            {
                base.AttributedText = value;
                ApplyKerning(Kerning);
            }
        }

        #endregion

        #region Constructor

        public ARTextField()
        {
            this.EditingDidBegin += OnEditingDidBegin;
            this.EditingDidEnd += OnEditingDidEnd;
        }

        public ARTextField(IntPtr handle)
            : base(handle)
        {
            this.EditingDidBegin += OnEditingDidBegin;
            this.EditingDidEnd += OnEditingDidEnd;
        }

        public ARTextField(NSCoder coder)
            : base(coder)
        {
        }

        protected ARTextField(NSObjectFlag t)
            : base(t)
        {
        }

        public ARTextField(CGRect frame)
            : base(frame)
        {
        }

        #endregion

        #region Private

        private void OnEditingDidBegin(object sender, EventArgs e)
        {
            BackgroundColor = _tintBackgroundColor.ColorWithAlpha(0f);

            Animate(0.3f, () => BackgroundColor = _tintBackgroundColor.ColorWithAlpha(_tintBackgroundColor.CGColor.Alpha));

            OnEditing = true;

            UpdatePlaceholder();

            ApplyTypingKerning(Kerning);
        }

        private void OnEditingDidEnd(object sender, EventArgs e)
        {
            BackgroundColor = _defaultBackgroundColor;

            OnEditing = false;

            UpdatePlaceholder();

            ApplyKerning(Kerning);
        }

        private void UpdatePlaceholder(bool animated = true)
        {
            UpdatePlaceholderText();

            if (animated)
                Animate(0.3f, () => _placeholderLabel.Frame = PlaceholderRect(Bounds));
            else
                _placeholderLabel.Frame = PlaceholderRect(Bounds);
        }

        private void UpdatePlaceholderText()
        {
            if (_placeholderLabel == null || _placeholderFont == null || _placeholderColor == null)
                return;

            var attrStr = new NSMutableAttributedString(
                Placeholder ?? " ",
                font: (OnEditing || !Text.IsNullOrEmpty() ? _placeholderFont.WithSize(_placeholderFont.PointSize * 0.75f) : _placeholderFont),
                foregroundColor: (OnEditing ? TextColor : _placeholderColor));

            _placeholderLabel.AttributedText = attrStr;
        }

        private void UpdateSeparatorBackground()
        {
            if (_separator != null)
                _separator.BackgroundColor = !Error ? SeparatorColor : InvalidTintColor;
        }

        private void ApplyTransform(TextTransform value)
        {
            switch (value)
            {
                case TextTransform.Uppercase:
                    AutocapitalizationType = UITextAutocapitalizationType.AllCharacters;
                    break;
                default:
                    AutocapitalizationType = UITextAutocapitalizationType.None;
                    break;
            }
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

            ApplyTransform(TextTransform);
        }

        private void ApplyTypingKerning(float? value)
        {
            var attrs = base.TypingAttributes;
            if (attrs != null && value.HasValue)
            {
                var mutableAttrs = new NSMutableDictionary(attrs);

                if (mutableAttrs.ContainsKey(UIStringAttributeKey.KerningAdjustment))
                    mutableAttrs[UIStringAttributeKey.KerningAdjustment] = NSNumber.FromFloat(value ?? 0f);
                else
                    mutableAttrs.Add(UIStringAttributeKey.KerningAdjustment, NSNumber.FromFloat(value ?? 0f));

                base.TypingAttributes = mutableAttrs;
            }
            else if (value.HasValue)
            {
                var mutableAttrs = new NSMutableDictionary();

                if (base.AttributedText != null)
                {
                    var attrStr = new NSMutableAttributedString(base.AttributedText);

                    if (attrStr.Length > 0)
                    {
                        var dict = attrStr.GetAttributes(0, out NSRange error);
                        if (dict != null)
                        {
                            mutableAttrs = new NSMutableDictionary(dict);
                            if (mutableAttrs.ContainsKey(UIStringAttributeKey.KerningAdjustment))
                                mutableAttrs[UIStringAttributeKey.KerningAdjustment] = NSNumber.FromFloat(value ?? 0f);
                            else
                            {
                                mutableAttrs.Add(UIStringAttributeKey.KerningAdjustment, NSNumber.FromFloat(value ?? 0f));
                                attrStr.SetAttributes(mutableAttrs, new NSRange(0, attrStr.Length));
                            }

                            base.TypingAttributes = mutableAttrs;

                            return;
                        }
                    }

                    mutableAttrs.Add(UIStringAttributeKey.KerningAdjustment, NSNumber.FromFloat(value ?? 0f));
                    mutableAttrs.Add(UIStringAttributeKey.Font, Font);
                    mutableAttrs.Add(UIStringAttributeKey.ForegroundColor, TextColor);

                    base.TypingAttributes = mutableAttrs;
                }
            }
        
            ApplyTransform(TextTransform);
        }

        #endregion

        #region Public

        public override void SetNeedsDisplay()
        {
            base.SetNeedsDisplay();

            if (_separator == null)
            {
                _separator = new UIView();
                _separator.TranslatesAutoresizingMaskIntoConstraints = false;
                _separator.AddConstraint(NSLayoutConstraint.Create(_separator, NSLayoutAttribute.Height, NSLayoutRelation.Equal, 1, 1));

                UpdateSeparatorBackground();

                AddSubviews(_separator);

                AddConstraints(new[]
                {
                    NSLayoutConstraint.Create(_separator, NSLayoutAttribute.Leading, NSLayoutRelation.Equal, this, NSLayoutAttribute.Leading, 1, 16),
                    NSLayoutConstraint.Create(_separator, NSLayoutAttribute.Trailing, NSLayoutRelation.Equal, this, NSLayoutAttribute.Trailing, 1, 0),
                    NSLayoutConstraint.Create(_separator, NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, this, NSLayoutAttribute.Bottom, 1, -16),
                });
            }

            if (_placeholderLabel == null)
            {
                _placeholderLabel = new ARLabel { TextTransform = this.TextTransform, Kerning = this.Kerning };

                UpdatePlaceholderText();

                AddSubview(_placeholderLabel);
            }
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            _placeholderLabel.Frame = PlaceholderRect(Bounds);
            BringSubviewToFront(_placeholderLabel);
            UpdatePlaceholder(false);
        }

        public override CoreGraphics.CGRect PlaceholderRect(CoreGraphics.CGRect forBounds)
        {
            var placeholderSize = new NSString(Placeholder ?? " ").GetSizeUsingAttributes((_placeholderLabel?.AttributedText ?? AttributedPlaceholder).GetUIKitAttributes(0, out NSRange effectiveRange));

            var rect = new CGRect();

            rect = new CoreGraphics.CGRect(
                TEXT_HORIZONTAL_MARGINS,
                forBounds.Height - placeholderSize.Height - TEXT_BOTTOM_MARGINS - (OnEditing ? Font.LineHeight + Font.xHeight : (Text.IsNullOrEmpty() ? 0 : Font.LineHeight + Font.xHeight)),
                forBounds.Width - (TEXT_HORIZONTAL_MARGINS * 2),
                placeholderSize.Height
            );

            return rect;
        }

        public override CoreGraphics.CGRect TextRect(CoreGraphics.CGRect forBounds)
        {
            var textRect = base.TextRect(forBounds);

            textRect = new CoreGraphics.CGRect(
                TEXT_HORIZONTAL_MARGINS,
                forBounds.Height - Font.LineHeight - TEXT_BOTTOM_MARGINS,
                textRect.Width - TEXT_HORIZONTAL_MARGINS * 2,
                Font.LineHeight * 2
            );

            return textRect;
        }

        public override CoreGraphics.CGRect EditingRect(CoreGraphics.CGRect forBounds)
        {
            var editingRect = base.EditingRect(forBounds);

            editingRect = new CoreGraphics.CGRect(
                TEXT_HORIZONTAL_MARGINS,
                forBounds.Height - Font.LineHeight - TEXT_BOTTOM_MARGINS,
                editingRect.Width - TEXT_HORIZONTAL_MARGINS * 2,
                Font.LineHeight * 2
            );

            return editingRect;
        }

        public override void RemoveFromSuperview()
        {
            this.EditingDidBegin -= OnEditingDidBegin;
            this.EditingDidEnd -= OnEditingDidEnd;

            if (_placeholderLabel != null)
            {
                _placeholderLabel.RemoveFromSuperview();
                _placeholderLabel.Dispose();
                _placeholderLabel = null;
            }

            if (_separator != null)
            {
                _separator.RemoveFromSuperview();
                _separator.Dispose();
                _separator = null;
            }

            _separatorColor = null;
            _placeholderColor = null;
            _placeholderFont = null;
            _defaultBackgroundColor = null;

            base.RemoveFromSuperview();
        }

        #endregion
    }
}
