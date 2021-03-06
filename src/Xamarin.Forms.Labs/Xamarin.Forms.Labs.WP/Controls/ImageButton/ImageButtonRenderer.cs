﻿using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Xamarin.Forms.Platform.WinPhone;
using Xamarin.Forms.Labs.Controls;
using Xamarin.Forms.Labs.Enums;
using Xamarin.Forms.Labs.WP8.Controls.ImageButton;

[assembly: Xamarin.Forms.ExportRenderer(typeof(ImageButton), typeof(ImageButtonRenderer))]
namespace Xamarin.Forms.Labs.WP8.Controls.ImageButton
{
    /// <summary>
    /// Draws a button on the Windows Phone platform with the image shown in the right 
    /// position with the right size.
    /// </summary>
    public class ImageButtonRenderer : ButtonRenderer
    {
        /// <summary>
        /// The image displayed in the button.
        /// </summary>
        private System.Windows.Controls.Image currentImage;

        /// <summary>
        /// Handles the initial drawing of the button.
        /// </summary>
        /// <param name="e">Information on the <see cref="ImageButton"/>.</param> 
        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);

            var sourceButton = this.Element as Labs.Controls.ImageButton;
            var targetButton = this.Control;
            if (sourceButton != null && targetButton != null && !string.IsNullOrEmpty(sourceButton.Image))
            {
                var stackPanel = new StackPanel
                    {
                        Orientation = (sourceButton.Orientation == ImageOrientation.ImageOnTop ||
                                       sourceButton.Orientation == ImageOrientation.ImageOnBottom)
                                          ? Orientation.Vertical
                                          : Orientation.Horizontal
                    };

                this.currentImage = GetImage(sourceButton.Image, sourceButton.ImageHeightRequest, sourceButton.ImageWidthRequest);
                SetImageMargin(this.currentImage, sourceButton.Orientation);

                var label = new TextBlock
                    {
                        TextAlignment = GetTextAlignment(sourceButton.Orientation),
                        FontSize = 16,
                        VerticalAlignment = VerticalAlignment.Center,
                        Text = sourceButton.Text
                    };

                if (sourceButton.Orientation == ImageOrientation.ImageToLeft)
                {
                    targetButton.HorizontalContentAlignment = HorizontalAlignment.Left;
                }
                else if (sourceButton.Orientation == ImageOrientation.ImageToRight)
                {
                    targetButton.HorizontalContentAlignment = HorizontalAlignment.Right;
                }
                
                if (sourceButton.Orientation == ImageOrientation.ImageOnTop ||
                    sourceButton.Orientation == ImageOrientation.ImageToLeft)
                {
                    this.currentImage.HorizontalAlignment = HorizontalAlignment.Left;
                    stackPanel.Children.Add(this.currentImage);
                    stackPanel.Children.Add(label);
                }
                else
                {
                    stackPanel.Children.Add(label);
                    stackPanel.Children.Add(this.currentImage);
                }

                targetButton.Content = stackPanel;
            }
        }

        /// <summary>
        /// Called when the underlying model's properties are changed.
        /// </summary>
        /// <param name="sender">Model sending the change event.</param>
        /// <param name="e">Event arguments.</param>
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == Labs.Controls.ImageButton.ImageProperty.PropertyName)
            {
                var sourceButton = this.Element as Labs.Controls.ImageButton;
                if (sourceButton != null && !string.IsNullOrEmpty(sourceButton.Image))
                {
                    this.currentImage = GetImage(sourceButton.Image, sourceButton.ImageHeightRequest, sourceButton.ImageWidthRequest);
                    SetImageMargin(this.currentImage, sourceButton.Orientation);
                }
            }
        }

        /// <summary>
        /// Returns the alignment of the label on the button depending on the orientation.
        /// </summary>
        /// <param name="imageOrientation">The orientation to use.</param>
        /// <returns>The alignment to use for the text.</returns>
        private static System.Windows.TextAlignment GetTextAlignment(ImageOrientation imageOrientation)
        {
            System.Windows.TextAlignment returnValue;
            switch (imageOrientation)
            {
                case ImageOrientation.ImageToLeft:
                    returnValue = System.Windows.TextAlignment.Left;
                    break;
                case ImageOrientation.ImageToRight:
                    returnValue = System.Windows.TextAlignment.Right;
                    break;
                default:
                    returnValue = System.Windows.TextAlignment.Center;
                    break;
            }

            return returnValue;
        }

        /// <summary>
        /// Returns a <see cref="Image"/> of type .png that is a resource in the Windows Phone project
        /// and stored in the images folder.
        /// </summary>
        /// <param name="imageName">The name of the image to return.  Should be the resource name without the .png extension.</param>
        /// <param name="height">The height for the image (divides by 2 for the Windows Phone platform).</param>
        /// <param name="width">The width for the image (divides by 2 for the Windows Phone platform).</param>
        /// <returns>A properly sized image.</returns>
        private static System.Windows.Controls.Image GetImage(string imageName, int height, int width)
        {
            var image = new System.Windows.Controls.Image();
            var uri = new Uri("images/" + imageName, UriKind.Relative);
            var bmp = new BitmapImage(uri);
            image.Source = bmp;
            image.Height = Convert.ToDouble(height / 2);
            image.Width = Convert.ToDouble(width / 2);
            return image;
        }

        /// <summary>
        /// Sets a margin of 10 between the image and the text.
        /// </summary>
        /// <param name="image">The image to add a margin to.</param>
        /// <param name="orientation">The orientation of the image on the button.</param>
        private static void SetImageMargin(System.Windows.Controls.Image image, ImageOrientation orientation)
        {
            const int DefaultMargin = 10;
            int left = 0;
            int top = 0;
            int right = 0;
            int bottom = 0;

            switch (orientation)
            {
                case ImageOrientation.ImageToLeft:
                    right = DefaultMargin;
                    break;
                case ImageOrientation.ImageOnTop:
                    bottom = DefaultMargin;
                    break;
                case ImageOrientation.ImageToRight:
                    left = DefaultMargin;
                    break;
                case ImageOrientation.ImageOnBottom:
                    top = DefaultMargin;
                    break;
            }

            image.Margin = new System.Windows.Thickness(left, top, right, bottom);
        }
    }
}
