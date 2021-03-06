﻿// ---
// Open Source Screenshot Plugin for Xamarin.forms 
// 
// Ville Kokkarinen | @ | kokkarinen.ville@gmail.com
// 
// ---

using Android.App;
using Android.Views;
using Android.Graphics;
using System;
using System.IO;

[assembly: Xamarin.Forms.Dependency(typeof(Screenshot_Plugin.SnapshotService))]
namespace Screenshot_Plugin
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SnapshotService"/> class.
    /// </summary>
    public class SnapshotService : ISnapShotService
    {      
        /// <summary>
        /// Returns bytes of current screen for further processing
        /// </summary>
        /// <returns></returns>
        public byte[] CurrentScreenBytes(View view = null)
        {
            byte[] bitmapData;

            if (view == null)
                view = ((Activity)Xamarin.Forms.Forms.Context).Window.DecorView.RootView;

            
            //Creates a bitmap of the current view
            using (var screenshot = Bitmap.CreateBitmap(
                    view.Width,
                    view.Height,
                    Bitmap.Config.Argb8888))
            {
                //Draws a canvas of the current screen
                var canvas = new Canvas(screenshot);
                view.Draw(canvas);

                //Compresses the screenshot and saves it to the device
                using (var screenshotOutputStream = new MemoryStream())
                {
                    screenshot.Compress(Bitmap.CompressFormat.Png, 90, screenshotOutputStream);
                    bitmapData = screenshotOutputStream.ToArray();
                    return bitmapData;
                }
            }            
        }

        
        /// <summary>
        /// Generate a SnapShotService and call TakeSnapShot to take a snapshot from the current view
        /// <para> Or call directly with: </para>
        /// DependencyService.Get&lt;ISnapShotService>().TakeSnapShot()
        /// </summary>
        public SnapshotService() { }

        /// <summary>
        ///  Method to take a screenshot,
        ///  <para>View = View of which the screenshot will be taken. Null defaults to the root of current xamarin.forms </para>
        ///  <para>Path = Path on the device where to save. Null defaults to root/pictures </para>
        ///  <para>Imagename = Name of the image + extension ".png" </para>
        /// </summary>
        /// <param name="view"></param>
        /// <param name="path"></param>
        /// <param name="Imagename"></param>
        public void SaveScreenShot(View view = null, string path=null, string Imagename=null)
        {
            if (view == null)            
                view = ((Activity)Xamarin.Forms.Forms.Context).Window.DecorView.RootView;               
                            
            
            if (path == null)
                path = Android.OS.Environment.GetExternalStoragePublicDirectory("Pictures").AbsolutePath + Java.IO.File.Separator;                
            

            if (Imagename == null)
                Imagename = "Screenshot.png";

            //Creates a bitmap of the current view
            using (var screenshot = Bitmap.CreateBitmap(
                    view.Width,
                    view.Height,
                    Bitmap.Config.Argb8888))
            {
                //Draws a canvas of the current screen
                var canvas = new Canvas(screenshot);
                view.Draw(canvas);

                //Compresses the screenshot and saves it to the device
                using (var screenshotOutputStream = new System.IO.FileStream(path+Imagename, System.IO.FileMode.Create))
                {
                    screenshot.Compress(Bitmap.CompressFormat.Png, 90, screenshotOutputStream);
                    screenshotOutputStream.Flush();                   
                }
            }                       
           
        }
    }
}