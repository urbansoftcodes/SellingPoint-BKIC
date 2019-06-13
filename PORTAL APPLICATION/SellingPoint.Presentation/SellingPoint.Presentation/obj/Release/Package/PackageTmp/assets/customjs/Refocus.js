var lastFocusedControlId = "";

function focusHandler(e) {
    document.activeElement = e.originalTarget;
}

function appInit() {
    if (typeof (window.addEventListener) !== "undefined") {
        window.addEventListener("focus", focusHandler, true);
    }
    Sys.WebForms.PageRequestManager.getInstance().add_pageLoading(pageLoadingHandler);
    Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(pageLoadedHandler);
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(Ending);
}

function pageLoadingHandler(sender, args) {
    //lastFocusedControlId = typeof (document.activeElement) === "undefined"
    //    ? "" : document.activeElement.id;    

    var fe = document.activeElement;
    //focusedElementSelector = "";

    if (fe != null) {
        if (fe.id) {
            lastFocusedControlId = "#" + fe.id;
        } else {
            // Handle Chosen Js Plugin
            var $chzn = $(fe).closest('.chosen-container[id]');
            if ($chzn.size() > 0) {
                lastFocusedControlId = '#' + $chzn.attr('id') + ' input[type=text]';
            }
        }
    }
}

function focusControl(targetControl) {
    if (Sys.Browser.agent === Sys.Browser.InternetExplorer) {
        var focusTarget = targetControl;
        if (focusTarget && (typeof (focusTarget.contentEditable) !== "undefined")) {
            oldContentEditableSetting = focusTarget.contentEditable;
            focusTarget.contentEditable = false;
        }
        else {
            focusTarget = null;
        }
        targetControl.focus();
        if (focusTarget) {
            focusTarget.contentEditable = oldContentEditableSetting;
        }
    }
    else {
        var controlID = $(targetControl).attr("id");
        if (controlID.toLowerCase().indexOf("ddl") >= 0) {
            $(targetControl).trigger('chosen:activate')
        }
        else {
            targetControl.focus();
        }  
    }
}

function pageLoadedHandler(sender, args) {
    //if (typeof (lastFocusedControlId) !== "undefined" && lastFocusedControlId != "") {
    //    var newFocused = $get(lastFocusedControlId);
    //    if (newFocused) {
    //        focusControl(newFocused);
    //    }
    //}
    var fe = document.activeElement;
    //focusedElementSelector = "";

    if (fe != null) {
        if (fe.id) {
            lastFocusedControlId = "#" + fe.id;
        } else {
            // Handle Chosen Js Plugin
            var $chzn = $(fe).closest('.chosen-container[id]');
            if ($chzn.size() > 0) {
                lastFocusedControlId = '#' + $chzn.attr('id') + ' input[type=text]';
            }
        }
    }        
    
}
function Ending(sender, args) {
    //if (typeof (lastFocusedControlId) !== "undefined" && lastFocusedControlId != "") {
    //    var newFocused = $get(lastFocusedControlId);
    //    if (newFocused) {
    //        focusControl(newFocused);
    //    }
    //}
    if (lastFocusedControlId) {
        $(lastFocusedControlId).focus();
    }
}

Sys.Application.add_init(appInit);