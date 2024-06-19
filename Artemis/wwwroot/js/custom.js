/* -------------------------------------------------------------------------
 * CUSTOM JQUERY FUNCTIONS AND WRAPPERS
 * DATE: 6-12-2018
 * AUTHOR: Various sources
 * ------------------------------------------------------------------------
 */

function MessageBoxConfirm(message, title) {
    return $.MessageBox({
        buttonDone: "Yes",
        buttonFail: "No",
        speed: 250,
        top: "35%",
        width: "auto",
        queue: false,
        title: title,
        message: message
    });
}

function MessageBoxAlert(message, title) {
    return $.MessageBox({
        speed: 250,
        top: "40%",
        title: title,
        width: "auto",
        queue: false,
        message: message
    });
}

function TryParseInt(str, defaultValue) {
    var retValue = defaultValue;
    if (str !== null) {
        if (str.length > 0) {
            if (!isNaN(str)) {
                retValue = parseInt(str);
            }
        }
    }
    return retValue;
}

function error_handler(e) {
    if (e.errors) {
        var message = "";
        $.each(e.errors, function (key, value) {
            if ('errors' in value) {
                $.each(value.errors, function () {
                    message += this + "<br/>";
                });
            }
        });
        MessageBoxAlert(message, "Error");
        RefreshGrid(e);
    }
}

function error_handler_norefresh(e) {
    if (e.errors) {
        var message = "";
        $.each(e.errors, function (key, value) {
            if ('errors' in value) {
                $.each(value.errors, function () {
                    message += this + "<br/>";
                });
            }
        });
        MessageBoxAlert(message, "Error");
    }
}

function RefreshGrid() {
    $("#grid").data("kendoGrid").dataSource.read();
}
