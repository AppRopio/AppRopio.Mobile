using System;
using UIKit;
using CoreGraphics;
using MvvmCross.Binding.ExtensionMethods;

namespace AppRopio.Base.iOS.UIExtentions
{
	public static class UIViewExtensions
	{
		#region Update Table Header or Footer

        static public UIView UpdatedHeaderOrFooter(UIView headerOrFooterView, nfloat offset = default(nfloat), params UIView[] autoSizeableViews)
        {
            if (headerOrFooterView == null)
                return null;
            
            nfloat height = offset;

            if (autoSizeableViews != null && autoSizeableViews.Count() > 0)
            {
                foreach (var item in autoSizeableViews)
                {
                    height += item.SystemLayoutSizeFittingSize(UIView.UILayoutFittingCompressedSize).Height;
                }
            }
            else
                height += headerOrFooterView.SystemLayoutSizeFittingSize(UIView.UILayoutFittingCompressedSize).Height;

            var footerFrame = headerOrFooterView.Frame;

            if (headerOrFooterView.Frame.Height != height)
            {
                footerFrame.Height = height;
                headerOrFooterView.Frame = footerFrame;
            }

            return headerOrFooterView;
        }

        static public void UpdateFooterHeight(this UITableView tableView, nfloat footerOffset = default(nfloat), params UIView[] autoSizeableViews)
        {
            if (tableView == null)
                return;

            tableView.TableFooterView = UpdatedHeaderOrFooter(tableView.TableFooterView);
        }

		static public void UpdateHeaderHeight(this UITableView tableView, nfloat headerOffset = default(nfloat), params UIView[] autoSizeableViews)
		{
            //TODO пока не стал трогать, но можно переписать на вариант как выше ↑
			if (tableView == null)
				return;

			var headerView = tableView.TableHeaderView;
			nfloat height = headerOffset;

			if (autoSizeableViews != null && autoSizeableViews.Count() > 0)
			{
				foreach (var item in autoSizeableViews)
				{
					height += item.SystemLayoutSizeFittingSize(UIView.UILayoutFittingCompressedSize).Height;
				}
			}
			else
				height += headerView.SystemLayoutSizeFittingSize(UIView.UILayoutFittingCompressedSize).Height;

			var headerFrame = headerView.Frame;

			if (headerView.Frame.Height != height)
			{
				headerFrame.Height = height;
				headerView.Frame = headerFrame;

				tableView.TableHeaderView = headerView;
			}
		}

		#endregion

        #region AddSubviews

        /// <summary>
        /// Add constraints with insets to each side of superview
        /// </summary>
        public static void AddSubviewWithFill(this UIView superview, UIView view, UIEdgeInsets insets)
        {
            superview.AddSubview(view);

            superview.TopAnchor.ConstraintEqualTo(view.TopAnchor, insets.Top).Active = true;
            superview.RightAnchor.ConstraintEqualTo(view.RightAnchor, insets.Right).Active = true;
            superview.BottomAnchor.ConstraintEqualTo(view.BottomAnchor, insets.Bottom).Active = true;
            superview.LeftAnchor.ConstraintEqualTo(view.LeftAnchor, insets.Left).Active = true;
        }

        /// <summary>
        /// Add equal constraints to each side of superview
        /// </summary>
        public static void AddSubviewWithFill(this UIView superview, UIView view)
        {
            superview.AddSubviewWithFill(view, UIEdgeInsets.Zero);
        }

        #endregion

		#region Change frame

		public static void ChangeFrame(this UIView control, nfloat? x = null, nfloat? y = null, nfloat? w = null, nfloat? h = null)
		{
			x = x ?? control.Frame.X;
			y = y ?? control.Frame.Y;
			w = w ?? control.Frame.Width;
			h = h ?? control.Frame.Height;

			control.Frame = new CGRect(x.Value, y.Value, w.Value, h.Value);
		}

		public static void SetCenterXFrom(this UILabel control, UILabel sourceControl)
		{
			control.Frame = new CGRect(sourceControl.Frame.X + (sourceControl.Frame.Width - control.Frame.Width) / 2,
				control.Frame.Y, control.Frame.Width, control.Frame.Height);
		}

		/// <summary>
		/// Выставляет вьюху по центру (ось X)
		/// </summary>
		public static void SetCenterXFrom(this UIView control, UIView parentControl)
		{
			control.ChangeFrame((parentControl.Bounds.Width - control.Frame.Width) / 2);
		}

		/// <summary>
		/// Выставляет вьюху по центру (ось Y)
		/// </summary>
		public static void SetCenterYFrom(this UIView control, UIView parentControl)
		{
			control.ChangeFrame(y: (parentControl.Bounds.Height - control.Frame.Height) / 2);
		}

		public static void SetCenterXYFrom(this UIView control, UIView parentControl)
		{
			control.SetCenterXFrom(parentControl);
			control.SetCenterYFrom(parentControl);
		}

		/// <summary>
		/// Выставляет вьюху справа (ось X)
		/// </summary>
		public static void SetRightAlignmentFrom(this UIView control, UIView parentControler, float rightPadding = 0f)
		{
			control.ChangeFrame(x: parentControler.Bounds.Right - control.Frame.Width - rightPadding);
		}

		/// <summary>
		/// Выставляет вьюху снизу (ось Y)
		/// </summary>
		public static void SetBottomAlignmentFrom(this UIView control, UIView parentControler, float bottomPadding = 0)
		{
			control.ChangeFrame(y: parentControler.Bounds.Height - control.Frame.Height - bottomPadding);
		}


		/// <summary>
		/// Удаляет вьюху. Метод с проверкой  на null
		/// </summary>
		public static void DeleteView(this UIView control)
		{
			if (control != null)
			{
				control.RemoveFromSuperview();
				control = null;
			}
		}

		/// <summary>
		/// Делает SizeToFit и задает получившуюся высоту
		/// </summary>
		/// <returns>Bottom position of view</returns>
		/// <param name="view">View.</param>
		/// <param name = "yPosition">Можно задать позицию, которая будет после выполнения SizeTofit</param>
		/// <param name = "saveDefaultWidth">Сохраняет значение предустановленной ширины представления</param>
		/// <param name = "minHeight">Есть возможность задать минимальную высоту. В случае, если высота после SizeToFit будет меньше заданной, будет использоваться minHeight</param>
		public static nfloat ResizeView(this UIView view, nfloat? yPosition = null, bool saveDefaultWidth = true, float minHeight = 0)
		{
			var defaultWidth = view.Frame.Width;
			view.SizeToFit();
			view.ChangeFrame(y: yPosition ?? view.Frame.Y,
				w: saveDefaultWidth ? defaultWidth : view.Frame.Width,
				h: (minHeight > 0 && view.Frame.Height < minHeight) ? minHeight : view.Frame.Height);
			return view.Frame.Bottom;
		}

		/// <summary>
		/// Border the specified control, color and width.
		/// </summary>
		/// <param name="control">Control.</param>
		/// <param name="color">Color.</param>
		/// <param name="width">Width.</param>
		public static void Border(this UIView control, UIColor color, float width = 1)
		{
			control.Layer.BorderColor = color.CGColor;
			control.Layer.BorderWidth = width;
		}


		#endregion

		#region loadFromFile

		/// <summary>
		/// Создает изображение из файла по заданному пути
		/// </summary>
		public static UIImage LoadFromFile(this string path, bool throwException = true)
		{
			if (string.IsNullOrEmpty(path))
				return new UIImage();

			UIImage resultImage = UIImage.FromFile(path);

#if DEBUG
			if (resultImage == null && throwException)
				throw new Exception(string.Format("Отсутствует изображение по пути {0}", path));
#endif
			return resultImage;
		}

		/// <summary>
		/// Загружает изображение из файла по заданному пути автоматически в зависимости от версии iOS
		/// </summary>
		/// <param name="path">Путь к картинке</param>
		/// <para name="throwIfNotExists">Бросать исключение, если изображение отсутствует</para>
		public static UIImage LoadFromFileDeviceIndependent(string path, bool throwIfNotExists = true)
		{
			var imagePathTemplate = path.Replace(".png", "{0}.png");

			//UIImage.FromFile автоматически загружает изображение под ретину, если оно есть    

			UIImage resultImage = null;

			//если нет, пробуем загрузить обычную
			if (resultImage == null)
				resultImage = UIImage.FromFile(string.Format(imagePathTemplate, string.Empty));

#if DEBUG
			if (resultImage == null && throwIfNotExists)
				throw new Exception(string.Format("Image is missing on the path {0}", path));
#endif
			return resultImage;
		}

		#endregion

		#region lines

		/// <summary>
		/// рисует линию
		/// </summary>
		public static UIView DrawHorizontalLine(nfloat x, nfloat y, nfloat width, nfloat height, UIColor color)
		{
			UIView view = new UIView(new CGRect(x, y, width, height));
			view.BackgroundColor = color;
			return view;
		}

		/// <summary>
		/// рисует линию под представлением
		/// </summary>
		public static void DrawHorizontalLineAtBottom(this UIView source, float height, UIColor color)
		{
			UIView view = new UIView(new CGRect(0, source.Frame.Height - height, source.Frame.Width, height));
			view.BackgroundColor = color;
			source.AddSubview(view);
		}

		#endregion

		#region actionOnTap

		/// <summary>
		/// Выполняет заданное действие при событии Tap для view, а также возвращает точку касания
		/// </summary>
		/// <param name="action">Action</param>
		public static void ActionOnTap(this UIView view, Action<CGPoint> action)
		{
			view.UserInteractionEnabled = true;
			var tapRecogniser = new UITapGestureRecognizer((x) =>
				{
					action((CGPoint)x.LocationInView(view));
				});
			view.AddGestureRecognizer(tapRecogniser);
		}

		/// <summary>
		/// Выполняет заданный Action при событии Tap для view
		/// </summary>
		/// <param name="action">Action</param>
		public static void ActionOnTap(this UIView view, Action action)
		{
			view.UserInteractionEnabled = true;
			var tapRecogniser = new UITapGestureRecognizer(action);
			view.AddGestureRecognizer(tapRecogniser);
		}

		#endregion
	}
}

