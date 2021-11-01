require([
    "esri/Map",
    "esri/views/MapView",
    "esri/Graphic",
    "esri/geometry/SpatialReference",
    "esri/widgets/LayerList",
    "esri/widgets/Search",
    "esri/widgets/Legend",
    "esri/PopupTemplate",
    "esri/layers/FeatureLayer",
    "esri/widgets/Feature"
], function (Map, MapView, Graphic, SpatialReference, LayerList, Search, Legend, PopupTemplate, FeatureLayer, Feature) {

    let trackFeatureLayer; // needs to be declared here so we can switch between visual variables

    //#region Basic setup: Map and View

    // Create the map
    let map = new Map({
        basemap: "topo-vector"
    });

    // Create the View (MapView)
    let view = new MapView({
        container: "droneFlightMap",
        map: map,
        center: [4.3, 50.9],
        zoom: 9
    });

    //#endregion

    //#region LAYERLIST
    let layerList = new LayerList({
        view: view
    });

    // Adds widget below other elements in the top right corner of the view
    view.ui.add(layerList, "top-right");
    //#endregion 

    //#region LEGEND Widget
    let legend = new Legend({
        view: view,
        style: "classic", // other styles include 'card'
        label: {
            title: "DEFAULT TITLE"
        }
    });
    view.ui.add(legend, "top-right");
    //#endregion

    //#region Search widget
    const searchWidget = new Search({
        view: view
    });
    // Adds the search widget below other elements in
    // the top left corner of the view
    view.ui.add(searchWidget, {
        position: "bottom-right",
        //index: 2
    });
    //#endregion

    // #region Coordinate Widget
    // Create a coordinate widget
    let coordsWidget = document.createElement("div");
    coordsWidget.id = "coordsWidget";
    coordsWidget.className = "esri-widget esri-component";
    coordsWidget.style.padding = "7px 15px 5px";

    // Add this widget to the View
    view.ui.add(coordsWidget, "bottom-right");

    // Function to show coordinates
    function showCoordinates(pt) {
        var coords = "Lat/Lon " + pt.latitude.toFixed(3) + " " + pt.longitude.toFixed(3) +
            " | Scale 1:" + Math.round(view.scale * 1) / 1 +
            " | Zoom " + view.zoom;
        coordsWidget.innerHTML = coords;
    }

    view.watch("stationary", function (isStationary) {
        showCoordinates(view.center);
    });

    view.on("pointer-move", function (evt) {
        showCoordinates(view.toMap({ x: evt.x, y: evt.y }));
    });

    // #endregion

    //#region Area Measurement Widget
    // To add the AreaMeasurement2D widget to your map
    //let measurementWidget = new AreaMeasurement2D({
    //    view: view
    //});
    //view.ui.add(measurementWidget, "bottom-left");
    //#endregion

    //#region Visualisation Const Values 
    //ID van de droneflight 
    const id = $("#droneFlightMap").data("id");

    //Lambert spacial reference (needed for all featurelayers except track visualisation)
    const LambertSR = { wkid: 31370 };

    //#endregion 

    //#region XYZ VISUALISATION

    //this is fully implemented but currently not useful for the user, 
    //as there are too many xyz points to have a clear overview of 
    //once pointclouds have been further implemented then this code can be adjusted to be useful 

    //const XYZPopup = {
    //    "title": "XYZ Point with ID: {XYZId}",
    //    "content": [{
    //        "type": "fields",
    //        "fieldInfos": [
    //            {
    //                "fieldName": "x",
    //                "label": "X",

    //            },
    //            {
    //                "fieldName": "y",
    //                "label": "Y",

    //            },
    //            {
    //                "fieldName": "z",
    //                "label": "Z",

    //            },
    //            {
    //                "fieldName": "RedValue",
    //                "label": "Red",

    //            },
    //            {
    //                "fieldName": "GreenValue",
    //                "label": "Green",

    //            },
    //            {
    //                "fieldName": "BlueValue",
    //                "label": "Blue",

    //            }
    //        ]
    //    }]
    //};

    //let readXYZ = (XYZPoint) => {
    //    let pointGraphic = {             //type graphic (autocasts)
    //        geometry: {
    //            type: "point",
    //            x: XYZPoint.X,
    //            y: XYZPoint.Y,
    //            z: XYZPoint.Z,
    //            spatialReference: LambertSR   //needs to be defined here
    //        },
    //        attributes: {               //additional attributes (battery etc) will go here !!!
    //            XYZId: XYZPoint.PointCloudXYZId,
    //            x: XYZPoint.X,
    //            y: XYZPoint.Y,
    //            z: XYZPoint.Z,
    //            RedValue: XYZPoint.Red,
    //            GreenValue: XYZPoint.Green,
    //            BlueValue: XYZPoint.Blue,
    //            Intensity: XYZPoint.Intensity
    //        }
    //    };
    //    return pointGraphic;
    //}

    //$.ajax({
    //    type: "GET",
    //    url: "/WebAPI/api/PointCloudXYZs/" + id, // the URL of the controller action method
    //    //data: null, // optional data
    //    success: (result) => {

    //        let XYZPoints = [];
    //        for (let i = 0; i < result.length; i++) {
    //            XYZPoints.push(readXYZ(result[i]));
    //        }

    //        let XYZFeatureLayer = new FeatureLayer({
    //            title: "XYZ Points",
    //            source: XYZPoints,

    //            fields: [{                //repeat the fields for visual variables here
    //                name: "x",
    //                type: "double"
    //            },
    //            {
    //                name: "y",
    //                type: "double"
    //            },
    //            {
    //                name: "z",
    //                type: "double"
    //            },
    //            {
    //                name: "RedValue",
    //                type: "double"
    //            },
    //            {
    //                name: "GreenValue",
    //                type: "double"
    //            },
    //            {
    //                name: "BlueValue",
    //                type: "double"
    //            },
    //            {
    //                name: "Intensity",
    //                type: "double"
    //            }],

    //            objectIdField: "XYZId",                             //needed to uniquely identify each object

    //            //renderer defines how everything will be visualised inside this layer
    //            renderer: {
    //                type: "simple",  // autocasts as new SimpleRenderer()
    //                symbol: {
    //                    type: "simple-marker",
    //                    color: ["RedValue", "GreenValue", "BlueValue"]
    //                    //outline: {
    //                    //    color: [255, 255, 255], // white
    //                    //    width: 1
    //                    //}
    //                }
    //            },
    //            popupTemplate: XYZPopup
    //        });

    //        map.add(XYZFeatureLayer);
    //    },
    //    error: (req, status, error) => {
    //        console.log("AJAX FAIL: XYZ");
    //        console.log(req + status + error);
    //    }
    //});
    //#endregion 

    //#region CTRLPoints VISUALISATION 

    const CTRLTemplate = {
        "title": "Control Point with ID: {CTRLId}",
        "content": [{
            "type": "fields",
            "fieldInfos": [
                {
                    "fieldName": "x",
                    "label": "X",

                },
                {
                    "fieldName": "y",
                    "label": "Y",

                },
                {
                    "fieldName": "z",
                    "label": "Z",

                },
                {
                    "fieldName": "Inside",
                    "label": "Inside pointcloud"
                }
            ]
        }]
    };

    let readCTRL = (CTRLPoint) => {
        let pointGraphic = {             //type graphic (autocasts)
            geometry: {
                type: "point",
                x: CTRLPoint.X,
                y: CTRLPoint.Y,
                z: CTRLPoint.Z,
                spatialReference: LambertSR   //needs to be defined here
            },
            attributes: {               //additional attributes (battery etc) will go here !!!
                CTRLId: CTRLPoint.CTRLId,
                x: CTRLPoint.X,
                y: CTRLPoint.Y,
                z: CTRLPoint.Z,
                inside: CTRLPoint.Inside
            }
        };
        return pointGraphic;
    }

    $.ajax({
        type: "GET",
        url: "/WebAPI/api/CTRLPoints/" + id, // the URL of the controller action method
        //data: null, // optional data
        success: (result) => {

            let CTRLPoints = [];
            for (let i = 0; i < result.length; i++) {
                CTRLPoints.push(readCTRL(result[i]));
            }

            let CTRLfeatureLayer = new FeatureLayer({
                title: "CTRL Points",
                source: CTRLPoints,                                   //THIS needs to be set        (autocast as a Collection of new Graphic())
                geometryType: "point",                              //normaal niet nodig (kan hij afleiden uit features)
                fields: [{                //repeat the fields for visual variables here
                    name: "x",
                    type: "double"
                },
                {
                    name: "y",
                    type: "double"
                },
                {
                    name: "z",
                    type: "double"
                },
                {
                    name: "inside",
                    type: "string"
                }],
                objectIdField: "CTRLId",                             //needed to uniquely identify each object

                renderer: {
                    type: "simple",  // autocasts as new SimpleRenderer()
                    symbol: {
                        type: "simple-marker",
                        color: [26, 102, 255], // light blue
                        outline: {
                            color: [255, 255, 255], // white
                            width: 1
                        }
                    }
                },
                popupTemplate: CTRLTemplate
            });

            map.add(CTRLfeatureLayer);
        },
        error: (req, status, error) => {
            console.log("AJAX FAIL: CTRL POINTS");
            console.log(req + status + error);
        }
    });
    //#endregion

    //#region GCP VISUALISATION 

    const GCPTemplate = {
        "title": "Ground Control Point with ID: {GCPId}",
        "content": [{
            "type": "fields",
            "fieldInfos": [
                {
                    "fieldName": "x",
                    "label": "X",

                },
                {
                    "fieldName": "y",
                    "label": "Y",

                },
                {
                    "fieldName": "z",
                    "label": "Z",

                }
            ]
        }]
    };

    let readGCP = (gcp) => {
        let pointGraphic = {             //type graphic (autocasts)
            geometry: {
                type: "point",
                x: gcp.X,
                y: gcp.Y,
                z: gcp.Z,
                spatialReference: LambertSR   //needs to be defined here??
            },
            attributes: {               //additional attributes (battery etc) will go here 
                GCPId: gcp.GCPId,
                x: gcp.X,
                y: gcp.Y,
                z: gcp.Z
            }
        };
        return pointGraphic;
    }

    $.ajax({
        type: "GET",
        url: "/WebAPI/api/GCP/" + id, // the URL of the controller action method
        data: null, // optional data
        success: (result) => {
            let GCPs = [];
            for (let i = 0; i < result.length; i++) {
                GCPs.push(readGCP(result[i]));
            }

            let GCPFeatureLayer = new FeatureLayer({
                title: "Ground Control Points",
                source: GCPs,                                   //THIS needs to be set        (autocast as a Collection of new Graphic())
                geometryType: "point",                              //normaal niet nodig (kan hij afleiden uit features) 
                fields: [{                //repeat the fields for visual variables here
                    name: "x",
                    type: "double"
                },
                {
                    name: "y",
                    type: "double"
                },
                {
                    name: "z",
                    type: "double"
                }],
                objectIdField: "GCPId",                             //needed to uniquely identify each object

                renderer: {
                    type: "simple",  // autocasts as new SimpleRenderer()
                    symbol: {
                        type: "simple-marker",
                        color: [0, 102, 0], // dark green
                        outline: {
                            color: [255, 255, 255], // white
                            width: 1
                        }
                    }
                },

                popupTemplate: GCPTemplate

            });

            map.add(GCPFeatureLayer);
        },
        error: (req, status, error) => {
            console.log("AJAX GCP FAIL");
            console.log(req);
            console.log(status);
            console.log(error);
        }
    });
    //#endregion

    //#region TRACK VISUALISATION 

    //#region visual variables, custom Renderer and popup

    //visual variable BatteryPercentage
    let colorVar_BatteryPercentage = {
        type: "color",          //specify that its based on color (not size or rotation etc)
        field: "BatteryPercentage",     //specify which field to use
        stops: [{ value: 0, color: "red" }, { value: 50, color: "yellow" }, { value: 100, color: "green" }]
    };

    //visual variable HeightMSL
    let colorVar_HeightMSL = {
        type: "color",          //specify that its based on color (not size or rotation etc)
        field: "HeightMSL",     //specify which field to use   
        stops: [{ value: 2, color: "#470000" }, { value: 53, color: "#ffefdc" }]
    };

    //visual variable VelComposite
    let colorVar_VelComposite = {
        type: "color",          //specify that its based on color (not size or rotation etc)
        field: "VelComposite",     //specify which field to use  
        stops: [{ value: 0, color: "#ebe6df" }, { value: 6, color: "#afbac4" }, { value: 13, color: "#436480" }]
    };

    //specify visualisation here 
    let customRenderer = {
        type: "simple",                 // autocasts as new SimpleRenderer()
        symbol: {
            type: "simple-marker",
            size: 7,
            outline: {
                color: [0, 0, 0],
                width: 1
            }
        }, // autocasts as new SimpleMarkerSymbol()
        visualVariables: [colorVar_HeightMSL]
    };

    const GPTemplate = {
        "title": "Track Point with ID: {GPSId}",
        "content": [{
            "type": "fields",
            "fieldInfos": [
                {
                    "fieldName": "x",
                    "label": "X (WGS84)",
                },
                {
                    "fieldName": "y",
                    "label": "Y (WGS84)",
                },
                {
                    "fieldName": "HeightMSL",
                    "label": "Height (Mean Sea Level)",
                },
                {
                    "fieldName": "VelComposite",
                    "label": "Velocity (Composite)",
                },
                {
                    "fieldName": "BatteryPercentage",
                    "label": "Battery Percentage",
                }
            ]
        }]
    };

    //#endregion

    //niet-hardgecodeerde maxima voor colorranges
    let maxHeightMSL = 0;
    let maxVelComposite = 0;

    let readTrackPoint = (gp) => {

        //set maxima 
        if (maxHeightMSL < gp.HeightMSL) {
            maxHeightMSL = gp.HeightMSL
        }
        if (maxVelComposite < gp.VelComposite) {
            maxVelComposite = gp.VelComposite;
        }

        let pointGraphic = {             //type graphic (autocasts)
            geometry: {
                type: "point",
                x: gp.Long,
                y: gp.Lat
            },
            attributes: {               //additional attributes (battery etc) will go here !!!
                GPSId: gp.GPSId,

                HeightMSL: gp.HeightMSL,
                BatteryPercentage: gp.BatteryPercentage,
                VelComposite: gp.VelComposite,

                x: gp.Long,
                y: gp.Lat
            }
        };
        return pointGraphic;
    }

    //MAIN TRACK VISUALISATION AJAX
    $.ajax({
        type: "GET",
        url: "/WebAPI/api/DroneGPs/" + id, // the URL of the controller action method
        success: (result) => {

            let trackpoints = [];
            for (let i = 0; i < result.length; i++) {
                trackpoints.push(readTrackPoint(result[i]));
            }

            //#region visual variables with non hardcoded maxima 
            colorVar_HeightMSL.stops[1].value = maxHeightMSL;
            colorVar_VelComposite.stops[1].value = (maxVelComposite / 2);
            colorVar_VelComposite.stops[2].value = maxVelComposite;
            //#endregion 

            trackFeatureLayer = new FeatureLayer({
                title: "Track",
                source: trackpoints,                                   //THIS needs to be set        (autocast as a Collection of new Graphic())
                geometryType: "point",                              //normaal niet nodig (kan hij afleiden uit features)
                spatialReference: SpatialReference.WGS84,           // autocasts to wgs84 if not set 
                fields: [{                                          //repeat the fields for visual variables here
                    name: "HeightMSL",
                    type: "double"
                },
                {                                           
                    name: "BatteryPercentage",
                    type: "double"
                },
                {                                           
                    name: "VelComposite",
                    type: "double"
                },
                {                                           
                    name: "x",
                    type: "double"
                },
                {                                           
                    name: "y",
                    type: "double"
                }],
                objectIdField: "GPSId",                             //needed to uniquely identify each object
                renderer: customRenderer,
                popupTemplate: GPTemplate
            });

            map.add(trackFeatureLayer);

        },
        error: (req, status, error) => {
            console.log("AJAX FAIL: TRACK VISUALISATION");
            console.log(req);
            console.log(status);
            console.log(error);
        }
    });

    //#endregion 

    //#region IMAGES

    let convertDegreesToDouble = (coordsDMS) => {
        let coords = coordsDMS.split(/°|'|"/); //split based on degree,minute,sec chars
        let degreesconverted = parseInt(coords[0]);
        let minutesconverted = parseInt(coords[1]) / 60;
        let secondsconverted = parseFloat(coords[2]) / 3600;
        return degreesconverted + minutesconverted + secondsconverted;
    }

    let readImage = (img) => {
        let URL = "/WebAPI/api/RawImages/" + img.FlightID + "/" + img.ImageID;
        let ThumbnailURL = "/WebAPI/api/Thumbnails/" + img.FlightID + "/" + img.ImageID;

        let x = convertDegreesToDouble(img.GpsLongitude);
        let y = convertDegreesToDouble(img.GpsLatitude);

        //rekening houden met de hemisphere voor lat en long conversion 
        if (img.GPSLatRef == "S") {
            y = y * -1;
        }
        if (img.GPSLongRef == "W") {
            x = x * -1;
        }

        let pointGraphic = {             //type graphic (autocasts)
            geometry: {
                type: "point",
                x: x,
                y: y
            },
            attributes: {
                FileName: img.FileName,
                CreateDate: img.CreateDate,
                ImageID: img.ImageID,
                FlightID: img.FlightID,
                url: URL,
                thumbnailURL: ThumbnailURL
            }
        };
        return pointGraphic;
    }

    //images ajax
    $.ajax({
        type: "GET",
        url: "/WebAPI/api/RawImages/" + id, // the URL of the controller action method
        success: (result) => {

            let images = [];
            for (let i = 0; i < result.length; i++) {
                images.push(readImage(result[i]));
            }
            const ImageLayer = new FeatureLayer({
                title: "Raw Images",
                source: images,  // array of graphics objects
                objectIdField: "ImageID",
                fields: [{
                    name: "ImageID",
                    type: "integer"
                },
                {
                    name: "FlightID",
                    type: "integer"
                },
                {
                    name: "url",
                    type: "string"
                },
                {
                    name: "thumbnailURL",
                    type: "string"
                }
                ],
                popupTemplate: {
                    title: "Raw Image Taken",
                    outFields: ["*"],
                    content: (feature) => {
                        var node = document.createElement("div");
                        node.innerHTML = "<a target='_blank' class='thumbnail-enlarge' rel='noopener noreferrer' href='" + feature.graphic.attributes.url + "'>"
                            + "<img src='" + feature.graphic.attributes.thumbnailURL + "' ></a>";
                        return node;
                    }
                },
                renderer: {  // overrides the layer's default renderer
                    type: "simple",
                    symbol: {
                        type: "text",
                        color: "#7A003C",
                        text: "\ue661",
                        font: {
                            size: 20,
                            family: "CalciteWebCoreIcons"
                        }
                    }
                }
            });

            map.add(ImageLayer);

        },
        error: (req, status, error) => {
            console.log("AJAX FAIL: RAW IMAGES");
            console.log(req);
            console.log(status);
            console.log(error);
        }
    });

    //#endregion 

    //#region FEATURE (LEGENDE) 

    $.ajax({
        type: "GET",
        url: "/WebAPI/api/DroneFlightsAPI/" + id, // the URL of the controller action method
        success: (result) => {
            let feature = new Feature({
                id: 'infoFeature',
                graphic: new Graphic({
                    attributes: {
                        pilotName: result.PilotName,
                        droneName: result.DroneName,

                        departureUTC: result.DepartureUTC,
                        departureLatitude: result.DepartureLatitude,
                        departureLongitude: result.DepartureLongitude,

                        destinationUTC: result.DestinationUTC,
                        destinationLatitude: result.DestinationLatitude,
                        destinationLongitude: result.DestinationLongitude
                    },
                    popupTemplate: {
                        title: "Drone Flight Information",
                        outFields: ["*"],
                        content: (feature) => {
                            var node = document.createElement("div");

                            node.innerHTML = `<div class="droneFlightPopup">
	<table>
		<tr><td rowspan="2">👨‍✈️</td><th>Pilot</th><td>${feature.graphic.attributes.pilotName}</td></tr>
		<tr><th>Drone</th><td>${feature.graphic.attributes.droneName}</td></tr>
		<tr class="titleRow"><td>↗️</td><th colspan="2">Departure</th></tr>
		<tr><td></td><th>Time (UTC)</th><td>${feature.graphic.attributes.departureUTC}</td></tr>
		<tr class="coordinatesRow"><td></td><th>Coordinates</th><td>${feature.graphic.attributes.departureLatitude},${feature.graphic.attributes.departureLongitude}</td></tr>
		<tr class="titleRow"><td>↘️</td><th colspan="2">Destination</th></tr>
		<tr><td></td><th>Time (UTC)</th><td>${feature.graphic.attributes.destinationUTC}</td></tr>
		<tr class="coordinatesRow"><td></td><th>Coordinates</th><td>${feature.graphic.attributes.destinationLatitude},${feature.graphic.attributes.destinationLongitude}</td></tr>
	</table>
</div>`;
                            return node;
                        }
                    }
                }),
                map: map
            });

            //center and zoom to track starting point
            view.center = [result.DepartureLongitude, result.DepartureLatitude];
            view.zoom = 16;

            view.ui.add(feature, "top-left");
        },
        error: (req, status, error) => {
            console.log("AJAX FAIL: FEATURE (LEGENDE)");
        }
    });
    //#endregion 


    $(document).ready(function () {


        //#region FANCY POPUP for IMAGES
        $(document).on('click', '.thumbnail-enlarge', function (e) {
            e.preventDefault();
            $.magnificPopup.open({ type: 'image', items: { src: $(this).attr('href') } });
            return false;
        });
        //#endregion

        //#region interface voor visual variables 
        let visualVariableInterface = new Feature({
            id: 'visualVariableInterface',
            graphic: new Graphic({
                popupTemplate: {
                    title: "Color Coding", 
                    outFields: ["*"],
                    content: (feature) => {
                        var node = document.createElement("div");

                        node.innerHTML = `<div class="colorCodingInterface">
	<button class="colorCodingButton active" data-colorMode="height">Height (MSL)</button>
	<button class="colorCodingButton" data-colorMode="velocity">Velocity</button>
	<button class="colorCodingButton" data-colorMode="battery">Battery %</button>
</div>`;
                        return node;
                    }
                },

            }),
            map: map
        });

        view.ui.add(visualVariableInterface, "bottom-left");

        $(document).on('click', '.colorCodingButton', function () {
            var mode = $(this).attr('data-colorMode');
            var useMode = colorVar_BatteryPercentage;
            if (mode == 'height') useMode = colorVar_HeightMSL;
            if (mode == 'velocity') useMode = colorVar_VelComposite;
            if (mode == 'battery') useMode = colorVar_BatteryPercentage;
            customRenderer.visualVariables = [useMode];
            trackFeatureLayer.renderer = customRenderer;
            $(this).addClass('active').siblings().removeClass('active');

        });
        //#endregion

    });




});