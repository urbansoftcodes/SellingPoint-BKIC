<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="BKIC.SellingPoint.Presentation.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>BKIC</title>
    <!-- custom-theme -->
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="keywords" content="Esteem Responsive web template, Bootstrap Web Templates, Flat Web Templates, Android Compatible web template,
	Smartphone Compatible web template, free webdesigns for Nokia, Samsung, LG, SonyEricsson, Motorola web design" />
    <script type="application/x-javascript"> addEventListener("load", function() { setTimeout(hideURLbar, 0); }, false);
			function hideURLbar(){ window.scrollTo(0,1); } </script>
    <!-- //custom-theme -->   
    <link href="assets/css/component.css" rel="stylesheet" type="text/css" media="all" />  
    <link href="assets/css/style_grid.css" rel="stylesheet" type="text/css" media="all" />    
    <link href="css/style.css" rel="stylesheet" type="text/css" media="all" />
    <link href="assets/css/style.css" rel="stylesheet" type="text/css" media="all" />
</head>
<body>
    <div>
        <div class="wthree_agile_admin_info">
            <div class="w3_agileits_top_nav">
                <ul id="gn-menu" class="gn-menu-main">

                    <li class="second logo admin">
                        <h1><a href="#">
                            <i class="fa fa-graduation-cap" aria-hidden="true"></i>BKIC </a></h1>
                    </li>
                    <li class="second w3l_search admin_login"></li>
                </ul>
            </div>
            <div class="clearfix"></div>
            <!-- //w3_agileits_top_nav-->

            <div class="inner_content">
                <!-- /inner_content_w3_agile_info-->
                <div class="inner_content_w3_agile_info">
                    <div class="registration admin_agile">
                        <div class="signin-form profile admin">
                            <h2>Selling Point </h2>
                            <div class="login-form">
                                <form runat="server">
                                    <asp:TextBox ID="txtUserName" class="numbersOnly" runat="server" placeholder="User Name" type="text"></asp:TextBox>
                                    <span class="err">
                                        <asp:RequiredFieldValidator ID="rfvCPR" ControlToValidate="txtUserName" ValidationGroup="login" runat="server" ErrorMessage="Please enter user name"></asp:RequiredFieldValidator>
                                    </span>
                                    <input type="password" runat="server" autocomplete="off" id="txtLoginPassword" />
                                    <span class="err">
                                        <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="txtLoginPassword" ErrorMessage="Password is required." ToolTip="Password is required." ValidationGroup="login">Password is required.</asp:RequiredFieldValidator>
                                    </span>
                                    <div>
                                        <asp:Literal ID="ErrorMessage" runat="server"></asp:Literal>
                                    </div>

                                    <div class="tp">
                                        <asp:Button ID="btnLogin" ValidationGroup="login" runat="server" type="button" Text="LOGIN" class="btn btn-buttons" OnClick="btnLogin_Click" />
                                    </div>
                                </form>
                            </div>
                        </div>
                    </div>
                    <!-- //inner_content_w3_agile_info-->
                </div>
                <!-- //inner_content-->
            </div>

            <!--copy rights start here-->
            <div class="copyrights">
                <p>© 2018 BKIC. All Rights Reserved</p>
            </div>
            <!--copy rights end here-->
            <!-- js -->            
            <script src="assets/customjs/modernizr.custom.js"></script>
            <script src="assets/customjs/classie.js"></script>
            <script src="assets/customjs/gnmenu.js"></script>
            <script>
                new gnMenu(document.getElementById('gn-menu'));
            </script>
            <script type="text/javascript" src="https://maps.googleapis.com/maps/api/js"></script>
            <script type="text/javascript">
                google.maps.event.addDomListener(window, 'load', init);
                function init() {
                    var mapOptions = {
                        zoom: 11,
                        center: new google.maps.LatLng(40.6700, -73.9400),
                        styles: [{ "featureType": "all", "elementType": "labels.text.fill", "stylers": [{ "saturation": 36 }, { "color": "#000000" }, { "lightness": 40 }] }, { "featureType": "all", "elementType": "labels.text.stroke", "stylers": [{ "visibility": "on" }, { "color": "#000000" }, { "lightness": 16 }] }, { "featureType": "all", "elementType": "labels.icon", "stylers": [{ "visibility": "off" }] }, { "featureType": "administrative", "elementType": "geometry.fill", "stylers": [{ "color": "#000000" }, { "lightness": 20 }] }, { "featureType": "administrative", "elementType": "geometry.stroke", "stylers": [{ "color": "#000000" }, { "lightness": 17 }, { "weight": 1.2 }] }, { "featureType": "landscape", "elementType": "geometry", "stylers": [{ "color": "#000000" }, { "lightness": 20 }] }, { "featureType": "poi", "elementType": "geometry", "stylers": [{ "color": "#000000" }, { "lightness": 21 }] }, { "featureType": "road.highway", "elementType": "geometry.fill", "stylers": [{ "color": "#000000" }, { "lightness": 17 }] }, { "featureType": "road.highway", "elementType": "geometry.stroke", "stylers": [{ "color": "#000000" }, { "lightness": 29 }, { "weight": 0.2 }] }, { "featureType": "road.arterial", "elementType": "geometry", "stylers": [{ "color": "#000000" }, { "lightness": 18 }] }, { "featureType": "road.local", "elementType": "geometry", "stylers": [{ "color": "#000000" }, { "lightness": 16 }] }, { "featureType": "transit", "elementType": "geometry", "stylers": [{ "color": "#000000" }, { "lightness": 19 }] }, { "featureType": "water", "elementType": "geometry", "stylers": [{ "color": "#000000" }, { "lightness": 17 }] }]
                    };
                    var mapElement = document.getElementById('map');
                    var map = new google.maps.Map(mapElement, mapOptions);
                    var marker = new google.maps.Marker({
                        position: new google.maps.LatLng(40.6700, -73.9400),
                        map: map,
                    });
                }
            </script>
            <script src="assets/customjs/prettymaps.js"></script>
            <script>

                $(function () {
                    //default
                    $('.map-canvas').prettyMaps({
                        address: 'Melbourne, Australia',
                        image: 'map-icon.png',
                        hue: '#FF0000',
                        saturation: -20
                    });

                    //red map example
                    $('#default-map-btn').on('click', function () {
                        $('.map-canvas').prettyMaps();
                    });

                    //green map example
                    $('#green-map-btn').on('click', function () {
                        $('.map-canvas').prettyMaps({
                            address: 'Melbourne, Australia',
                            image: 'map-icon.png',
                            hue: '#00FF55',
                            saturation: -30
                        });
                    });

                    //blue map example
                    $('#blue-map-btn').on('click', function () {
                        $('.map-canvas').prettyMaps({
                            address: 'Melbourne, Australia',
                            image: 'map-icon.png',
                            hue: '#0073FF',
                            saturation: -30,
                            zoom: 16,
                            panControl: true,
                            zoomControl: true,
                            mapTypeControl: true,
                            scaleControl: true,
                            streetViewControl: true,
                            overviewMapControl: true,
                            scrollwheel: false,
                        });
                    });

                    //grey map example
                    $('#grey-map-btn').on('click', function () {
                        $('.map-canvas').prettyMaps({
                            address: 'Melbourne, Australia',
                            image: 'map-icon.png',
                            saturation: -100,
                            lightness: 10
                        });
                    });
                });
            </script>
            <!-- //js -->
            <script src="assets/customjs/screenfull.js"></script>
            <script>
                $(function () {
                    $('#supported').text('Supported/allowed: ' + !!screenfull.enabled);

                    if (!screenfull.enabled) {
                        return false;
                    }

                    $('#toggle').click(function () {
                        screenfull.toggle($('#container')[0]);
                    });
                });
            </script>
            <script src="assets/customjs/jquery.nicescroll.js"></script>
            <script src="assets/customjs/scripts.js"></script>        
        </div>
    </div>
</body>
</html>