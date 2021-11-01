using iText.IO.Font;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace DroneWebApp.Models.DataExport
{
    public class ExportPDF : AExport
    {
        public override void CreateDroneLog(int droneId, DroneDBEntities db, HttpContextBase context)
        {
            // get drone
            Drone drone = db.Drones.Find(droneId);
            // get drone flights
            List<DroneFlight> flights = db.DroneFlights.Where(df => df.DroneId == droneId).ToList();

            // list of table headers
            List<string> keys = new List<string>
            {
                "Registration",
                "DroneType",
                "DroneName",
                "Date",
                "PilotName",
                "DepartureLongitude",
                "DepartureLatitude",
                "DepartureUTCTime",
                "DestinationLongitude",
                "DestinationLatitude",
                "DestinationUTCTime",
                "TypeOfActivity",
                "FlightTime",
                "Other",
                "Simulator",
                "Instructor",
                "Remarks"
            };

            context.Response.Clear();

            // create new pfd document
            PdfWriter writer = new PdfWriter(context.Response.OutputStream);
            PdfDocument pdfDocument = new PdfDocument(writer);
            pdfDocument.SetTagged();
            Document document = new Document(pdfDocument);
            document.SetMargins(20, 20, 20, 20);

            // Add front page with drone info
            Paragraph title = new Paragraph(new Text("Remotely Piloted Aircraft Logbook").SetFontSize(26).SetBold());
            title.SetTextAlignment(TextAlignment.CENTER);
            title.SetMarginBottom(20);
            title.SetMarginTop(20);
            document.Add(title);

            Paragraph title2 = new Paragraph(new Text("Drone Info").SetFontSize(26).SetUnderline());
            title2.SetMarginLeft(20);
            title2.SetMarginBottom(20);
            document.Add(title2);

            Paragraph droneName = new Paragraph(new Text("Drone Name: " + drone.DroneName).SetFontSize(20));
            droneName.SetMarginLeft(20);
            document.Add(droneName);

            Paragraph droneType = new Paragraph(new Text("Drone Type: " + drone.DroneType).SetFontSize(20));
            droneType.SetMarginLeft(20);
            document.Add(droneType);

            Paragraph droneReg = new Paragraph(new Text("Drone Registration: " + drone.Registration).SetFontSize(20));
            droneReg.SetMarginLeft(20);
            document.Add(droneReg);

            if (flights.Count != 0)
            {
                foreach (DroneFlight flight in flights)
                {
                    // table with data
                    Table table = new Table(new float[] { 1, 2 });
                    table.SetWidth(UnitValue.CreatePercentValue(100));

                    if (flight.DepartureInfo == null)
                    {
                        flight.DepartureInfo = new DepartureInfo
                        {
                            Longitude = null,
                            Latitude = null,
                            UTCTime = null
                        };
                    }
                    if (flight.DestinationInfo == null)
                    {
                        flight.DestinationInfo = new DestinationInfo
                        {
                            Longitude = null,
                            Latitude = null,
                            UTCTime = null
                        };
                    }

                    // list of table values
                    List<string> values = new List<string>
                    {
                        flight.Drone.Registration ?? "",
                        flight.Drone.DroneType ?? "",
                        flight.Drone.DroneName ?? "",
                        flight.Date.ToString() ?? "",
                        flight.Pilot.PilotName ?? "",
                        flight.DepartureInfo.Longitude.ToString() ?? "",
                        flight.DepartureInfo.Latitude.ToString() ?? "",
                        flight.DepartureInfo.UTCTime.ToString() ?? "",
                        flight.DestinationInfo.Longitude.ToString() ?? "",
                        flight.DestinationInfo.Latitude.ToString() ?? "",
                        flight.DestinationInfo.UTCTime.ToString() ?? "",
                        flight.TypeOfActivity ?? "",
                        GetFlightTime(flight).ToString() ?? "",
                        flight.Other ?? "",
                        flight.Simulator ?? "",
                        flight.Instructor ?? "",
                        flight.Remarks ?? ""
                    };

                    // go to next page for each drone flight
                    document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));

                    Text text = new Text("Drone Flight with ID = " + flight.FlightId);
                    text.SetFontSize(20);
                    Paragraph flightInfo = new Paragraph(text);
                    document.Add(flightInfo);

                    // add data to table
                    for (int i = 0; i < keys.Count; i++)
                    {
                        table.AddCell(new Cell().Add(new Paragraph(new Text(keys[i]).SetBold())));
                        table.AddCell(new Cell().Add(new Paragraph(new Text(values[i]))));
                    }

                    // add table to pdf document
                    document.Add(table);
                }
            }

            document.Close();

            string filename = drone.DroneName;
            filename += droneId;
            filename = ReplaceInvalidChars(filename);

            // create downloadable document
            context.Response.ContentType = "application/pdf";
            context.Response.AddHeader("Content-Disposition", "attachment;filename=" + filename + ".pdf");
            context.Response.Write(document);
            context.Response.End();
        }

        public override void CreatePilotLog(int pilotId, DroneDBEntities db, HttpContextBase context)
        {
            // get pilot
            Pilot pilot = db.Pilots.Find(pilotId);
            // get drone flights
            List<DroneFlight> flights = db.DroneFlights.Where(df => df.PilotId == pilotId).ToList();

            // list of table headers
            List<string> keys = new List<string>
            {
                "PilotName",
                "Street",
                "ZIP",
                "City",
                "Country",
                "Phone",
                "LicenseNo",
                "Email",
                "EmergencyPhone",
                "Date",
                "DepartureLongitude",
                "DepartureLatitude",
                "DepartureUTCTime",
                "DestinationLongitude",
                "DestinationLatitude",
                "DestinationUTCTime",
                "Registration",
                "DroneType",
                "FlightTime",
                "Other",
                "Simulator",
                "Instructor",
                "Remarks"
            };

            context.Response.Clear();

            // create new pdf document
            PdfWriter writer = new PdfWriter(context.Response.OutputStream);
            PdfDocument pdfDocument = new PdfDocument(writer);
            pdfDocument.SetTagged();
            Document document = new Document(pdfDocument);
            document.SetMargins(20, 20, 20, 20);

            // Add front page with pilot info
            Paragraph title = new Paragraph(new Text("Remotely Piloted Aircraft Systems Logbook").SetFontSize(26).SetBold());
            title.SetTextAlignment(TextAlignment.CENTER);
            title.SetMarginBottom(20);
            title.SetMarginTop(20);
            document.Add(title);

            Paragraph title2 = new Paragraph(new Text("Pilot Info").SetFontSize(26).SetUnderline());
            title2.SetMarginLeft(20);
            title2.SetMarginBottom(20);
            document.Add(title2);

            Paragraph pilotName = new Paragraph(new Text("Name: " + pilot.PilotName).SetFontSize(20));
            pilotName.SetMarginLeft(20);
            document.Add(pilotName);

            Paragraph pilotStreet = new Paragraph(new Text("Street: " + pilot.Street).SetFontSize(20));
            pilotStreet.SetMarginLeft(20);
            document.Add(pilotStreet);

            Paragraph pilotZIP = new Paragraph(new Text("Postal Code: " + pilot.ZIP).SetFontSize(20));
            pilotZIP.SetMarginLeft(20);
            document.Add(pilotZIP);

            Paragraph pilotCity = new Paragraph(new Text("City: " + pilot.City).SetFontSize(20));
            pilotCity.SetMarginLeft(20);
            document.Add(pilotCity);

            Paragraph pilotCountry = new Paragraph(new Text("Country: " + pilot.Country).SetFontSize(20));
            pilotCountry.SetMarginLeft(20);
            document.Add(pilotCountry);

            Paragraph pilotPhone = new Paragraph(new Text("Tel: " + pilot.Phone).SetFontSize(20));
            pilotPhone.SetMarginLeft(20);
            document.Add(pilotPhone);

            Paragraph pilotLicNo = new Paragraph(new Text("License N°: " + pilot.LicenseNo).SetFontSize(20));
            pilotLicNo.SetMarginLeft(20);
            document.Add(pilotLicNo);

            Paragraph pilotMail = new Paragraph(new Text("Email: " + pilot.Email).SetFontSize(20));
            pilotMail.SetMarginLeft(20);
            document.Add(pilotMail);

            Paragraph pilotEmPhone = new Paragraph(new Text("Tel Emergency: " + pilot.EmergencyPhone).SetFontSize(20));
            pilotEmPhone.SetMarginLeft(20);
            document.Add(pilotEmPhone);

            if (flights.Count != 0)
            {
                foreach (DroneFlight flight in flights)
                {
                    // table for flight info
                    Table table = new Table(new float[] { 1, 2 });
                    table.SetWidth(UnitValue.CreatePercentValue(100));

                    if (flight.DepartureInfo == null)
                    {
                        flight.DepartureInfo = new DepartureInfo
                        {
                            Longitude = null,
                            Latitude = null,
                            UTCTime = null
                        };
                    }
                    if (flight.DestinationInfo == null)
                    {
                        flight.DestinationInfo = new DestinationInfo
                        {
                            Longitude = null,
                            Latitude = null,
                            UTCTime = null
                        };
                    }

                    // list with table data
                    List<string> values = new List<string>
                    {
                        flight.Pilot.PilotName ?? "",
                        flight.Pilot.Street ?? "",
                        flight.Pilot.ZIP.ToString() ?? "",
                        flight.Pilot.City ?? "",
                        flight.Pilot.Country ?? "",
                        flight.Pilot.Phone ?? "",
                        flight.Pilot.LicenseNo.ToString() ?? "",
                        flight.Pilot.Email ?? "",
                        flight.Pilot.EmergencyPhone ?? "",
                        flight.Date.ToString() ?? "",
                        flight.DepartureInfo.Longitude.ToString() ?? "",
                        flight.DepartureInfo.Latitude.ToString() ?? "",
                        flight.DepartureInfo.UTCTime.ToString() ?? "",
                        flight.DestinationInfo.Longitude.ToString() ?? "",
                        flight.DestinationInfo.Latitude.ToString() ?? "",
                        flight.DestinationInfo.UTCTime.ToString() ?? "",
                        flight.Drone.Registration ?? "",
                        flight.Drone.DroneType ?? "",
                        GetFlightTime(flight).ToString() ?? "",
                        flight.Other ?? "",
                        flight.Simulator ?? "",
                        flight.Instructor ?? "",
                        flight.Remarks ?? ""
                    };

                    // go to next page for each drone flight
                    document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));

                    Text text = new Text("Drone Flight with ID = " + flight.FlightId);
                    text.SetFontSize(20);
                    Paragraph flightInfo = new Paragraph(text);
                    document.Add(flightInfo);

                    // add data to table
                    for (int i = 0; i < keys.Count; i++)
                    {
                        table.AddCell(new Cell().Add(new Paragraph(new Text(keys[i]).SetBold())));
                        table.AddCell(new Cell().Add(new Paragraph(new Text(values[i]))));
                    }

                    // add table to pdf document
                    document.Add(table);
                }
            }

            document.Close();

            string filename = pilot.PilotName;
            filename += pilotId;
            filename = ReplaceInvalidChars(filename);

            // create downloadable document
            context.Response.ContentType = "application/pdf";
            context.Response.AddHeader("Content-Disposition", "attachment;filename=" + filename + ".pdf");
            context.Response.Write(document);
            context.Response.End();
        }
    }
}