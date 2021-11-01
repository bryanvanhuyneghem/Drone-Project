// These scripts change the regular tables selected by id into DataTables that have searchability, sortability and paging

// DroneFlights table (admin & user)
$(document).ready(function () {
    $.fn.dataTable.moment('DD/MM/YYYY');

    $('#dftable').dataTable({
        "columnDefs": [
            {
                "orderable": false, "targets": [-4, 6, 7, 8],
                "searchable": false, "targets": [-4, 6, 7, 8]
            }
        ]
      }); 
});

// DroneFlights table (viewer)
$(document).ready(function () {
    $.fn.dataTable.moment('DD/MM/YYYY');

    $('#dftable_view').dataTable({
        "columnDefs": [
            {
                "orderable": false, "targets": [5, 6, 7],
                "searchable": false, "targets": [5, 6, 7]
            }
        ]
    });
});

// Projects table
$(document).ready(function () {
    $('#prtable').dataTable({
        "columnDefs": [
            {
                "orderable": false, "targets": [2, 3],
                "searchable": false, "targets": [2, 3]
            }
        ]
    });
});

// Drones table
$(document).ready(function () {
    $('#dtable').dataTable({
        "columnDefs": [
            {
                "orderable": false, "targets": [5, 6],
                "searchable": false, "targets": [5, 6]
            }
        ]
    });
});

// Pilots table
$(document).ready(function () {
    $('#ptable').dataTable({
        "columnDefs": [
            {
                "orderable": false, "targets": [4, 5],
                "searchable": false, "targets": [4, 5]
            }
        ]
    });
});


// Pilot's Drone Flights table
$(document).ready(function () {
    $.fn.dataTable.moment('DD/MM/YYYY');

    $('#pdftable').dataTable({
        "columnDefs": [
            {
                "orderable": false, "targets": [-4, 5, 6, 7],
                "searchable": false, "targets": [-4, 5, 6, 7]
            }
        ]
    });
});

// Drone's Drone Flights table
$(document).ready(function () {
    $.fn.dataTable.moment('DD/MM/YYYY');

    $('#ddftable').dataTable({
        "columnDefs": [
            {
                "orderable": false, "targets": [-4, 5, 6, 7],
                "searchable": false, "targets": [-4, 5, 6, 7]
            }
        ]
    });
});

// Project's DroneFlights table
$(document).ready(function () {
    $.fn.dataTable.moment('DD/MM/YYYY');

    $('#prdftable').dataTable({
        "columnDefs": [
            {
                "orderable": false, "targets": [-4, 5, 6, 7],
                "searchable": false, "targets": [-4, 5, 6, 7]
            }
        ]
    });
});

// Project's Drone Flights table (viewer)
$(document).ready(function () {
    $.fn.dataTable.moment('DD/MM/YYYY');

    $('#prdftable_view').dataTable({
        "columnDefs": [
            {
                "orderable": false, "targets": [4, 5, 6],
                "searchable": false, "targets": [4, 5, 6]
            }
        ]
    });
});

// CTRL points table
$(document).ready(function () {
    $('#ctrlptable').dataTable();
});

// GCP table
$(document).ready(function () {
    $('#gcpptable').dataTable();
});

// Images table
$(document).ready(function () {
    $('#imgtable').dataTable({
        /*
        "columnDefs": [
            {
                "orderable": false, 
                "searchable": false, "targets": 0
            }
        ]
        */
        searching: false
    });
});