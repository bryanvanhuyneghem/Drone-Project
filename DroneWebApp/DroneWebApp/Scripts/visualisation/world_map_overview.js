require([
    "esri/Map",
    "esri/views/MapView",
    "esri/geometry/SpatialReference",
    "esri/widgets/Search",
    "esri/PopupTemplate",
    "esri/layers/FeatureLayer",
    "esri/widgets/Legend"
], function (Map, MapView, SpatialReference, Search, PopupTemplate, FeatureLayer, Legend) {

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

    //#region get all droneflights 
    let readFlightpoint = (fp) => {
        let pointGraphic = {             //type graphic (autocasts)
            geometry: {
                type: "point",
                x: fp.DepartureLongitude,
                y: fp.DepartureLatitude
            },
            attributes: {
                FlightId: fp.FlightId,
                PilotName: fp.PilotName,
                DroneName: fp.DroneName,
                DepartureUTC: fp.DepartureUTC,
                DepartureLatitude: fp.DepartureLatitude,
                DepartureLongitude: fp.DepartureLongitude,
                DestinationUTC: fp.DestinationUTC,
                DestinationLatitude: fp.DestinationLatitude,
                DestinationLongitude: fp.DestinationLongitude
            }
        };
        return pointGraphic;
    }

    $.ajax({
        type: "GET",
        url: "/WebAPI/api/DroneFlightsAPI/",
        success: (result) => {

            let flightpoints = [];
            for (let i = 0; i < result.length; i++) {
                flightpoints.push(readFlightpoint(result[i]));
            }

            flightsFeatureLayer = new FeatureLayer({
                title: "Track starting points", 
                source: flightpoints,
                fields: [
                    {
                        name: "FlightId",
                        type: "integer"
                    },
                    {
                        name: "PilotName",
                        type: "string"
                    },
                    {
                        name: "DroneName",
                        type: "string"
                    },
                    {
                        name: "DepartureUTC",
                        type: "string"
                    },
                    {
                        name: "DepartureLatitude",
                        type: "double"
                    },
                    {
                        name: "DepartureLongitude",
                        type: "double"
                    },
                    {
                        name: "DestinationUTC",
                        type: "string"
                    },
                    {
                        name: "DestinationLatitude",
                        type: "double"
                    },
                    {
                        name: "DestinationLongitude",
                        type: "double"
                    }
                ],
                objectIdField: "FlightId",
                geometryType: "point",
                popupTemplate: {
                    title: "Drone Flight Information",
                    outFields: ["*"],
                    content: (feature) => {   //we make a custom popup with our own html
                        var node = document.createElement("div");

                        node.innerHTML = `<div class="droneFlightPopup">
	<table>
		<tr><td rowspan="2">👨‍✈️</td><th>Pilot</th><td>${feature.graphic.attributes.PilotName}</td></tr>
		<tr><th>Drone</th><td>${feature.graphic.attributes.DroneName}</td></tr>
		<tr class="titleRow"><td>↗️</td><th colspan="2">Departure</th></tr>
		<tr><td></td><th>Time (UTC)</th><td>${feature.graphic.attributes.DepartureUTC}</td></tr>
		<tr class="coordinatesRow"><td></td><th>Coordinates</th><td>${feature.graphic.attributes.DepartureLatitude},${feature.graphic.attributes.DepartureLongitude}</td></tr>
		<tr class="titleRow"><td>↘️</td><th colspan="2">Destination</th></tr>
		<tr><td></td><th>Time (UTC)</th><td>${feature.graphic.attributes.DestinationUTC}</td></tr>
		<tr class="coordinatesRow"><td></td><th>Coordinates</th><td>${feature.graphic.attributes.DestinationLatitude},${feature.graphic.attributes.DestinationLongitude}</td></tr>
	</table><a href="/Map/ViewMap/${feature.graphic.attributes.FlightId}">Go to Flight ▶️</a>
</div>`;
                        return node;
                    }
                },
                renderer: {
                    type: "simple",
                    symbol: {
                        type: "text",
                        color: "#1d82ac",
                        text: "\ue61d",
                        //size: 20,
                        font: {
                            size: 20,
                            family: "CalciteWebCoreIcons"
                        }
                    }
                }
            });

            map.add(flightsFeatureLayer);

        },
        error: (req, status, error) => {
            console.log("AJAX FAIL: TRACK VISUALISATION");
            console.log(req);
            console.log(status);
            console.log(error);
        }
    });

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
});