using Android.App;
using Android.Widget;
using Android.OS;
using Xamarin.Controls;
using System.IO;
using System.Drawing;
using Android.Runtime;
using Android.Graphics;
using Android.Content;
using Android.Graphics.Drawables;
using Android.Provider;

namespace CanvasDemo
{
    [Activity(Label = "CanvasDemo", MainLauncher = true)]
    public class MainActivity : Activity
    {
        private System.Drawing.PointF[] points;
        SignaturePadView signatureView;
        Button btnSaveImage;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            signatureView = FindViewById<SignaturePadView>(Resource.Id.signatureView);

            var btnSave = FindViewById<Button>(Resource.Id.btnSave);
            var btnLoad = FindViewById<Button>(Resource.Id.btnLoad);
            var btnSaveImage = FindViewById<Button>(Resource.Id.btnSaveImage);
            btnSaveImage.Click += BtnCamera_Click; ;
            // signatureView.SetBackgroundResource(Resource.Drawable.splash_logo);

            //btnSave.Click += delegate
            //{
            //    points = signatureView.Points;

            //    Toast.MakeText(this, "Vector signature saved to memory.", ToastLength.Short).Show();
            //};
            btnSave.Click += async delegate
            {
                points = signatureView.Points;

                Toast.MakeText(this, "Vector signature saved to memory.", ToastLength.Short).Show();

                var path = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
                var file = System.IO.Path.Combine(path, "signature.png");

                using (var bitmap = await signatureView.GetImageStreamAsync(SignatureImageFormat.Png, Android.Graphics.Color.Black, Android.Graphics.Color.White, 1f))
                using (var dest = File.OpenWrite(file))
                {
                    await bitmap.CopyToAsync(dest);
                }

                Toast.MakeText(this, "Raster signature saved to the photo gallery.", ToastLength.Short).Show();
            };

            btnLoad.Click += delegate
            {
                if (points != null)
                    signatureView.LoadPoints(points);
            };

            //btnSaveImage.Click += async delegate
            //{
            //    var path = Environment.GetExternalStoragePublicDirectory(Environment.DirectoryPictures).AbsolutePath;
            //    var file = System.IO.Path.Combine(path, "signature.png");

            //    using (var bitmap = await signatureView.GetImageStreamAsync(SignatureImageFormat.Png, Color.Black, Color.White, 1f))
            //    using (var dest = File.OpenWrite(file))
            //    {
            //        await bitmap.CopyToAsync(dest);
            //    }

            //    Toast.MakeText(this, "Raster signature saved to the photo gallery.", ToastLength.Short).Show();
            //};
        }
        private void BtnCamera_Click(object sender, System.EventArgs e)
        {
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            StartActivityForResult(intent, 0);
        }
        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            Bitmap bitmap = (Bitmap)data.Extras.Get("data"); ;
            //Convert bitmap to drawable
            Drawable drawable = new BitmapDrawable(Resources, bitmap);
            //signatureView = new SignaturePadView(this);
            //signatureView.StrokeColor = Android.Graphics.Color.Red;
            //signatureView.SetBackgroundDrawable(drawable);

            signatureView.SetBackgroundDrawable(drawable);
        }
    }
}

