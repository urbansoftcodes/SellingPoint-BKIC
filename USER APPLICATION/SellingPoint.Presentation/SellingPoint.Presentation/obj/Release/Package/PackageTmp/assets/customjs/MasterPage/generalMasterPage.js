var intervalSlide;
var ishomePage = false;
var achievementsInterval;

$(document).ready(function () {
    pageLoad();


    $(document).on("click", '.but', function (e) {
        e.preventDefault();
        var button = ("#" + this.id + "div");
        var btn_css = $(button).css('display');

        if (btn_css == 'block') {
            $(".home-form").hide();
            $("#" + this.id + "div").hide();
        } else {
            $(".home-form").hide();
            $("#" + this.id + "div").show();
        }
    });

    $(document).on('click', '.menu-trigger-bkic .menu-item-bkic', function () {
        var menu_trigger_id = $(this).attr("data-menuorder-id");
        var menu = $(this);

        selectMainMenuId(menu, menu_trigger_id);
    });

    HideShowHiddenRequiredSpan();

    if ($('[id*="userDetailsName"]').html() != undefined && $('[id*="userDetailsName"]').html().length > 0) {
        $("#btn2div").hide();
        $("#btn2").hide();
    }
    else {
        $("#btn3div").hide();
        $("#btn3").hide();
    }

    var mainMenuOrderId = $("[id*='hdnMainMenuId']").val();

    if (mainMenuOrderId != undefined && mainMenuOrderId.length > 0) {
        var menu = $(".menu-trigger-bkic .menu-item-bkic[data-menuorder-id='" + mainMenuOrderId + "']");
        selectMainMenuId(menu, mainMenuOrderId);
    }
    else {
        var menu = $(".menu-trigger-bkic .menu-item-bkic[data-menuorder-id='0']");
        selectMainMenuId(menu, 0);
    }

    $(document).on('click', '.slide-menus-bar-mobile', function () {
        $('.side-bar-menus').toggleClass('open');
        $('.side-bar-bkic-sticky').toggleClass('hide-body-content');
    });

    setTimeout(function () {
        showPopupAfterLoad();
    }, 1500);
   
    $(document).on("click", ".sucess i", function () {
        $('.successs').hide();
    });

    $(".side-bar-bkic-sticky ol li").on("mouseenter touchstart", function () {
        clearInterval(intervalSlide);
        $(".side-bar-bkic-sticky  .show-bkic li").removeClass("active");
        $(this).addClass("active");
    });

    $(".side-bar-bkic-sticky ol li").on("mouseleave  touchend", function () {
        if (ishomePage != undefined && ishomePage == true) {
            autoSlideSideBar();
        }
        else {
            $(".side-bar-bkic-sticky ol li").removeClass("active");
        }
    });

    $('.show-bkic li').on('click', function (event) {
        var linkElement = $(this).find("a");

        if (linkElement != undefined && $(linkElement).attr("href") != undefined && $(linkElement).attr("href") != "") {
            //$(linkElement).trigger("click");
            window.location.href = $(linkElement).attr("href");
        }
    });

    $(document).on('keypress', 'input[type=text],textarea', function (event) {
        var regex = new RegExp("^[<>%^#*{}]+$");
        var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
        if (regex.test(key)) {
            event.preventDefault();
            return false;
        }
    });

    $(".chzn-select").chosen();
    $(".chzn-select-deselect").chosen({ allow_single_deselect: true });
    $('.chosen-single').focus();
    $(".chzn-select").chosen();


    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
    function EndRequestHandler(sender, args) {
        //Binding Code Again
        $(".chzn-select").chosen();
        $('.chosen-single').focus();
        autocompleteCPR();
        getDomesticPolicies();
        getTravelPolicies();
        getHomePolicies();
        getMotorPolicies();
        getMotorEndorsementPolicies();
        getHomeEndorsementPolicies();
        getTravelEndorsementPolicies();
        getMotorSystemRenewalPolicies();
        getHomeSystemRenewalPolicies();
        getMotorOracleRenewalPolicies();
        getHomeOracleRenewalPolicies();
    }

});

function autocompleteCPR() {  
    var agency = $('#AgencyHidden').val();
    var agentCode = $('#AgentCodeHidden').val();
    var agentBranch = $('#AgentBranch').val();
    var webApiUrl = $('#WebApiUrlHidden').val();

    $(document).find('[id*=txtCPRSearch]').autocomplete({
        minLength: 3
        , source: function (request, response) {
            $.ajax({
                url: webApiUrl + "api/admin/getagencycpr/" + request.term + "/" + agency,
                dataType: "json",
                type: "GET",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    response(data);
                    $(".dropdown-menu").css("height", "auto");
                },
                error: function (response) {
                    alert(response.responseText);
                },
                failure: function (response) {
                    alert(response.responseText);
                }               
            });
        },
        select: function (event, ui) {  
           $('[id*=btnCPRSearch]').click();
        },        

    });
}

function getDomesticPolicies() {
    var agency = $('#AgencyHidden').val();
    var agentCode = $('#AgentCodeHidden').val();
    var agentBranch = $('#AgentBranch').val();
    var webApiUrl = $('#WebApiUrlHidden').val();

    $(document).find('[id*=txtDomesticPolicySearch]').autocomplete({
        minLength: 3
        , source: function (request, response) {
            $.ajax({
                url: webApiUrl + "api/domestichelp/GetDomesticAgencyPolicy/",
                dataType: "json",
                type: "POST",
                contentType: "application/json",
                data: JSON.stringify({
                    'agency': agency,
                    'agentCode': agentCode,
                    'isRenewal': 0,
                    'agentBranch': agentBranch,
                    'includeHIR': 0,
                    'documentNo': request.term
                }),
                success: function (res) {
                    items = [];                   
                    $.each(res.result.domesticAgencyPolicies, function (index, value) {
                        items.push(value.documentNo);
                    });
                    response(items);
                    $(".dropdown-menu").css("height", "auto");
                },
                error: function (response) {
                    alert(response.responseText);
                },
                failure: function (response) {
                    alert(response.responseText);
                }
            });
        },       
        
    });
}

function getTravelPolicies() {

    var agency = $('#AgencyHidden').val();
    var agentCode = $('#AgentCodeHidden').val();
    var agentBranch = $('#AgentBranch').val();
    var webApiUrl = $('#WebApiUrlHidden').val();

    $(document).find('[id*=txtTravelPolicySearch]').autocomplete({
        minLength: 3
        , source: function (request, response) {
            $.ajax({
                url: webApiUrl + "api/travelinsurance/getAgencyPolicy/",
                dataType: "json",
                type: "POST",
                contentType: "application/json",
                data: JSON.stringify({
                    'agency': agency,
                    'agentCode': agentCode,
                    'isRenewal': 0,
                    'agentBranch': agentBranch,
                    'includeHIR': 0,
                    'documentNo': request.term
                }),
                success: function (res) {
                    items = [];
                    $.each(res.result.agencyTravelPolicies, function (index, value) {
                        items.push(value.documentNo);
                    });
                    response(items);
                    $(".dropdown-menu").css("height", "auto");
                },
                error: function (response) {
                    alert(response.responseText);
                },
                failure: function (response) {
                    alert(response.responseText);
                }
            });
        },
    });
}

function getHomePolicies() {

    var agency = $('#AgencyHidden').val();
    var agentCode = $('#AgentCodeHidden').val();
    var agentBranch = $('#AgentBranch').val();
    var webApiUrl = $('#WebApiUrlHidden').val();

    var map = {};

    $(document).find('[id*=txtHomePolicySearch]').autocomplete({        
        minLength: 3
        , source: function (request, response) {
            $.ajax({
                url: webApiUrl + "api/home/GetHomeAgencyPolicy/",
                dataType: "json",
                type: "POST",
                contentType: "application/json",
                data: JSON.stringify({
                    'agency': agency,
                    'agentCode': agentCode,
                    'isRenewal': 0,
                    'agentBranch': agentBranch,
                    'includeHIR': 0,
                    'documentNo': request.term
                }),
                success: function (res) {
                    var items = [];                    
                    $.each(res.result.agencyHomePolicies, function (index, value) {
                        map[value.documentNo] = { renewalCount: value.renewalCount, documentNo: value.documentNo };
                        items.push(value.documentNo);
                    });
                    response(items);                   
                    $(".dropdown-menu").css("height", "auto");
                },
                error: function (response) {
                    alert(response.responseText);
                },
                failure: function (response) {
                    alert(response.responseText);
                }
            });
        },
        select: function (event, ui) {           
            $(document).find('[id*=txtHomePolicySearch]').val(ui.item.label); // display the selected text
            $(document).find('[id*=renewalCount]').val(map[ui.item.label].renewalCount); // save selected id to hidden input          
            return false;
        }       
    });
}

function getMotorPolicies() {

    var agency = $('#AgencyHidden').val();
    var agentCode = $('#AgentCodeHidden').val();
    var agentBranch = $('#AgentBranch').val();
    var webApiUrl = $('#WebApiUrlHidden').val();

    var map = {};

    $(document).find('[id*=txtMotorPolicySearch]').autocomplete({
        minLength: 3
        , source: function (request, response) {
            $.ajax({
                url: webApiUrl + "api/motor/GetMotorAgencyPolicy/",
                dataType: "json",
                type: "POST",
                contentType: "application/json",
                data: JSON.stringify({
                    'agency': agency,
                    'agentCode': agentCode,
                    'isRenewal': 0,
                    'agentBranch': agentBranch,
                    'includeHIR': 0,
                    'documentNo': request.term
                }),
                success: function (res) {
                    var items = [];
                    $.each(res.result.agencyMotorPolicies, function (index, value) {
                        map[value.documentNo] = { renewalCount: value.renewalCount, documentNo: value.documentNo };
                        items.push(value.documentNo);
                    });
                    response(items);
                    $(".dropdown-menu").css("height", "auto");
                },
                error: function (response) {
                    alert(response.responseText);
                },
                failure: function (response) {
                    alert(response.responseText);
                }
            });
        },
        select: function (event, ui) {
            $(document).find('[id*=txtMotorPolicySearch]').val(ui.item.label); // display the selected text
            $(document).find('[id*=renewalCount]').val(map[ui.item.label].renewalCount); // save selected id to hidden input
            return false;
        },     
    });
}

function getMotorEndorsementPolicies() {

    var agency = $('#AgencyHidden').val();
    var agentCode = $('#AgentCodeHidden').val();
    var agentBranch = $('#AgentBranch').val();
    var webApiUrl = $('#WebApiUrlHidden').val();

    var map = {};

    $(document).find('[id*=txtMotorEndorsementSearch]').autocomplete({
        minLength: 3
        , source: function (request, response) {
            $.ajax({
                url: webApiUrl + "api/motor/getmotorpoliciesendorsement/",
                dataType: "json",
                type: "POST",
                contentType: "application/json",
                data: JSON.stringify({
                    'agency': agency,
                    'agentCode': agentCode,
                    'isRenewal': 0,
                    'agentBranch': agentBranch,
                    'includeHIR': 0,
                    'documentNo': request.term
                }),
                success: function (res) {
                    var items = [];
                    $.each(res.result.agencyMotorPolicies, function (index, value) {
                        map[value.documentNo] = { renewalCount: value.renewalCount, documentNo: value.documentNo };
                        items.push(value.documentNo);
                    });
                    response(items);
                    $(".dropdown-menu").css("height", "auto");
                },
                error: function (response) {
                    alert(response.responseText);
                },
                failure: function (response) {
                    alert(response.responseText);
                }
            });
        },
        select: function (event, ui) {
            $(document).find('[id*=txtMotorEndorsementSearch]').val(ui.item.label); // display the selected text
            $(document).find('[id*=renewalCount]').val(map[ui.item.label].renewalCount); // save selected id to hidden input
            return false;
        },       
    });
}

function getHomeEndorsementPolicies() {

    var agency = $('#AgencyHidden').val();
    var agentCode = $('#AgentCodeHidden').val();
    var agentBranch = $('#AgentBranch').val();
    var webApiUrl = $('#WebApiUrlHidden').val();

    var map = {};

    $(document).find('[id*=txtHomeEndorsementSearch]').autocomplete({
        minLength: 3
        , source: function (request, response) {
            $.ajax({
                url: webApiUrl + "api/home/gethomepoliciesendorsement/",
                dataType: "json",
                type: "POST",
                contentType: "application/json",
                data: JSON.stringify({
                    'agency': agency,
                    'agentCode': agentCode,
                    'isRenewal': 0,
                    'agentBranch': agentBranch,
                    'includeHIR': 0,
                    'documentNo': request.term
                }),
                success: function (res) {
                    var items = [];
                    $.each(res.result.agencyHomePolicies, function (index, value) {
                        map[value.documentNo] = { renewalCount: value.renewalCount, documentNo: value.documentNo };
                        items.push(value.documentNo);
                    });
                    response(items);
                    $(".dropdown-menu").css("height", "auto");
                },
                error: function (response) {
                    alert(response.responseText);
                },
                failure: function (response) {
                    alert(response.responseText);
                }
            });
        },
        select: function (event, ui) {
            $(document).find('[id*=txtHomeEndorsementSearch]').val(ui.item.label); // display the selected text
            $(document).find('[id*=renewalCount]').val(map[ui.item.label].renewalCount); // save selected id to hidden input
            return false;
        },      
    });
}

function getTravelEndorsementPolicies() {

    var agency = $('#AgencyHidden').val();
    var agentCode = $('#AgentCodeHidden').val();
    var agentBranch = $('#AgentBranch').val();
    var webApiUrl = $('#WebApiUrlHidden').val();

    var map = {};

    $(document).find('[id*=txtTravelEndorsementSearch]').autocomplete({
        minLength: 3
        , source: function (request, response) {
            $.ajax({
                url: webApiUrl + "api/travel/gettravelpoliciesendorsement/",
                dataType: "json",
                type: "POST",
                contentType: "application/json",
                data: JSON.stringify({
                    'agency': agency,
                    'agentCode': agentCode,
                    'isRenewal': 0,
                    'agentBranch': agentBranch,
                    'includeHIR': 0,
                    'documentNo': request.term
                }),
                success: function (res) {
                    var items = [];
                    $.each(res.result.agencyTravelPolicies, function (index, value) {
                        map[value.documentNo] = { renewalCount: value.renewalCount, documentNo: value.documentNo };
                        items.push(value.documentNo);
                    });
                    response(items);
                    $(".dropdown-menu").css("height", "auto");
                },
                error: function (response) {
                    alert(response.responseText);
                },
                failure: function (response) {
                    alert(response.responseText);
                }
            });
        },
        select: function (event, ui) {
            $(document).find('[id*=txtTravelEndorsementSearch]').val(ui.item.label); // display the selected text
            $(document).find('[id*=renewalCount]').val(map[ui.item.label].renewalCount); // save selected id to hidden input
            return false;
        },       
    });
}

function getMotorSystemRenewalPolicies() {

    var agency = $('#AgencyHidden').val();
    var agentCode = $('#AgentCodeHidden').val();
    var agentBranch = $('#AgentBranch').val();
    var webApiUrl = $('#WebApiUrlHidden').val();

    var map = {};

    $(document).find('[id*=txtMotorRenewalPolicySearch]').autocomplete({
        minLength: 3
        , source: function (request, response) {
            $.ajax({
                url: webApiUrl + "api/motor/GetMotorAgencyPolicy/",
                dataType: "json",
                type: "POST",
                contentType: "application/json",
                data: JSON.stringify({
                    'agency': agency,
                    'agentCode': agentCode,
                    'isRenewal': 1,
                    'agentBranch': agentBranch,
                    'includeHIR': 0,
                    'documentNo': request.term
                }),
                success: function (res) {
                    var items = [];
                    $.each(res.result.agencyMotorPolicies, function (index, value) {
                        map[value.documentNo] = { renewalCount: value.renewalCount, documentNo: value.documentNo };
                        items.push(value.documentNo);
                    });
                    response(items);
                    $(".dropdown-menu").css("height", "auto");
                },
                error: function (response) {
                    alert(response.responseText);
                },
                failure: function (response) {
                    alert(response.responseText);
                }
            });
        },
        select: function (event, ui) {
            $(document).find('[id*=txtMotorRenewalPolicySearch]').val(ui.item.label); // display the selected text
            $(document).find('[id*=renewalCount]').val(map[ui.item.label].renewalCount); // save selected id to hidden input
            return false;
        },

    });
}

function getHomeSystemRenewalPolicies() {

    var agency = $('#AgencyHidden').val();
    var agentCode = $('#AgentCodeHidden').val();
    var agentBranch = $('#AgentBranch').val();
    var webApiUrl = $('#WebApiUrlHidden').val();

    var map = {};

    $(document).find('[id*=txtHomeRenewalPolicySearch]').autocomplete({
        minLength: 3
        , source: function (request, response) {
            $.ajax({
                url: webApiUrl + "api/home/GetHomeAgencyPolicy/",
                dataType: "json",
                type: "POST",
                contentType: "application/json",
                data: JSON.stringify({
                    'agency': agency,
                    'agentCode': agentCode,
                    'isRenewal': 1,
                    'agentBranch': agentBranch,
                    'includeHIR': 0,
                    'documentNo': request.term
                }),
                success: function (res) {
                    var items = [];
                    $.each(res.result.agencyHomePolicies, function (index, value) {
                        map[value.documentNo] = { renewalCount: value.renewalCount, documentNo: value.documentNo };
                        items.push(value.documentNo);
                    });
                    response(items);
                    $(".dropdown-menu").css("height", "auto");
                },
                error: function (response) {
                    alert(response.responseText);
                },
                failure: function (response) {
                    alert(response.responseText);
                }
            });
        },
        select: function (event, ui) {
            $(document).find('[id*=txtHomeRenewalPolicySearch]').val(ui.item.label); // display the selected text
            $(document).find('[id*=renewalCount]').val(map[ui.item.label].renewalCount); // save selected id to hidden input           
            return false;
        },
       
    });
}

function getMotorOracleRenewalPolicies() {

    var agency = $('#AgencyHidden').val();
    var agentCode = $('#AgentCodeHidden').val();
    var agentBranch = $('#AgentBranch').val();
    var webApiUrl = $('#WebApiUrlHidden').val();

    var map = {};

    $(document).find('[id*=txtMotorOracleRenewalPolicySearch]').autocomplete({
        minLength: 3
        , source: function (request, response) {
            $.ajax({
                url: webApiUrl + "api/motor/GetOracleMotorRenewalPolicies/",
                dataType: "json",
                type: "POST",
                contentType: "application/json",
                data: JSON.stringify({
                    'agency': agency,
                    'agentCode': agentCode,
                    'isRenewal': 1,
                    'agentBranch': agentBranch,
                    'includeHIR': 0,
                    'documentNo': request.term
                }),
                success: function (res) {
                    var items = [];
                    $.each(res.result.agencyMotorPolicies, function (index, value) {
                        map[value.documentNo] = { renewalCount: value.renewalCount, documentNo: value.documentNo };
                        items.push(value.documentNo);
                    });
                    response(items);
                    $(".dropdown-menu").css("height", "auto");
                },
                error: function (response) {
                    alert(response.responseText);
                },
                failure: function (response) {
                    alert(response.responseText);
                }
            });
        },
        select: function (event, ui) {
            $(document).find('[id*=txtMotorOracleRenewalPolicySearch]').val(ui.item.label); // display the selected text
            $(document).find('[id*=renewalCount]').val(map[ui.item.label].renewalCount); // save selected id to hidden input
            return false;
        },      
    });
}

function getHomeOracleRenewalPolicies() {

    var agency = $('#AgencyHidden').val();
    var agentCode = $('#AgentCodeHidden').val();
    var agentBranch = $('#AgentBranch').val();
    var webApiUrl = $('#WebApiUrlHidden').val();

    var map = {};

    $(document).find('[id*=txtHomeOracleRenewalPolicySearch]').autocomplete({
        minLength: 3
        , source: function (request, response) {
            $.ajax({
                url: webApiUrl + "api/home/GetOracleHomeRenewalPolicies",
                dataType: "json",
                type: "POST",
                contentType: "application/json",
                data: JSON.stringify({
                    'agency': agency,
                    'agentCode': agentCode,
                    'isRenewal': 1,
                    'agentBranch': agentBranch,
                    'includeHIR': 0,
                    'documentNo': request.term
                }),
                success: function (res) {
                    var items = [];
                    $.each(res.result.agencyHomePolicies, function (index, value) {
                        map[value.documentNo] = { renewalCount: value.renewalCount, documentNo: value.documentNo };
                        items.push(value.documentNo);
                    });
                    response(items);
                    $(".dropdown-menu").css("height", "auto");
                },
                error: function (response) {
                    alert(response.responseText);
                },
                failure: function (response) {
                    alert(response.responseText);
                }
            });
        },
        select: function (event, ui) {
            $(document).find('[id*=txtHomeOracleRenewalPolicySearch]').val(ui.item.label); // display the selected text
            $(document).find('[id*=renewalCount]').val(map[ui.item.label].renewalCount); // save selected id to hidden input           
            return false;
        },       
    });
}

function selectMainMenuId(menu, menu_trigger_id) {
    $('.menu-trigger-bkic .menu-item-bkic').removeClass('active');
    if (!$(menu).hasClass('active')) {
        $(menu).addClass('active');
    }

    if ($(menu).attr('data-clickable') != "false") {
        if ($('.carousel-indicators[data-menuorder-id]').hasClass('show-bkic')) {
            $('.carousel-indicators[data-menuorder-id]').addClass('hide-bkic');

            $('.carousel-indicators[data-menuorder-id]').removeClass('show-bkic');
        }

        if ($('.carousel-indicators[data-menuorder-id="' + menu_trigger_id + '"]').hasClass('hide-bkic')) {
            $('.carousel-indicators[data-menuorder-id="' + menu_trigger_id + '"]').removeClass('hide-bkic');

            $('.carousel-indicators[data-menuorder-id="' + menu_trigger_id + '"]').addClass('show-bkic');
        }
        if (ishomePage != undefined && ishomePage == true) {
            if (intervalSlide != undefined) {
                clearInterval(intervalSlide);
            }

            autoSlideSideBar();
        }
    }
}

function pageLoad() {

    setCancelDateDate();
    setExtendDate();
    setRenewalDate();

    $(".datepicker").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: "dd/mm/yy",
        yearRange: 'c:c+90'
    }).attr('readOnly', 'true');

    $(".fromdate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: "dd/mm/yy",
        yearRange: 'c-10:c+90',
        //maxDate: -1,
    }).attr('readOnly', 'true');

    var date = new Date();
    var currentMonth = date.getMonth();
    var currentDate = date.getDate();
    var currentYear = date.getFullYear();

    $(".todate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: "dd/mm/yy",
        yearRange: 'c-10:c+90',
        //maxDate: -1,
    }).attr('readOnly', 'true');

 
    $(".policydate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: "dd/mm/yy",
        yearRange: 'c:c+1',
        minDate: new Date(currentYear, currentMonth, currentDate),
        maxDate: '+3m'
    }).attr('readOnly', 'true');
       

    $('.numbersOnly').keyup(function () {
        this.value = this.value.replace(/[^0-9\.]/g, '');
    });
    $(".onlynumber").keypress(function (e) {
        //if not number display error message
        if (e.which != 8 && e.which != 0 && e.which != 46 && e.which != 45 && (e.which < 48 || e.which > 57)) {
            return false;
        }
    });
    $(".onlynumbernotallowminus").keypress(function (e) {
        //if not number display error message
        if (e.which != 8 && e.which != 0 && e.which != 46 && (e.which < 48 || e.which > 57)) {
            return false;
        }
    });

    $(".above18").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: "dd/mm/yy",
        yearRange: '-90:-18',
        maxDate: '-18y'
    }).attr('readOnly', 'true');

    $(".blow20years").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: "dd/mm/yy",
        yearRange: '1997:2018',
        //maxDate: '-20y'
    }).attr('readOnly', 'true');

    $(".dateofbirth").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: "dd/mm/yy",
        yearRange: 'c-90:c',
        //maxDate: '-20y'
    }).attr('readOnly', 'true');

    $(".datepickerAge18to55").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: "dd/mm/yy",
        yearRange: '-55:-18',
        maxDate: '-18y',
    }).attr('readOnly', 'true');

    if ($("#userLogin_TxtLoginCPR") != undefined && $("#userLogin_txtLoginPassword") != undefined) {
        $("#userLogin_TxtLoginCPR,#userLogin_txtLoginPassword").keypress(function (e) {
            if (e.which == 13) {
                $("#userLogin_btnLogin").trigger('click');
            }
        });
    }
}

function showPageLoader(formgroup) {
    if (formgroup != undefined) {
        var listGroups = formgroup.split(',');

        //Single Group Validation.
        if (listGroups.length == 1) {
            if (Page_ClientValidate(listGroups[0])) {
                $(".modal-backdrop").remove();
                $("#pageLoadUpdatePanel").show();
                $("#loadPageUC_loading").show();
                HideShowHiddenRequiredSpan();
            }
            else {
                focusErrorElement();
            }
        }
        //Multiple Group Validation.
        else {
            var isValid = validateGroups(listGroups);
            if (!isValid) {
                focusErrorElement();
            }
            else {
                $(".modal-backdrop").remove();
                $("#pageLoadUpdatePanel").show();
                $("#loadPageUC_loading").show();
                HideShowHiddenRequiredSpan();
            }
        }
    }
    else {
        $("#pageLoadUpdatePanel").show();
        $("#loadPageUC_loading").show();
        HideShowHiddenRequiredSpan();

        // setTimeout(hidePageLoader, 5000);

    }

    if (formgroup == 'domesticValidation') {
        if ($('#ContentPlaceHolder1_formDomesticSubmitted').length > 0) {
            $('#ContentPlaceHolder1_formDomesticSubmitted').val('true');
        }
    }
    else if (formgroup == 'travelValidation') {
        if ($('#ContentPlaceHolder1_formTravelSubmitted').length > 0) {
            $('#ContentPlaceHolder1_formTravelSubmitted').val('true');
        }
    }
    else if (formgroup == 'HomeCalculationValidation') {
        if ($('#ContentPlaceHolder1_formHomeCalulated').length > 0) {
            $('#ContentPlaceHolder1_formHomeCalulated').val('true');
        }
    }
    else if (formgroup == 'HomeCalculationValidation,HomeInsuranceValidation') {
        if ($('#ContentPlaceHolder1_formHomeCalulated').length > 0) {
            $('#ContentPlaceHolder1_formHomeCalulated').val('true');
        }
        if ($('#ContentPlaceHolder1_formHomeSubmitted').length > 0) {
            $('#ContentPlaceHolder1_formHomeSubmitted').val('true');
        }
    }
    else if (formgroup == 'MotorCalculationValidation') {
        if ($('#ContentPlaceHolder1_formMotorCalculated').length > 0) {
            $('#ContentPlaceHolder1_formMotorCalculated').val('true');
        }
    }
    else if (formgroup == 'MotorCalculationValidation,MotorInsuranceValidation') {
        if ($('#ContentPlaceHolder1_formMotorCalculated').length > 0) {
            $('#ContentPlaceHolder1_formMotorCalculated').val('true');
        }
        if ($('#ContentPlaceHolder1_formMotorSubmitted').length > 0) {
            $('#ContentPlaceHolder1_formMotorSubmitted').val('true');
        }
    }
    else if (formgroup == 'MotorEndorsementValidation') {
        if ($('#ContentPlaceHolder1_endorsementSubmitted').length > 0) {
            $('#ContentPlaceHolder1_endorsementSubmitted').val('true');
        }
    }
}

function hidePageLoader() {
    $("#pageLoadUpdatePanel").hide();
    $("#loadPageUC_loading").hide();
}

function HideShowHiddenRequiredSpan() {
    $(".required-field[style*='visibility:hidden']").attr("style", "display:none");
    $(".required-field[style*='visibility: hidden']").attr("style", "display:none");
    $(".required-field[style*='visibility: visible']").attr("style", "display:block");
    $(".required-field[style*='visibility:visible']").attr("style", "display:block");

    $("span[style*='visibility:hidden']").attr("style", "display:none");
    $("span[style*='visibility: hidden']").attr("style", "display:none");
    $("span[style*='visibility: visible']").attr("style", "display:block");
    $("span[style*='visibility:visible']").attr("style", "display:block");
}

function validateGroups(listGroups) {
    var invalidIdxs = [];
    var result = true;

    // run validation from each group and remember failures
    for (var g = 0; g < listGroups.length; g++) {
        result = Page_ClientValidate(listGroups[g]) && result;
        for (var v = 0; v < Page_Validators.length; v++)
            if (!Page_Validators[v].isvalid)
                invalidIdxs.push(v);
    }

    // re-show any failures
    for (var i = 0; i < invalidIdxs.length; i++) {
        ValidatorValidate(Page_Validators[invalidIdxs[i]]);
    }

    // return false if any of the groups failed
    return result;
}

function focusErrorElement() {
    HideShowHiddenRequiredSpan();
    var errorElements = $('.err');
    for (var i = 0; i < errorElements.length; i++) {
        if ($(errorElements[i]).attr('style') == 'display:block' || $(errorElements[i]).attr('style') == 'display:block;visiblity:visible') {
            if ($(errorElements[i]).parent().find('.chosen-focus-input').length > 0) {
                // $(window).scrollTop($(errorElements[i]).parent().find('.chosen-focus-input').position().top);
                $(errorElements[i]).parent().find('.chosen-focus-input').focus();
            }
            else {
                // $(window).scrollTop($(errorElements[i]).prev().position.top);
                $(errorElements[i]).prev().focus();
            }
            return;
        }
    }
}

function open_panel() {
    slideIt();
    var a = document.getElementById("sidebarpanelright");
    a.setAttribute("id", "sidebar1panelright");
    a.setAttribute("onclick", "close_panel()");
}

function slideIt() {
    var slidingDiv = document.getElementById("sliderpanelright");
    var stopPosition = 0;

    if (parseInt(slidingDiv.style.right) < stopPosition) {
        slidingDiv.style.right = parseInt(slidingDiv.style.right) + 2 + "px";
        setTimeout(slideIt, 1);
    }
}

function close_panel() {
    slideIn();
    a = document.getElementById("sidebar1panelright");
    a.setAttribute("id", "sidebarpanelright");
    a.setAttribute("onclick", "open_panel()");
}

function slideIn() {
    var slidingDiv = document.getElementById("sliderpanelright");
    var stopPosition = -360;

    if (parseInt(slidingDiv.style.right) > stopPosition) {
        slidingDiv.style.right = parseInt(slidingDiv.style.right) - 2 + "px";
        setTimeout(slideIn, 1);
    }
}

function autoSlideSideBar() {
    var viewportWidth = $(window).width();
    if (viewportWidth > 1023) {
        var i = 0;
        $('.show-bkic li').removeClass("active");
        var $target = $('.show-bkic li');
        if (ishomePage == true) {
            $target.eq(0).addClass('active');
        }
        intervalSlide = setInterval(function () {
            $target.removeClass('active');
            $target.eq(i).addClass('active');
            if (i == $target.length - 1) i = 0;
            else i++;
        }, 3000);
    }
}

function dataLoadAchievements(element) {
    var reference = $(element).attr("data-ref");
    if ($("#achivements-wrapper ul#dates li").hasClass("active")) {
        $("#achivements-wrapper ul#dates li").removeClass("active");
    }

    $(element).closest("li").addClass("active");
    $("#achivements-wrapper ul#issues li[id]").hide();
    if (!$("#achivements-wrapper ul#issues li[id='" + reference + "']").is(":visible")) {
        $("#achivements-wrapper ul#issues li[id='" + reference + "']").show();
    }
}
setTimeout(function () {
    showPopupAfterLoad();
}, 1500);

function showPopupAfterLoad() {
    if (ishomePage == true) {
        if (sessionStorage["PopupShown"] != 'yes') {
            $("#popupcontainer").show();
        }
    }
}


$('.menus-bar-mobile').click(function () {
    $('.navbar-nav.side-menu-mobile').toggleClass('open')
});

$('.menus-bar-mobile').click(function () {
    $('.mobile-menu-view').toggleClass('open')
});

$('a.ca-more').click(function () {
    $('.ca-content-wrapper').addClass('open');
});

$('.ca-close').click(function () {
    $('.ca-content-wrapper').removeClass('open');
});

$(document).on('keydown', '.numbersonly', function (e) { -1 !== $.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) || (/65|67|86|88/.test(e.keyCode) && (e.ctrlKey === true || e.metaKey === true)) && (!0 === e.ctrlKey || !0 === e.metaKey) || 35 <= e.keyCode && 40 >= e.keyCode || (e.shiftKey || 48 > e.keyCode || 57 < e.keyCode) && (96 > e.keyCode || 105 < e.keyCode) && e.preventDefault() });

$(document).on("click", ".form-cancel-button", function () {
    if ($(".content-1") != undefined) {
        $(".content-1").hide();
        if ($("#btn1").hasClass("active")) {
            $("#btn1").removeClass("active");
        }
    }

    if ($(".content-2") != undefined) {
        $(".content-2").hide();
        if ($("#btn2").hasClass("active")) {
            $("#btn2").removeClass("active");
        }
    }
});

$(".policydate").datepicker({
    onSelect: function (d, i) {
        if (d !== i.lastVal) {
            $(this).change();
        }
    }
}).attr('readOnly', 'true');

function setCancelDateDate() {

    $(".cancel-date").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: "dd/mm/yy",
        minDate: $("#ContentPlaceHolder1_todayDate").val(),
        maxDate: $("#ContentPlaceHolder1_expireDate").val(),
        //endDate: '+0d',
        autoclose: true
    }).attr('readOnly', 'true');

};

function setExtendDate() {

    $(".extend-date").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: "dd/mm/yy",
        minDate: $("#ContentPlaceHolder1_extendStartDate").val(),
        // maxDate: $("#ContentPlaceHolder1_expireDate").val(),
        //endDate: '+0d',
        autoclose: true
    }).attr('readOnly', 'true');

};

function setRenewalDate() {
    $(".renewal-date").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: "dd/mm/yy",
        minDate: $("#ContentPlaceHolder1_txtRenewalPeriodFrom").val(),
        // maxDate: $("#ContentPlaceHolder1_expireDate").val(),
        //endDate: '+0d',
        autoclose: true
    }).attr('readOnly', 'true');
}

function Set21Years(element) {
    var id = element.id;
    var idelment = "#" + id;
    var currentYear = (new Date()).getFullYear();
    var yearBefore21 = (new Date()).getFullYear() - 21;
    $(idelment).datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: "dd/mm/yy",
        yearRange: yearBefore21 + ":" + currentYear,
    }).attr('readOnly', 'true');

}

function Set100Years(element) {
    var id = element.id;
    var idelment = "#" + id;
    var currentYear = (new Date()).getFullYear();
    var yearBefore100 = (new Date()).getFullYear() - 100;
    $(idelment).addClass('hasDatePicker');
    $(idelment).datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: "dd/mm/yy",
        yearRange: yearBefore100 + ":" + currentYear,
    }).attr('readOnly', 'true');

}

function SetAbove21Years(element) {
    var id = element.id;
    var idelment = "#" + id;
    var currentYear = (new Date()).getFullYear();
    var yearBefore21 = (new Date()).getFullYear() - 21;
    var yearBefore100 = yearBefore21 - 100;
    $(idelment).datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: "dd/mm/yy",
        yearRange: yearBefore100 + ":" + yearBefore21,
    }).attr('readOnly', 'true');

}
