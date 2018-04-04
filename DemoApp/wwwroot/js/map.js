var tripTime;
var originId;
var destId;
var map;
var adh;

  function initMap() {
    map = new google.maps.Map(document.getElementById('map'), {
      mapTypeControl: false,
      center: { lat: 39.580745, lng: -119.723402 },
      zoom: 13
    });

    adh = new AutocompleteDirectionsHandler(map);
  };

  function AutocompleteDirectionsHandler(map) {
    this.map = map;
    this.originPlaceId = null;
    this.destinationPlaceId = null;
    this.travelMode = 'WALKING';
    var originInput = document.getElementById('origin-input');
    var destinationInput = document.getElementById('destination-input');
    var modeSelector = document.getElementById('mode-selector');
    this.directionsService = new google.maps.DirectionsService;
    this.directionsDisplay = new google.maps.DirectionsRenderer;
    this.directionsDisplay.setMap(map);

    var originAutocomplete = new google.maps.places.Autocomplete(
      originInput, { placeIdOnly: true });
    var destinationAutocomplete = new google.maps.places.Autocomplete(
      destinationInput, { placeIdOnly: true });

    this.setupPlaceChangedListener(originAutocomplete, 'ORIG');
    this.setupPlaceChangedListener(destinationAutocomplete, 'DEST');

    this.map.controls[google.maps.ControlPosition.TOP_LEFT].push(originInput);
    this.map.controls[google.maps.ControlPosition.TOP_LEFT].push(destinationInput);
    this.map.controls[google.maps.ControlPosition.TOP_LEFT].push(modeSelector);
  }

  AutocompleteDirectionsHandler.prototype.setupClickListener = function (id, mode) {
    var radioButton = document.getElementById(id);
    var me = this;
    radioButton.addEventListener('click', function () {
      me.travelMode = mode;
      me.route();
    });
  };

  AutocompleteDirectionsHandler.prototype.setupPlaceChangedListener = function (autocomplete, mode) {
    var me = this;
    autocomplete.bindTo('bounds', this.map);
    autocomplete.addListener('place_changed', function () {
      var place = autocomplete.getPlace();
      if (!place.place_id) {
        window.alert("Please select an option from the dropdown list.");
        return;
      }
      if (mode === 'ORIG') {
        me.originPlaceId = place.place_id;
      } else {
        me.destinationPlaceId = place.place_id;
      }
      me.route();
    });

  };

  AutocompleteDirectionsHandler.prototype.route = function () {
    if (!this.originPlaceId || !this.destinationPlaceId) {
      return;
    }
    var me = this;

    originId = this.originPlaceId;
    destId = this.destinationPlaceId;

    this.directionsService.route({
      origin: { 'placeId': this.originPlaceId },
      destination: { 'placeId': this.destinationPlaceId },
      travelMode: 'DRIVING'
    }, function (response, status) {
      if (status === 'OK') {
        me.directionsDisplay.setDirections(response);
        var departureTime = $('#DepartureTime').val();
        if (departureTime.length > 0) {
          var departureDate = new Date($('#DepartureTime').val());
          var returnDate = dateAdd(departureDate, 'second', (response.routes[0].legs[0].duration.value * 2));
          var returnTime = formatTime(returnDate);
          var deliveryTime = formatTime(dateAdd(departureDate, 'second', response.routes[0].legs[0].duration.value));
          $('#ReturnTime').val(returnTime);
          $('#DeliveryTime').val(deliveryTime);
        }
        tripTime = response.routes[0].legs[0].duration.value;
      } else {
        window.alert('Directions request failed due to ' + status);
      }
    });
  };

AutocompleteDirectionsHandler.prototype.centerMap = function (lat, lng) {
  this.map.setCenter({ lat: lat, lng: lng });
}

AutocompleteDirectionsHandler.prototype.clearRoute = function () {
  this.directionsDisplay.setDirections({ routes: [] });
}

function centerMap(lat, lng) {
  map.setCenter({ lat: lat, lng: lng });
};

function dateAdd(date, interval, units) {
  var ret = new Date(date); //don't change original date
  var checkRollover = function () { if (ret.getDate() != date.getDate()) ret.setDate(0); };
  switch (interval.toLowerCase()) {
    case 'year': ret.setFullYear(ret.getFullYear() + units); checkRollover(); break;
    case 'quarter': ret.setMonth(ret.getMonth() + 3 * units); checkRollover(); break;
    case 'month': ret.setMonth(ret.getMonth() + units); checkRollover(); break;
    case 'week': ret.setDate(ret.getDate() + 7 * units); break;
    case 'day': ret.setDate(ret.getDate() + units); break;
    case 'hour': ret.setTime(ret.getTime() + units * 3600000); break;
    case 'minute': ret.setTime(ret.getTime() + units * 60000); break;
    case 'second': ret.setTime(ret.getTime() + units * 1000); break;
    default: ret = undefined; break;
  }
  return ret;
}

function formatTime(date) {
  return date.getFullYear() + '/' + ('0' + (date.getMonth() + 1)).slice(-2) + '/' + ('0' + date.getDate()).slice(-2) + ' ' + ('0' + date.getHours()).slice(-2) + ':' + ('0' + date.getMinutes()).slice(-2);
}