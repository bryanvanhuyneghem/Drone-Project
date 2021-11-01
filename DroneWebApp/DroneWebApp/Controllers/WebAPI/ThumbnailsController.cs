using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Description;
using DroneWebApp.Models;

namespace DroneWebApp.Controllers.WebAPI
{
    public class ThumbnailsController : ApiController
    {
        private DroneDBEntities db = new DroneDBEntities();


        public HttpResponseMessage GetThumbnail(int id, int imageid)
        {
            try
            {
                // Using parameters against sql injections
                RawImage rawImage = db.RawImages.SqlQuery(
                    "SELECT * FROM RawImages WHERE FlightId = @id AND RawImageKey = @imageid;",
                     new SqlParameter("id", id),
                     new SqlParameter("imageid", imageid)
                    ).First<RawImage>();

                // Config to an image
                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                result.Content = new ByteArrayContent(rawImage.RawDataDownsized); //downsized img = thumbnail
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpg");
                return result;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }
        }       
    }
}