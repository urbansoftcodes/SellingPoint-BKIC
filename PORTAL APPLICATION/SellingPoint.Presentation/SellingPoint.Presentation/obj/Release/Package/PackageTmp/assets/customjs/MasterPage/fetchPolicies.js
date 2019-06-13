$(document).ready(function () {
    function autocompleteCPR() {
        $(document).find('[id*=txtCPRSearch]').autocomplete({
            minLength: 3
            , source: function (request, response) {
                $.ajax({
                    url: "Http://localhost:7050" + "/api/admin/getagencycpr/" + request.term + "/" + "BBK",
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
        });
    }

    function getDomesticPolicies() {
        $(document).find('[id*=txtDomesticPolicySearch]').autocomplete({
            minLength: 3
            , source: function (request, response) {
                $.ajax({
                    url: "Http://localhost:7050" + "/api/domestichelp/GetDomesticAgencyPolicy/",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json",
                    data: JSON.stringify({
                        'agency': 'BBK',
                        'agentCode': 'B0900001',
                        'isRenewal': 0,
                        'agentBranch': 'FMA',
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
        $(document).find('[id*=txtTravelPolicySearch]').autocomplete({
            minLength: 3
            , source: function (request, response) {
                $.ajax({
                    url: "Http://localhost:7050" + "/api/travelinsurance/getAgencyPolicy/",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json",
                    data: JSON.stringify({
                        'agency': 'BBK',
                        'agentCode': 'B0900001',
                        'isRenewal': 0,
                        'agentBranch': 'FMA',
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
        var map = {};
        $(document).find('[id*=txtHomePolicySearch]').autocomplete({
            minLength: 3
            , source: function (request, response) {
                $.ajax({
                    url: "Http://localhost:7050" + "/api/home/GetHomeAgencyPolicy/",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json",
                    data: JSON.stringify({
                        'agency': 'BBK',
                        'agentCode': 'B0900001',
                        'isRenewal': 0,
                        'agentBranch': 'FMA',
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
        var map = {};
        $(document).find('[id*=txtMotorPolicySearch]').autocomplete({
            minLength: 3
            , source: function (request, response) {
                $.ajax({
                    url: "Http://localhost:7050" + "/api/motor/GetMotorAgencyPolicy/",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json",
                    data: JSON.stringify({
                        'agency': 'BBK',
                        'agentCode': 'B0900001',
                        'isRenewal': 0,
                        'agentBranch': 'FMA',
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
            }
        });
    }
});


