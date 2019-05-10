using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using SkiaSharp;
using TouchTracking;
using SkiaSharp.Views.Forms;

namespace WhiteBoardDrawing
{
    public class PaintPath
    {
        public SKPath Path { get; set; }
        public SKPaint Paint { get; set; }
    }

    [DesignTimeVisible(true)]
    public partial class MainPage : ContentPage
    {
        Dictionary<long, SKPath> inProgressPaths = new Dictionary<long, SKPath>();
        List<PaintPath> completedPaths = new List<PaintPath>();

        SKPaint paint = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            Color = SKColors.Black,
            StrokeWidth = 10,
            StrokeCap = SKStrokeCap.Round,
            StrokeJoin = SKStrokeJoin.Round
        };


        public MainPage()
        {
            InitializeComponent();
        }

        void OnTouchEffectAction(object sender, TouchActionEventArgs args)
        {
            switch (args.Type)
            {
                case TouchActionType.Pressed:
                    if (!inProgressPaths.ContainsKey(args.Id))
                    {
                        SKPath path = new SKPath();
                        path.MoveTo(ConvertToPixel(args.Location));
                        inProgressPaths.Add(args.Id, path);
                        canvasView.InvalidateSurface();
                    }
                    break;

                case TouchActionType.Moved:
                    if (inProgressPaths.ContainsKey(args.Id))
                    {
                        SKPath path = inProgressPaths[args.Id];
                        path.LineTo(ConvertToPixel(args.Location));
                        canvasView.InvalidateSurface();
                    }
                    break;

                case TouchActionType.Released:
                    if (inProgressPaths.ContainsKey(args.Id))
                    {
                        completedPaths.Add(new PaintPath() { Path = inProgressPaths[args.Id], Paint = paint.Clone() });
                        inProgressPaths.Remove(args.Id);
                        canvasView.InvalidateSurface();
                    }
                    break;

                case TouchActionType.Cancelled:
                    if (inProgressPaths.ContainsKey(args.Id))
                    {
                        inProgressPaths.Remove(args.Id);
                        canvasView.InvalidateSurface();
                    }
                    break;
            }
        }

        void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            SKCanvas canvas = args.Surface.Canvas;
            canvas.Clear();

            foreach (var  path in completedPaths)
            {
                canvas.DrawPath(path.Path, path.Paint);
            }

            foreach (SKPath path in inProgressPaths.Values)
            {
                canvas.DrawPath(path, paint);
            }
        }

        SKPoint ConvertToPixel(TouchTrackingPoint pt)
        {
            return new SKPoint((float)(canvasView.CanvasSize.Width * pt.X / canvasView.Width),
                               (float)(canvasView.CanvasSize.Height * pt.Y / canvasView.Height));
        }

        private void ClearButton_Pressed(object sender, EventArgs e)
        {
            completedPaths = new List<PaintPath>();
            inProgressPaths = new Dictionary<long, SKPath>();
            canvasView.InvalidateSurface();
        }

        private void ColorButton_Pressed(object sender, EventArgs e)
        {

            var newPaint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                StrokeWidth = 10,
                StrokeCap = SKStrokeCap.Round,
                StrokeJoin = SKStrokeJoin.Round
            };


            if (sender == RedButton)
            {
                newPaint.Color = SKColors.Red;
                paint = newPaint;
            }
            if (sender == BlueButton)
            {
                newPaint.Color = SKColors.RoyalBlue;
                paint = newPaint;
            }
            if (sender == OrangeButton)
            {
                newPaint.Color = SKColors.DarkOrange;
                paint = newPaint;
            }
            if (sender == GreenButton)
            {
                newPaint.Color = SKColors.Green;
                paint = newPaint;
            }
            if (sender == PinkButton)
            {
                newPaint.Color = SKColors.HotPink;
                paint = newPaint;
            }
            if (sender == PurpleButton)
            {
                newPaint.Color = SKColors.Purple;
                paint = newPaint;
            }
            if (sender == BlackButton)
            {
                newPaint.Color = SKColors.Black;
                paint = newPaint;
            }



        }
    }
}
