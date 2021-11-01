using MetadataExtractor;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;

namespace DroneWebApp.Models.SimpleFactoryPattern.Parsers
{
	public class RawImageParser : IParser
	{
		public bool Parse(string path, int flightId, DroneDBEntities db)
		{
			DroneFlight droneFlight = db.DroneFlights.Find(flightId);

			// Set culture for double conversion 
			CultureInfo customCulture = (CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
			customCulture.NumberFormat.NumberDecimalSeparator = ".";
			System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;

			//culture for date parsing 
			CultureInfo provider = CultureInfo.InvariantCulture;
			string format = "yyyy:MM:dd HH:mm:ss"; //2019:09:12 15:49:47

			Helper.Helper.SetProgress(10);
			RawImage rawImage;
			try
			{
				byte[] rawData; //ingelezen image in raw bytes
				using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Read))
				{
					rawData = new byte[fs.Length];
					fs.Read(rawData, 0, System.Convert.ToInt32(fs.Length));
				}

				//reading metadata
				var directories = ImageMetadataReader.ReadMetadata(path);

				//check if image is already in db 
				string filename = directories[10].Tags[0].Description;
				int count = db.RawImages.SqlQuery(
						   "SELECT * FROM RawImages WHERE FlightId = @id AND FileName = @filename; ",
						   new SqlParameter("id", flightId),
						   new SqlParameter("filename", filename))
						   .Count<RawImage>();

				//the image is already in the db for this flight
				if (count != 0)
				{
					Helper.Helper.SetProgress(100);
					return false;
				}

				Helper.Helper.SetProgress(30);

				//thumbnail
				byte[] rawDataDownsized;
				Bitmap bmp = ResizeImage(Image.FromFile(path), 225, 150);
				ImageConverter converter = new ImageConverter();
				rawDataDownsized = (byte[])converter.ConvertTo(bmp, typeof(byte[]));

				Helper.Helper.SetProgress(60);

				//make RawImage object and set its attributes
				rawImage = new RawImage
				{
					FileName = directories[10].Tags[0].Description, //??

					RawData = rawData,
					RawDataDownsized = rawDataDownsized,


					FlightId = flightId,

					CreateDate = DateTime.ParseExact(directories[1].Tags[8].Description, format, provider),

					//drone (aircraft) 
					SpeedX = Convert.ToDouble(directories[3].Tags[2].Description, customCulture),
					SpeedY = Convert.ToDouble(directories[3].Tags[3].Description, customCulture),
					SpeedZ = Convert.ToDouble(directories[3].Tags[4].Description, customCulture),
					Pitch = Convert.ToDouble(directories[3].Tags[5].Description, customCulture),
					Yaw = Convert.ToDouble(directories[3].Tags[6].Description, customCulture),
					Roll = Convert.ToDouble(directories[3].Tags[7].Description, customCulture),

					CameraPitch = Convert.ToDouble(directories[3].Tags[8].Description, customCulture),
					CameraYaw = Convert.ToDouble(directories[3].Tags[9].Description, customCulture),
					CameraRoll = Convert.ToDouble(directories[3].Tags[10].Description, customCulture),

					GpsAltitude = directories[5].Tags[6].Description,
					GpsLatitude = directories[5].Tags[2].Description,
					GpsLongitude = directories[5].Tags[4].Description,


					//new fields 
					ExposureTime = directories[2].Tags[0].Description,
					ShutterSpeedValue = directories[2].Tags[9].Description,
					ApertureValue = directories[2].Tags[10].Description,
					MaxApertureValue = directories[2].Tags[12].Description,


					//cannot read these fields 
					ExposureCompensation = null,
					FNumber = null,
					Iso = null,
					AbsoluteAltitude = null,
					RelativeAltitude = null,
					GpsPosition = null,
					RtkFlag = null,

					GPSLatRef = directories[5].Tags[1].Description,
					GPSLongRef = directories[5].Tags[3].Description,
					GPSAltitudeRef = directories[5].Tags[5].Description
				};
			}
			catch (Exception)
			{
				Helper.Helper.SetProgress(100);
				return false;
			}
			
			Helper.Helper.SetProgress(90);

			//Add rawImage Object to DB and save changes
			db.RawImages.Add(rawImage);
			droneFlight.hasRawImages = true;

			db.SaveChanges();

			Helper.Helper.SetProgress(100);

			#region console prints for debugging
			//int directory_i = 0;
			//foreach (var directory in directories)
			//{
			//	int tag_i = 0;
			//	foreach (var tag in directory.Tags)
			//	{

			//		System.Diagnostics.Debug.WriteLine($" directory {directory_i}: {directory.Name}   -   tag {tag_i}: {tag.Name} = {tag.Description}");
			//		tag_i++;
			//	}
			//	directory_i++;
			//}

			//Debug.WriteLine(directories[0]); //JPEG Directory (8 tags)
			//Debug.WriteLine(directories[1]); //Exif IFD0 Directory (12 tags)
			//Debug.WriteLine(directories[2]); //Exif SubIFD Directory (37 tags)
			//Debug.WriteLine(directories[3]); //DJI Makernote Directory (35 tags)
			//Debug.WriteLine(directories[4]); //Interoperability Directory (2 tags)
			//Debug.WriteLine(directories[5]); //GPS Directory (7 tags)
			//Debug.WriteLine(directories[6]); //Exif Thumbnail Directory (6 tags)
			//Debug.WriteLine(directories[7]); //XMP Directory (1 tag)
			//Debug.WriteLine(directories[8]); //Huffman Directory (1 tag)
			//Debug.WriteLine(directories[9]); //File Type Directory (4 tags)
			//Debug.WriteLine(directories[10]); //File Directory (3 tags)
			//Debug.WriteLine("");

			//Debug.WriteLine("Image Height " + directories[0].Tags[2].Description);
			//Debug.WriteLine("Image Width " + directories[0].Tags[3].Description);

			//Debug.WriteLine("File Name: " + directories[10].Tags[0].Description);
			//Debug.WriteLine("File Size: " + directories[10].Tags[1].Description);

			//Debug.WriteLine("File Type Extension: " + directories[9].Tags[3].Description);

			//Debug.WriteLine("GPS Version ID: " + directories[5].Tags[0].Description);
			//Debug.WriteLine("GPS Latitude Ref: " + directories[5].Tags[1].Description);
			//Debug.WriteLine("GPS Latitude: " + directories[5].Tags[2].Description);
			//Debug.WriteLine("GPS Longitude Ref: " + directories[5].Tags[3].Description);
			//Debug.WriteLine("GPS Longitude: " + directories[5].Tags[4].Description);
			//Debug.WriteLine("GPS Altitude Ref: " + directories[5].Tags[5].Description);
			//Debug.WriteLine("GPS Altitude: " + directories[5].Tags[6].Description);

			//Debug.WriteLine("Make: " + directories[3].Tags[0].Description);

			//Debug.WriteLine("X Speed: " + directories[3].Tags[2].Description);
			//Debug.WriteLine("Y Speed: " + directories[3].Tags[3].Description);
			//Debug.WriteLine("Z Speed: " + directories[3].Tags[4].Description);

			//Debug.WriteLine("Aircraft Pitch: " + directories[3].Tags[5].Description);
			//Debug.WriteLine("Aircraft Yaw: " + directories[3].Tags[6].Description);
			//Debug.WriteLine("Aircraft Roll: " + directories[3].Tags[7].Description);

			//Debug.WriteLine("Camera Pitch: " + directories[3].Tags[8].Description);
			//Debug.WriteLine("Camera Yaw: " + directories[3].Tags[9].Description);
			//Debug.WriteLine("Camera Roll: " + directories[3].Tags[10].Description);




			#endregion

			return true;
		}

		private static Bitmap ResizeImage(Image image, int width, int height)
		{
			var destRect = new Rectangle(0, 0, width, height);
			var destImage = new Bitmap(width, height);

			destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

			using (var graphics = Graphics.FromImage(destImage))
			{
				graphics.CompositingMode = CompositingMode.SourceCopy;
				graphics.CompositingQuality = CompositingQuality.HighQuality;
				graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
				graphics.SmoothingMode = SmoothingMode.HighQuality;
				graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

				using (var wrapMode = new ImageAttributes())
				{
					wrapMode.SetWrapMode(WrapMode.TileFlipXY);
					graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
				}
			}

			return destImage;
		}
	}
}