let intervalID;
let totalFilesToParse;
let filesLeftToParse;
let amountParsed;
let currentFile;
let my_form;
let firstCheck = true;

$(document).ready(function () {
    // File extension verification; upload button will be disabled...
    // ...if the user tries to upload an extension that is not allowed
    $("#file").change(function () {
        var fileExtension = ['pdf', 'dat', 'txt', 'csv', 'xyz', 'tfw', 'jpg']; // allowed extensions
        var files = $("#file").get(0).files;
        var totalSize = 0;
        var imageSize = 0;
        for (i = 0; i < files.length; i++) { // limit images upload to 500 MB
            if (files[i].type.toLowerCase() == 'image/jpeg') {
                imageSize += files[i].size;
            }
            totalSize += files[i].size;
        }
        if ($.inArray($(this).val().split('.').pop().toLowerCase(), fileExtension) == -1) {
            alert("Only the following formats are allowed : " + fileExtension.join(', ') + ". Please try again.");
            $("#uploadbtnSubmit").prop('disabled', true); // disable upload button
        }
        else if (totalSize > 2147483647) {
            alert("Maximum total upload size is: 2.1 GB.");
            $("#uploadbtnSubmit").prop('disabled', true); // disable upload button
            $("#maximum").show();
            $("#maxUploadTextFiles").show();
        }
        else if (imageSize > 524288000) { // 500 MB
            alert("Maximum total upload size for images is: 500 MB.");
            $("#uploadbtnSubmit").prop('disabled', true); // disable upload button
            $("#maximum").show();
            $("#maxUploadTextImages").show();
        }
        else {
            $("#uploadbtnSubmit").prop('disabled', false); // enable upload button
        }
    });

    // AjaxForm
    $('#Myform').ajaxForm({
        beforeSend: function (xhr) {
            // If the user did not submit any files, abort the ajaxForm
            if ($('#file').get(0).files.length === 0) {
                xhr.abort();
            }
            else {
                //init 
                $("#progressbar").progressbar({ value: 0 }); // reset to 0%
                $("#progressbar").progressbar("enable");
                initHide();
                totalFilesToParse = $('#file').get(0).files.length; // amount of uploaded files
                filesLeftToParse = 0; // reset
                firstCheck = true;
            }
        },
        uploadProgress: function (event, position, total, percentComplete) {
            $("#uploadstatus").text("Uploading files... (" + percentComplete + "%)");
            $("#progressbar").progressbar("value", percentComplete);
            // Once uploading is complete...
            if (percentComplete == 100) {
                $("#uploadstatus").text("Upload complete.");
                $("#initialMessage").hide();
                //console.log("Attempting to parse: " + totalFilesToParse + " files.");
                currentFile = "";
                $(".amountParsed").text(0);
                $(".totalParsed").text(totalFilesToParse);
                $("#progressField").show();
                // begin parsing
                //console.log("startParsing");
                startParsing();
            }
        },
        // Return values for error-handling: 
        // 1: success
        // 0: no files submitted
        // 2: no drone flight specified
        // 3: drone flight does not exist
        // 4: someone else is already uploading
        // 5: invalid file type
        success: function (errorCode) {
            // to do: print message on screen for user
            // except when it is == 1
            if (errorCode != 1) {
                $("#progressField").hide();
                $("#endField").hide();
                let text;
                if (errorCode == 0) {
                    text = "No files were submitted.";
                }
                else if (errorCode == 2) {
                    text = "Please specify a Drone Flight.";
                }
                else if (errorCode == 3) {
                    text = "This Drone Flight does not exist.";
                }
                else if (errorCode == 4) {
                    text = "Someone else is already uploading. Please try again in a few minutes.";
                }
                else if (errorCode == 4) {
                    text = "An invalid file type was submitted.";
                }
                $("#errorField").show();
                $("#errorMessage").text(text);
            }
        }
    });

    // Helper-function: Reset all output fields to hidden
    function initHide() {
        $("#endField").hide(); // hide endField ending message 
        $("#successField").hide(); // hide the successField ending
        $("#failedField").hide(); // hide failedField ending message
        $("#progressField").hide(); // hide progress of reading files
        $("#failedFilesList").empty(); // empty the unordered list of failed files
        $("#moreUpload").hide(); // hide the moreUpload text
        $("#oneFile").hide(); // hide single failed file text
        $("#multipleFiles").hide(); // hide multiple failed files text
        $("#maxUploadTextImages").hide(); // hide maximum upload text
        $("#maxUploadTextFiles").hide(); // hide maximum upload text
        $("#maximum").hide(); // hide maximum upload text
    }

}); // end of $(document).ready

// Changes the progress bar value at an interval
function startParsing() {
    $("#progressbar").progressbar({ value: 0 });
    intervalID = setTimeout(parse, 500);  //every 0.5 sec the progress bar updates
}

// Helper-function for startParsing: called in startParsing to change the progress bar value ...
// ... through an ajax call
function parse() {
    $.get("/Files/GetStatus/", function (result) {
        currentFile = result.currFileName // set the current file
        filesLeftToParse = result.currFilesLeft;
        if (firstCheck) {
            if (filesLeftToParse > 0) {
                firstCheck = false;
            }
            intervalID = setTimeout(parse, 500);
            return;
        }

        amountParsed = totalFilesToParse - filesLeftToParse;
        // set View
        $(".amountParsed").text(amountParsed);
        // Update the amount of files that still have to be parsed
        //console.log("Files left to parse: " + filesLeftToParse);
        //update the progress bar
        let progress = Math.round((amountParsed / totalFilesToParse
                                        + (result.currProgress / 100)
                                        / totalFilesToParse)
                                            * 100);
        $("#uploadstatus").text("Parsing file: " + currentFile + " (" + progress + "%)");
        $("#progressbar").progressbar("value", progress);
        console.log(amountParsed + " " + totalFilesToParse);
        if (amountParsed == totalFilesToParse) { // ending procedure
            //console.log("Parsed: " + amountParsed + " (had to parse: " + totalFilesToParse + ")");
            clearTimeout(intervalID); // clear
            $(".amountParsed").text(amountParsed);
            $(".totalParsed").text(totalFilesToParse);
            $("#uploadstatus").text("Parsing complete.");
            $("#progressField").hide();
            //console.log("Done.")

            // Check whether any files failed to parse because of duplicates
            if (result.failedFiles.length == 0) { // no failed files
                $("#successField").show();
                $("#endField").addClass("alert-success");
                $("#endField").removeClass("alert-warning");
                $("#endField").show();
            }
            else { // failed files
                console.log("array length else: " + result.failedFiles.length);
                $("#failedField").show();
                $(".totalFailed").text(result.failedFiles.length);
                if (result.failedFiles.length > 1) { // more than 1 failed file
                    $("#multipleFiles").show()
                }
                else { // 1 failed file
                    $("#oneFile").show()
                }
                $("#endField").removeClass("alert-success");
                $("#endField").addClass("alert-warning");
                document.getElementById("failedFilesList").appendChild(makeUL(result.failedFiles));
                $("#endField").show();
            }
            $("#moreUpload").show();
        }
        else { // fire another parse
            intervalID = setTimeout(parse, 500);
        }
    });

}

// Make the HTML unordered list for the failed files
function makeUL(array) {
    // Create the list element:
    var list = document.createElement('ul');

    for (var i = 0; i < array.length; i++) {
        // Create the list item:
        var item = document.createElement('li');

        // Set its contents:
        item.appendChild(document.createTextNode(array[i]));

        // Add it to the list:
        list.appendChild(item);
    }

    // Finally, return the constructed list:
    return list;
}

